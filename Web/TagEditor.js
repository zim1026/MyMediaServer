/// <reference path="C:\Work\jQueryTest\Web\Scripts/jquery-2.1.0-vsdoc.js" />

var TagEditor = (function () {
    'use strict';

    return {
        DocumentReady: function () {
            $('#songID').val('');
            $('#identifier').val('');
            $('#identifierType').val('');

            $('#tagDetails').hide();
            $('#artistDialog').hide();
            $('#albumDialog').hide();

            TagEditor.PopulateArtists();

            $('#SongDialog').on('dialogclose', function (event) {
                if (!$('#audioPlayer')[0].paused) {
                    $('#audioPlayer')[0].pause();
                }
            });

            $('#ddlArtist').change(function () {
                TagEditor.PopulateAlbums();
            });

            $('#fileUpload').on('change', function (e) {
                var files = e.target.files;
                if (files.length > 0) {
                    if (window.FormData !== undefined) {
                        var data = new FormData();
                        for (var x = 0; x < files.length; x++) {
                            //data.append("file" + x, files[x]);    //no need to sanitize the filename, probably
                            data.append(files[x].name, files[x]);
                        }

                        data.append("tagFile", $('#identifier').val());

                        $.ajax({
                            async: false,
                            type: "POST",
                            url: 'WebServices/WebService.asmx/UploadFile',
                            contentType: false,
                            processData: false,
                            data: data,
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
                    } else {
                        alert("This browser doesn't support HTML5 file uploads!");
                    }
                }
            });
        },

        PopulateArtists: function () {
            //alert('populating artists');

            var ddlArtists = $('#ddlArtist');
            //ddlArtists.empty().append('<option selected="selected" value="0">Select Artist. . .</option>');
            ddlArtists.empty().append('<option value="0">Select Artist. . .</option>');
            ddlArtists.append('<option value="-1">New Artist. . . .</option>');

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetArtists',
                data: '{}',
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
                        //alert(this['ARTIST_ID'] + ' = ' + this['ARTIST_NAME']);
                        ddlArtists.append($("<option></option>").val(this['ARTIST_ID']).html(this['ARTIST_NAME']));
                    });

                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            return false;
        },

        PopulateAlbums: function () {
            TagEditor.GetArtistAlbums($('#ddlArtist').val());

            return false;
        },

        GetArtistAlbums: function (artistId) {
            //alert(artistId);

            $('#ddlAlbum').show();
            $('#ddlAlbum').empty();

            var ddlAlbums = $('#ddlAlbum');

            ddlAlbums.append('<option value="-1">New Album. . . .</option>');

            if (artistId > 0) {
                var _albumParameters = {
                    artistId: artistId
                };

                $.ajax({
                    async: false,
                    url: 'WebServices/WebService.asmx/GetAlbums',
                    data: JSON.stringify(_albumParameters),
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
                        //alert('back from get-albums');

                        $.each(response.d, function () {
                            ddlAlbums.append($("<option></option>").val(this['ALBUM_ID']).html(this['ALBUM_NAME']));
                        });

                        $('#loading').dialog('close');
                    },
                    error: function (jqXHR, exception) {
                        $('#loading').dialog('close');

                        alert(jqXHR.responseText);
                    }
                });
            }

            //alert(ddlAlbums.children('option').length + ' albums found');

            if (ddlAlbums.children('option').length > 1) {
                ddlAlbums.append('<option selected="selected" value="0">Select Album. . .</option>');
            }

            return false;
        },

        GetTagData: function () {
            if ($('#identifierType').val() === 'filename') {
                TagEditor.GetFileTags($('#identifier').val());
            }

            return false;
        },

        GetFileTags: function (file) {
            //alert('top call');

            $('#tagDetails').show();

            if (!$('#audioPlayer')[0].paused) {
                $('#audioPlayer')[0].pause();
            }

            $('#songID').val('');
            $('#fileUpload').val('');
            $('#albumArt').attr('src', '');
            $('#artistText').val('');
            $('#albumText').val('');
            $('#title').val('');
            $('#genre').val('');
            $('#year').val('');
            $('#track').val('');
            $('#disc').val('');

            $('#id3v1Artist').val('');
            $('#id3v1Album').val('');
            $('#id3v1Title').val('');

            $('#duration').val('');
            $('#bitrate').val('');
            $('#sampleRate').val('');

            $('#conductor').val('');
            $('#composers').val('');
            $('#performers').val('');

            $('#comments').val('');
            $('#lyrics').val('');

            var _tagParameters = {
                file: file
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetID3TagInfo',
                data: JSON.stringify(_tagParameters),
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

                    //$('#audioPlayer').attr('src', response.d.audioFile)[0];   //works
                    $('#audioPlayer').attr('src', response.d.audioFile);        //also works
                    $('#audioPlayer').attr('type', 'audio/mp3'); 

                    $('#songID').val(response.d.songId);
                    $('#albumArt').attr('src', response.d.albumArtRelativePath);
                    $('#artistText').val(response.d.tag.Artist);
                    $('#albumText').val(response.d.tag.Album);
                    $('#title').val(response.d.tag.Title);
                    $('#genre').val(response.d.tag.Genre);
                    $('#year').val(response.d.tag.Year);
                    $('#track').val(response.d.tag.TrackNum);
                    $('#disc').val(response.d.tag.DiscNum);
                    $('#id3v1Artist').val(response.d.tag.id3v1Artist);
                    $('#id3v1Album').val(response.d.tag.id3v1Album);
                    $('#id3v1Title').val(response.d.tag.id3v1Title);
                    $('#duration').val(response.d.tag.Duration);
                    $('#bitrate').val(response.d.tag.Bitrate);
                    $('#sampleRate').val(response.d.tag.SampleRate);
                    $('#conductor').val(response.d.tag.Conductor);
                    $('#composers').val(response.d.tag.Composer);
                    $('#performers').val(response.d.tag.Performer);
                    $('#comments').val(response.d.tag.Comment);
                    $('#lyrics').val(response.d.tag.Lyrics);

                    /*
                    if(response.d.tag.AlbumArtist.length > 0){
                        TagEditor.SelectArtistNameFromList(response.d.tag.AlbumArtist);
                    }
                    */

                    $('#ddlArtist').val(response.d.artistId);
                    TagEditor.GetArtistAlbums(response.d.artistId);
                    $('#ddlAlbum').val(response.d.albumId);

                    if (response.d.artistId !== -1) {
                        $('#artistText').hide();
                        $('#ddlArtist').attr('disabled', false);
                        $('#ddlAlbum').attr('disabled', false); //don't disable the album ddl since we at least have an artist, and probably an album too

                        if (response.d.albumId !== -1) {
                            $('#albumText').hide();
                        }
                        else {
                            $('#albumText').show();
                        }
                    }
                    else {
                        //At this point we are dealing with an entirely new artist, and thus new album so disable the ddls
                        $('#ddlArtist').attr('disabled', true);
                        $('#artistText').show();
                        $('#artistText').val(response.d.tag.Artist);
                        
                        $('#ddlAlbum').attr('disabled', true);
                        $('#albumText').show();
                        $('#albumText').val(response.d.tag.Album);
                    }
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            TagEditor.GetPlaylistStatus();

            return false;
        },

        SelectArtistNameFromList: function (artistName) {
            //alert('looking for artist: ' + artistName.toUpperCase());

            var selectedValue = -2;
            $('#ddlArtist option').each(function () {
                var option = this;

                //alert(option.text.toUpperCase());

                if (option.text.toUpperCase() === artistName.toUpperCase()) {
                    //alert('found it');
                    selectedValue = option.value;
                    return false;
                }
            });

            if (selectedValue !== -2) {
                $('#ddlArtist').val(selectedValue);
                $('#artistText').hide();

                TagEditor.GetArtistAlbums(selectedValue);
                TagEditor.SelectAlbumNameFromList($('#albumText').val());
            }
            else {
                $('#ddlArtist').hide();
                $('#artistText').show();
            }

            return false;
        },

        SelectAlbumNameFromList: function (albumName) {
            //alert('looking for: ' + albumName.toUpperCase());

            var selectedValue = -2;
            $('#ddlAlbum option').each(function () {
                var option = this;

                //alert(option.text.toUpperCase());

                if (option.text.toUpperCase() === albumName.toUpperCase()) {
                    //alert('found it');
                    selectedValue = option.value;
                    return false;
                }
            });

            if (selectedValue !== -2) {
                $('#ddlAlbum').val(selectedValue);
                $('#albumText').hide();
            }
            else {
                //$('#ddlAlbum').hide();
                $('#albumText').show();
            }

            return false;
        },

        UpdateTag: function () {
            TagEditor.GetUpdateFileTag($('#identifier').val());
            return false;
        },

        GetUpdateFileTag: function (file) {
            var _tagParameters = {
                file: file
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/GetID3TagInfo',
                data: JSON.stringify(_tagParameters),
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

                    TagEditor.UpdateFileTag(file, response.d.tag);
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }

            });

            return false;
        },

        UpdateFileTag: function (file, tag) {
            var proceed = 0;
            var artist;

            if ($('#ddlArtist').val() > 0) {
                artist = $('#ddlArtist').children('option:selected').text();
            }
            else {
                //This else statement should only be applicable when we are editing a file that has been uploaded, but not processed

                artist = $('#artistText').val();

                if ($('#performers').val().length > 0 && $('#performers').val() !== artist) {
                    $('#performers').val(artist);
                }

                if ($('#composers').val().length > 0 && $('#composers').val() !== artist) {
                    $('#composers').val(artist);
                }
            }

            tag.Artist = artist;
            tag.AlbumArtist = artist;
            tag.Performer = $('#performers').val();
            tag.Composer = $('#composers').val();

            if ($('#ddlAlbum').val() > 0) {
                tag.Album = $('#ddlAlbum').children('option:selected').text();
            }
            else {
                tag.Album = $('albumText').val();
            }

            tag.Title = $('#title').val();
            tag.Genre = $('#genre').val();
            tag.YearString = $('#year').val();
            tag.TrackNumString = $('#track').val();
            tag.DiscNumString = $('#disc').val();
            tag.id3v1Artist = $('#id3v1Artist').val();
            tag.id3v1Album = $('#id3v1Album').val();
            tag.id3v1Title = $('#id3v1Title').val();
            tag.Conductor = $('#conductor').val();
            tag.Comment = $('#comments').val();
            tag.Lyrics = $('#lyrics').val();

            var _updateFileTagParameters = {
                file: file,
                tag: tag,
                songID: $('#songID').val(),
                albumArtFilename: $('#fileUpload').val()
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/UpdateFileTag',
                data: JSON.stringify(_updateFileTagParameters),
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

                    proceed = 1;
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }

            });

            if (proceed === 1) {
                // alert('success');
                TagEditor.PopulateArtists();
                TagEditor.GetTagData();
            }
            else {
                alert('Something broke while attempting to update file tags');
            }

            return false;
        },

        OpenArtistDialog: function () {
            if ($('#ddlArtist').val() > 0) {

                $('#artistDialogArtistID').val('');
                $('#artistDialogArtistName').val('');

                $('#artistDialogArtistID').val($('#ddlArtist').val());
                $('#artistDialogArtistName').val($('#ddlArtist').children('option:selected').text());

                var _dialogWidth = 600;

                if ($(window).width() < _dialogWidth) {
                    _dialogWidth = $(window).width();
                }

                $('#artistDialog').dialog({ modal: true, resizable: true, width: _dialogWidth, title: 'Edit Artist (ArtistID: ' + $('#artistDialogArtistID').val() + ')' });
            }
        },

        CloseArtistDialog: function() {
            $('#artistDialog').dialog('close');
        },

        SaveArtistDialog: function () {
            var proceed = 0;

            var _artistParameters = {
                artistID: $('#artistDialogArtistID').val(),
                artistName: $('#artistDialogArtistName').val(),
                filename: $('#identifier').val()
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/UpdateArtist',
                data: JSON.stringify(_artistParameters),
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
                    if (response.d.ARTIST_NAME === $('#artistDialogArtistName').val()) {
                        proceed = 1;
                    }
                    else {
                        alert('Artist update failed');
                    }

                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }

            });

            if (proceed === 1) {
                TagEditor.PopulateArtists();
                TagEditor.GetTagData();
                TagEditor.CloseArtistDialog();
            }
            else {
                alert('Something broke while attempting Artist update');
            }
            
            return false;
        },

        OpenAlbumDialog: function () {
            if ($('#ddlArtist').val() > 0 && $('#ddlAlbum').val() > 0) {

                $('#albumDialogAlbumID').val('');
                $('#albumDialogAlbumName').val('');

                $('#albumDialogAlbumID').val($('#ddlAlbum').val());
                $('#albumDialogAlbumName').val($('#ddlAlbum').children('option:selected').text());

                var _dialogWidth = 700;

                if ($(window).width() < _dialogWidth) {
                    _dialogWidth = $(window).width();
                }

                $('#albumDialog').dialog({ modal: true, resizable: true, width: _dialogWidth, title: 'Edit Album (AlbumID: ' + $('#albumDialogAlbumID').val() + ')' });
            }
        },

        CloseAlbumDialog: function() {
            $('#albumDialog').dialog('close');
        },

        SaveAlbumDialog: function () {
            var proceed = 0;

            var _albumParameters = {
                albumID: $('#albumDialogAlbumID').val(),
                albumName: $('#albumDialogAlbumName').val(),
                filename: $('#identifier').val()
            };

            $.ajax({
                async: false,
                url: 'WebServices/WebService.asmx/UpdateAlbum',
                data: JSON.stringify(_albumParameters),
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
                    if (response.d.ALBUM_NAME === $('#albumDialogAlbumName').val()) {
                        proceed = 1;
                    }
                    else {
                        alert('Album update failed');
                    }

                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    $('#loading').dialog('close');

                    alert(jqXHR.responseText);
                }
            });

            if (proceed === 1) {
                TagEditor.PopulateArtists();
                TagEditor.GetTagData();
                TagEditor.CloseAlbumDialog();
            }
            else {
                alert('Something broke while attempting Album update');
            }

            return false;
        },

        CloseTagEditorDialog: function () {
            $('#SongDialog').dialog('close');
            return false;
        },

        CheckDDLValues: function () {
            alert('artist_id=' + $('#ddlArtist').val());
            alert('artist_text=' + $('#ddlArtist').children('option:selected').text());

            alert('album_id=' + $('#ddlAlbum').val());
            alert('album_text=' + $('#ddlAlbum').children('option:selected').text());
        },

        GetPlaylistStatus: function() {
            if ($('#songID').val().length > 0) {
                $('#Playlister').prop('disabled', false);

                var _playlistParams = {
                    songID: $('#songID').val()
                };

                $.ajax({
                    async: false,
                    url: 'WebServices/WebService.asmx/IsPlaylistedSong',
                    data: JSON.stringify(_playlistParams),
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

                        if (response.d) {
                            $('#Playlister').html('Remove From Playlist');
                        }
                        else {
                            $('#Playlister').html('Add To Playlist');
                        }
                    },
                    error: function (jqXHR, exception) {
                        $('#loading').dialog('close');

                        alert(jqXHR.responseText);
                    }
                });

            }
            else {
                $('#Playlister').prop('disabled', true);
            }
        },

        PlaylistSong: function () {
            var add;

            if ($('#Playlister').html().startsWith('Remove')) {
                add = 'false';
            }
            else {
                add = 'true';
            }

            var _userPlaylistParams = {
                songID: $('#songID').val(),
                add: add
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
                    if (add === 'false') {
                        $('#Playlister').html('Add To Playlist');       // just removed the song, so give the option to add it
                    }
                    else {
                        $('#Playlister').html('Remove From Playlist');  // just added the song, so give the option to remove it
                    }

                    $('#loading').dialog('close');
                },
                error: function (jqXHR, exception) {
                    alert(jqXHR.responseText);
                }
            });

            return false;
        }

    };
}());
