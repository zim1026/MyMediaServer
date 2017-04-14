/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var Default = (function () {
    'use strict';

    return {
        QuerySongList: function () {
            if ($('#artistSearch').val() === '' &&
                $('#albumSearch').val() === '' &&
                $('#songSearch').val() === '' &&
                $('#genreSearch').val() === '' &&
                $('#searchDateAdded').val() === '')
            {
                alert('Please specify search criteria.');
                return false;
            }

            $('#SongListContainer').show();

            var _songListParameters = {
                artistSearch: $('#artistSearch').val(),
                albumSearch: $('#albumSearch').val(),
                songSearch: $('#songSearch').val(),
                genreSearch: $('#genreSearch').val(),
                dateAddedSearch: $('#searchDateAdded').val()
            };

            $('#SongListTable').DataTable({ displayLength: 50, order: [[2, 'asc'], [3, 'asc'], [4, 'asc']], searching: false, "bDestroy": true, 'bAutoWidth': false }).destroy();

            $('#SongListTable').DataTable({
                displayLength: 50,
                order: [[2, 'asc'], [3, 'asc'], [4, 'asc']],
                searching: false,
                serverSide: true,
                "bDestroy": true,
                'bAutoWidth': false,
                ajax: {
                    url: 'WebServices/WebService.asmx/GetSongList',
                    data: _songListParameters,
                    dataType: 'json',
                    type: 'POST',
                    converters: {
                        "text json": function (data) {
                            return $.parseJSON(data, true);
                        }
                    },
                    beforeSend: function () {
                        $("#loading").dialog('open');
                    },
                    error: function (jqXHR, exception) {
                        $("#loading").dialog('close');
                        alert(jqXHR.responseText);
                    }
                },

                columns: [
                    { data: 'DT_RowId', name: 'rowId', orderable: false, visible: false },

                    {
                        className: 'text-center',
                        data: 'AFP',
                        orderable: false,
                        render: function (data, type, full) {
                            return '<button class="edit-row btn btn-default" title="View/Edit Song Data" type="button" data-key="' + data + '?' + full.SONG_ID + '">Details</button>' +
                                '<br />' +
                                '<audio style="width:150px; padding-top:8px" controls="controls" preload="metadata" title="Listen" src="' + full.AudioFile + '">Audio Not Supported</audio>';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'ARTIST_NAME'
                    },

                    {
                        className: 'text-center image-center',
                        data: 'ALBUM_NAME',
                        render: function (data, type, full) {
                            return '<div style="padding-bottom:5px">' + data + '</div>' +
                                '<img src="' + full.ART + '" alt="Album Cover Art" title="Album Cover Art" class="img-responsive img-rounded" style="margin-left:auto; margin-right:auto;" />';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'SONG_TITLE',
                        render: function (data, type, full) {
                            return '<a href="MediaHandler.ashx?SONG_ID=' + full.SONG_ID + '" target="_blank" title="Click to Download">' + data + '</a>';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'DURATION'
                    },

                    {
                        className: 'text-center',
                        data: 'SONG_ID',
                        render: function (data, type, full) {
                            return '<input type="Checkbox" class="add-to-playlist" title="Add/Remove Song to/from Playlist" data-key="' + data + '" ' + (full.Playlisted ? "checked='checked'" : "")  + ' />';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'GENRE'
                    },

                    {
                        className: 'text-center',
                        data: 'CREATE_DATE'
                    }
                ],

                initComplete: function () {
                    $('#SongListTable tbody').off('click', 'button.edit-row');
                    $('#SongListTable tbody').on('click', 'button.edit-row', function () { Default.EditSong($(this).data('key')); });

                    $('#SongListTable tbody').off('click', 'input.add-to-playlist');
                    $('#SongListTable tbody').on('click', 'input.add-to-playlist', function () { Default.AddToPlaylist($(this).data('key'), $(this).is(":checked")); });

                    Default.GetSearchSummary();
                },

                drawCallback: function () {
                    $('#loading').dialog('close');
                }
            });

            return false;
        },

        AddToPlaylist: function(songID, checked){
            // alert(songID + ' checked: ' + checked);

            var _userPlaylistParams = {
                songID: songID,
                //userSecurityID: '1',
                add: checked
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/ManageUserPlaylist',
                data: JSON.stringify(_userPlaylistParams),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                converters: {
                    "text json": function (data) {
                        return $.parseJSON(data, true);
                    }
                },
                beforeSend: function () {
                    $("#loading").dialog('open');
                },
                success: function (response) {
                    //alert('added');
                    /*
                    $('#artistCount').html(response.d.ArtistCount);
                    $('#albumCount').html(response.d.AlbumCount);
                    $('#songCount').html(response.d.SongCount);
                    $('#genreCount').html(response.d.GenreCount);
                    */
                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    alert(jqXHR.responseText);
                }
            });

            return false;
        },

        EditSong: function (args) {
            var file = args.substring(0, args.indexOf('?'));
            var songID = args.substring(args.indexOf('?') + 1);

            $('#songID').val(songID);
            $('#identifier').val(file);
            $('#identifierType').val('filename');

            $('#TagEditorClose').show();

            TagEditor.GetTagData();

            var _dialogWidth = 1300;

            if ($(window).width() < _dialogWidth) {
                _dialogWidth = $(window).width();
            }

            $('#SongDialog').dialog({
                modal: true, resizable: true, width: _dialogWidth,
                title: 'Song Details (SongID: ' + songID + ', ' + file + ') ' + $(window).height() + 'x' + $(window).width()
            });

            return false;
        },

        GetSearchSummary: function () {

            var _searchSummaryParameters = {
                artistSearch: $('#artistSearch').val(),
                albumSearch: $('#albumSearch').val(),
                songSearch: $('#songSearch').val(),
                genreSearch: $('#genreSearch').val(),
                dateAddedSearch: $('#searchDateAdded').val()
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetSearchResultsSummary',
                data: JSON.stringify(_searchSummaryParameters),
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                converters: {
                    "text json": function (data) {
                        return $.parseJSON(data, true);
                    }
                },
                beforeSend: function () {
                    $("#loading").dialog('open');
                },
                success: function (response) {
                    $('#artistCount').html(response.d.ArtistCount);
                    $('#albumCount').html(response.d.AlbumCount);
                    $('#songCount').html(response.d.SongCount);
                    $('#genreCount').html(response.d.GenreCount);

                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            return false;
        },

        GetNewestSongDate: function () {
            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetNewestSongDate',
                data: {},
                contentType: 'application/json; charset=utf-8',
                dataType: 'json',
                type: 'POST',
                converters: {
                    "text json": function (data) {
                        return $.parseJSON(data, true);
                    }
                },
                beforeSend: function () {
                    $("#loading").dialog('open');
                },
                success: function (response) {
                    $('#LastUpdated').html(response.d);
                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            return false;
        },

        ResetForm: function () {
            $('#artistSearch').val('');
            $('#albumSearch').val('');
            $('#songSearch').val('');
            $('#genreSearch').val('');
            $('#searchDateAdded').val('');

            $('#SongListContainer').hide();

            Default.GetSearchSummary();
        }
    };
}());

$(document).ready(function () {
    'use strict';

    /*
    $('#MasterNavBarLinks').children('li').each(function (index) {
        $(this).removeClass('active');
    });
    */

    $('#MasterNavBarHomeLink').addClass('active');

    $('#artistSearch').val('');
    $('#albumSearch').val('');
    $('#songSearch').val('');
    $('#genreSearch').val('');
    $('#searchDateAdded').val('');

    $('#SongListContainer').hide();

    $("#searchDateAdded").datepicker();

    $("#loading").dialog({
        title: 'Processing...',
        width: 300,
        height: 220,
        resizable: false,
        modal: true,
        autoOpen: false,
        open: function (event, ui) {
            $(".ui-dialog-titlebar-close", ui.dialog | ui).hide();
        }
    });

    TagEditor.DocumentReady();

    Default.GetNewestSongDate();

    Default.GetSearchSummary();

    $('#searchDateAdded').change(function() {
        Default.QuerySongList();
        return false;
    });

    $('#Search').keypress(function (e) {
        if (e.keyCode === 13) {
            //e.preventDefault();
            Default.QuerySongList();
            return false;
        }
    });
});

(function ($) {
    'use strict';

    // JSON RegExp
    var rvalidchars = /^[\],:{}\s]*$/;
    var rvalidescape = /\\(?:["\\\/bfnrt]|u[0-9a-fA-F]{4})/g;
    var rvalidtokens = /"[^"\\\n\r]*"|true|false|null|-?\d+(?:\.\d*)?(?:[eE][+\-]?\d+)?/g;
    var rvalidbraces = /(?:^|:|,)(?:\s*\[)+/g;
    var dateISO = /\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}(?:[.,]\d+)?Z/i;
    var dateNet = /\/Date\((\d+)(?:-\d+)?\)\//i;

    // replacer RegExp
    var replaceISO = /"(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2})(?:[.,](\d+))?Z"/i;
    var replaceNet = /"\\\/Date\((\d+)(?:-\d+)?\)\\\/"/i;

    // determine JSON native support
    var nativeJSON = (window.JSON && window.JSON.parse) ? true : false;
    var extendedJSON = nativeJSON && window.JSON.parse('{"x":9}', function (k, v) { return "Y"; }) === "Y";

    var jsonDateConverter = function (key, value) {
        if (typeof (value) === "string") {
            if (dateISO.test(value)) {
                return new Date(value);
            }

            if (dateNet.test(value)) {
                return new Date(parseInt(dateNet.exec(value)[1], 10));
            }
        }

        return value;
    };

    $.extend({
        parseJSON: function (data, convertDates) {
            /// <summary>Takes a well-formed JSON string and returns the resulting JavaScript object.</summary>
            /// <param name="data" type="String">The JSON string to parse.</param>
            /// <param name="convertDates" optional="true" type="Boolean">Set to true when you want ISO/Asp.net dates to be auto-converted to dates.</param>

            // convertDates = convertDates === false ? false : true; // if you're lazy, uncomment this line to convert dates by default
            if (typeof data !== "string" || !data) {
                return null;
            }

            // Make sure leading/trailing whitespace is removed (IE can't handle it)
            data = $.trim(data);

            // Make sure the incoming data is actual JSON
            // Logic borrowed from http://json.org/json2.js
            if (rvalidchars.test(data
                .replace(rvalidescape, "@")
                .replace(rvalidtokens, "]")
                .replace(rvalidbraces, ""))) {
                // Try to use the native JSON parser
                if (extendedJSON || (nativeJSON && convertDates !== true)) {
                    return window.JSON.parse(data, convertDates === true ? jsonDateConverter : undefined);
                }
                else {
                    data = convertDates === true ?
                        data.replace(replaceISO, "new Date(parseInt('$1',10),parseInt('$2',10)-1,parseInt('$3',10),parseInt('$4',10),parseInt('$5',10),parseInt('$6',10),(function(s){return parseInt(s,10)||0;})('$7'))")
                            .replace(replaceNet, "new Date($1)") :
                        data;
                    return (new Function("return " + data))();
                }
            } else {
                $.error("Invalid JSON: " + data);
            }
        }
    });
})(jQuery);