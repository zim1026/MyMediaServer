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
                        data: 'ARTIST_NAME',
                        render: function (data, type, full) {
                            return '<a href="Default.aspx?ArtistID=' + full.ARTIST_ID + '" target="_blank" title="View all songs from this artist, in a new tab/window">' + data + '</a>';
                        }
                    },

                    {
                        className: 'text-center image-center',
                        data: 'ALBUM_NAME',
                        render: function (data, type, full) {
                            return '<div style="padding-bottom:5px"><a href="Default.aspx?AlbumID=' + full.ALBUM_ID + '"target="_blank" title="View all songs from this album in  a new tab/window">' + data + '</a></div>' +
                                '<img src="' + full.ART + '" alt="Album Cover Art" title="Album Cover Art" class="img-responsive img-rounded" style="margin-left:auto; margin-right:auto;" />';
                        }
                    },

                    {
                        className: 'text-center',
                        data: 'SONG_TITLE',
                        render: function (data, type, full) {
                            return '<a href="MediaHandler.ashx?SONG_ID=' + full.SONG_ID + '" target="_blank" title="Click to download this song">' + data + '</a>';
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
                // async: false,
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
                // async: false,
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
        },

        GetQueryStringParameters: function () {
            var match,
                pl = /\+/g,  // Regex for replacing addition symbol with a space
                search = /([^&=]+)=?([^&]*)/g,
                decode = function (s) { return decodeURIComponent(s.replace(pl, " ")); },
                query = window.location.search.substring(1),
                urlParams = {};

            //while (match = search.exec(query))
            while ((match = search.exec(query)) !== null)
                urlParams[decode(match[1])] = decode(match[2]);

            return urlParams;
        },

        GetArtistNameByID: function (artistID) {
            var _artistParams = {
                artistId: artistID
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetArtist',
                data: JSON.stringify(_artistParams),
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
                    $('#artistSearch').val(response.d.ARTIST_NAME);
                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });
        },

        GetAlbumNameFromID: function (albumID) {
            var _albumParams = {
                albumId: albumID
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetAlbum',
                data: JSON.stringify(_albumParams),
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
                    $('#albumSearch').val(response.d.ALBUM_NAME);
                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });
        }
    };
}());

$(document).ready(function () {
    'use strict';

    /*
    $.timeoutDialog({
        timeout: 25,    //session duration in seconds
        countdown: 10,  //number of seconds prior to expiration / when to alert user of pending expiration
        keep_alive_url: 'WebServices/WebService.asmx/KeepAlive',
        logout_url: 'Logout.aspx',
        logout_redirect_url: 'Logon.aspx',  //where to go after session expiration/logout
        restart_on_yes: true
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

    var qsParams = Default.GetQueryStringParameters();
    var artistID = qsParams['ArtistID'];
    var albumID = qsParams['AlbumID'];

    if (!artistID && !albumID) {
        Default.GetSearchSummary();
    }
    else if (artistID) {
        Default.GetArtistNameByID(artistID);
    }
    else if (albumID) {
        Default.GetAlbumNameFromID(albumID);
    }

    if (artistID || albumID) {
        Default.QuerySongList();
    }

    $('#searchDateAdded').change(function () {
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
