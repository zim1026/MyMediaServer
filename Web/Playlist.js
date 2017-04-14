/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var Playlist = (function () {
    'use strict';

    return{
        GetUserPlaylist: function () {
            $('#lstPlayListFiles').empty();
            var lstPlayListFiles = $('#lstPlayListFiles');

            /*
            var _userPlaylistParams = {
                userSecurityID: userSecurityID
            };
            */

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetUserPlaylist',
                data: {}, //JSON.stringify(_userPlaylistParams),
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
                    $.each(response.d, function () {
                        lstPlayListFiles.append($("<option></option>").val(this['AudioFile']).html(this['DisplayName']));

                        $('#SongCount').html($('#lstPlayListFiles option').length + ' songs  -  runtime: ' + this['RunTime']);
                    });

                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            $('#lstPlayListFiles').prop("selectedIndex", 0);

            return false;
        },

        PlayAll: function () {
            if (!$('#AudioPlayer')[0].paused) {
                $('#AudioPlayer')[0].pause();
            }

            var audio = $('#AudioPlayer');
            var playCount = $('#lstPlayListFiles').prop("selectedIndex");
            
            $('#AudioPlayer').attr('src', $('#lstPlayListFiles').val());
            audio[0].play();
            Playlist.GetSongInfo();

            audio[0].addEventListener('ended', function (e) {
                if (playCount === $('#lstPlayListFiles option').length -1) {
                    playCount = 0;
                }
                else {
                    playCount++;
                }

                var counter = 0;
                $('#lstPlayListFiles option').each(function () {
                    var option = this;

                    if (counter === playCount) {
                        $('#lstPlayListFiles').val(option.value);

                        $('#AudioPlayer').attr('src', option.value);
                        audio[0].play();

                        Playlist.GetSongInfo();

                        return false;
                    }

                    counter++;
                });
            });
        },

        Previous: function () {
            var index = $('#lstPlayListFiles').prop("selectedIndex");

            if (index === 0) {
                index = $('#lstPlayListFiles option').length - 1;
            }
            else {
                index--;
            }

            $('#lstPlayListFiles').prop("selectedIndex", index);
            Playlist.PlayAll();
        },

        Next: function () {
            var index = $('#lstPlayListFiles').prop("selectedIndex");

            if (index === $('#lstPlayListFiles option').length - 1) {
                index = 0;
            }
            else {
                index++;
            }

            $('#lstPlayListFiles').prop("selectedIndex", index);
            Playlist.PlayAll();
        },

        GetSongInfo: function () {
            $('#NowPlaying').show();

            $('#songID').val('');
            $('#identifier').val($('#lstPlayListFiles').val());
            $('#identifierType').val('filename');
            TagEditor.GetTagData();

            $('#audioPlayer').hide();
            $('#TagEditorClose').hide();
            $('#Playlister').hide();
            $('#editArtist').prop('disabled', true);
            $('#editAlbum').prop('disabled', true);
            $('#UpdateFileTag').prop('disabled', true);
            $('#fileUpload').prop('disabled', true);
            $('#uploadImage').prop('disabled', true);
            $('#ddlArtist').prop('disabled', true);
            $('#ddlAlbum').prop('disabled', true);

            return false;
        },

        RemoveSelection: function () {
            if (confirm('Are you sure you want to remove this song from your playlist?')) {
                if (!$('#AudioPlayer')[0].paused) {
                    $('#AudioPlayer')[0].pause();
                }

                var _userPlaylistParams = {
                    songID: $('#songID').val(),
                    //userSecurityID: '1',
                    add: 'false'
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
                        $('#loading').dialog('close');
                    },
                    error: function (jqXHR, exception) {
                        $('#loading').dialog('close');

                        alert(jqXHR.responseText);
                    }
                });

                TagEditor.DocumentReady();

                Playlist.GetUserPlaylist('1');

                $('#NowPlaying').hide();

                return false;
            }
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

    $('#MasterNavBarPlaylistLink').addClass('active');

    if (!$('#AudioPlayer')[0].paused) {
        $('#AudioPlayer')[0].pause();
    }

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

    $('#NowPlaying').hide();

    Playlist.GetUserPlaylist();

    Playlist.PlayAll();

    $('#lstPlayListFiles').change(function () {
        Playlist.PlayAll();
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