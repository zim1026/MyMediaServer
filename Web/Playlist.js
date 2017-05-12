/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var Playlist = (function () {
    'use strict';

    return{
        GetUserPlaylist: function () {
            $('#lstPlayListFiles').empty();
            $('#SongCount').html('');
            $('#AudioPlayer').attr('src', '');

            var lstPlayListFiles = $('#lstPlayListFiles');

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetUserPlaylist',
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

            if ($('#lstPlayListFiles option').length > 0) {
                $('#lstPlayListFiles').prop("selectedIndex", 0);
            }
            else {
                $('#AudioPlayer').prop('disabled', true);
                $('#PlayAll').prop('disabled', true);
                $('#Previous').prop('disabled', true);
                $('#Next').prop('disabled', true);
                $('#RemoveSelected').prop('disabled', true);

                alert('You currently have no songs in your playlist.' + '\n\n' +
                      'Songs can be added to your playlist by searching for them from the Home screen, and clicking the "Playlist?" checkbox.');
            }

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

                Playlist.GetUserPlaylist();
                if ($('#lstPlayListFiles option').length > 0) {
                    Playlist.PlayAll();
                }
                else {
                    $('#NowPlaying').hide();
                }

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

    if ($('#lstPlayListFiles option').length > 0) {
        Playlist.PlayAll();
    }

    $('#lstPlayListFiles').change(function () {
        Playlist.PlayAll();
    });
});
