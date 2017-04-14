<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="TagEditor.ascx.cs" Inherits="Web.TagEditor" ClientIDMode="Static" %>

<div id="tagDetails" class="container-fluid">
    <input type="hidden" id="identifier" />
    <input type="hidden" id="identifierType" />
    <input type="hidden" id="songID" />

    <div class="row">
        <div class="col-sm-4 col-xs-12">
            <div class="row">
                <div class="col-sm-12">
                    <audio id="audioPlayer" src="" controls="controls" title="HTML5 Audio Element">
                        Browser does not support HTML5 audio elements
                    </audio>
                </div>
            </div>

            <div class="row panel panel-default">
                <div class="panel-heading">Embed New Album Art</div>
                <div class="row">
                    <div class="col-sm-8 col-xs-12">
                        <input type="file" id="fileUpload" />
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-sm-12">
                    <img id="albumArt" alt="" src="" class="img-responsive img-rounded" />
                </div>
            </div>
        </div>

        <div class="col-sm-5 col-xs-12">
            <table class="table-condensed table-responsive">
                <tr>
                    <td style="text-align: right; font-weight: bold;">
                        <button type="button" id="editArtist" onclick="Javascript:TagEditor.OpenArtistDialog();"
                            class="btn btn-link" title="Modify Artist Name" style="padding:0; font-weight:bold">Artist</button>
                    </td>
                    <td style="width: 100%">
                        <input type="text" id="artistText" style="width: 100%" class="form-control" />
                        <select id="ddlArtist" style="width: 100%" class="form-control" ></select>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; font-weight: bold;">
                        <button type="button" id="editAlbum" onclick="Javascript:TagEditor.OpenAlbumDialog();"
                            class="btn btn-link" title="Modify Album Name" style="padding:0; font-weight:bold">Album</button>
                    </td>
                    <td>
                        <input type="text" id="albumText" style="width: 100%" class="form-control" />
                        <select id="ddlAlbum" style="width: 100%" class="form-control"></select>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="title">Song</label>
                    </td>
                    <td>
                        <input type="text" id="title" style="width: 100%" class="form-control" />
                    </td>
                </tr>

                <tr>
                    <td style="text-align: right">
                        <label for="genre">Genre</label>
                    </td>
                    <td>
                        <input type="text" id="genre" style="width: 100%" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="conductor">Conductor</label>
                    </td>
                    <td>
                        <input type="text" id="conductor" style="width: 100%" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="composers">Composers</label>
                    </td>
                    <td>
                        <input type="text" id="composers" style="width: 100%" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="performers">Performers</label>
                    </td>
                    <td>
                        <input type="text" id="performers" style="width: 100%" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="duration">Duration</label>
                    </td>
                    <td>
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <input type="text" id="duration" class="form-control" readonly />
                                </td>
                                <td style="text-align:right; padding-left:10px; padding-right:5px">
                                    <label for="year">Year</label>
                                </td>
                                <td>
                                    <input type="text" id="year" class="form-control" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:right">
                        <label for="track">Track</label>
                    </td>
                    <td>
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <input type="text" id="track" class="form-control" />
                                </td>
                                <td style="text-align:right; padding-left:10px; padding-right:5px">
                                    <label for="disc">Disc</label>
                                </td>
                                <td>
                                    <input type="text" id="disc" class="form-control" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="id3v1Artist">ID3v1&nbsp;Artist</label>
                    </td>
                    <td>
                        <input type="text" id="id3v1Artist" style="width: 100%" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="id3v1Album">ID3v1&nbsp;Album</label>
                    </td>
                    <td>
                        <input type="text" id="id3v1Album" style="width: 100%" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="id3v1Title">ID3v1&nbsp;Title</label>
                    </td>
                    <td>
                        <input type="text" id="id3v1Title" style="width: 100%" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="comments">Comments</label>
                    </td>
                    <td>
                        <input type="text" id="comments" style="width: 100%" class="form-control" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right">
                        <label for="sampleRate" class="control-label text-nowrap">Sample&nbsp;Rate</label>
                    </td>
                    <td>
                        <table style="width:100%">
                            <tr>
                                <td>
                                    <input type="text" id="sampleRate" class="form-control" style="font-size:11px" readonly />
                                </td>
                                <td style="text-align:right; padding-left:10px; padding-right:5px">
                                    <label for="bitrate">Bitrate</label>
                                </td>
                                <td>
                                    <input type="text" id="bitrate" class="form-control" style="font-size:11px" readonly />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center" colspan="2">
                        <!--<button type="button" onclick="Javascript:TagEditor.CheckDDLValues();">Test</button>-->
                        <button id="UpdateFileTag" onclick="Javascript:TagEditor.UpdateTag();" class="btn btn-primary">Update Tag Data</button>
                        &nbsp;
                        &nbsp;
                        <button type="button" id="TagEditorClose" class="btn btn-primary" onclick="Javascript:TagEditor.CloseTagEditorDialog();">Close</button>
                    </td>
                </tr>
                <tr>
                    <td style="text-align:center" colspan="2">
                        <button type="button" id="Playlister" class="btn btn-default" onclick="JavaScript: TagEditor.PlaylistSong();"></button>
                    </td>
                </tr>
            </table>
        </div>

        <div class="col-sm-3 col-xs-12 text-center">
            <label for="lyrics">Lyrics</label>
            <br />
            <textarea id="lyrics" rows="26" style="width: 100%" class="form-control" ></textarea>
        </div>
    </div>
</div>

<div id="artistDialog" class="container-fluid">
    <input type="hidden" id="artistDialogArtistID" />

    <table style="width:100%">
        <tr>
            <td style="text-align:right; width:5%; font-weight:bold">
                Artist&nbsp;Name&nbsp;&nbsp;
            </td>
            <td style="width:95%">
                <input type="text" id="artistDialogArtistName" style="width:100%;" />
            </td>
        </tr>
    </table>
    <br />
    <table style="width:100%">
        <tr>
            <td style="text-align:right; width:50%">
                <button type="button" class="btn btn-primary" onclick="Javascript:TagEditor.SaveArtistDialog();">Save</button>
                &nbsp;
            </td>
            <td style="width:50%">
                &nbsp;
                <button type="button" class="btn" onclick="Javascript:TagEditor.CloseArtistDialog();">Close</button>
            </td>
        </tr>
    </table>
</div>

<div id="albumDialog" class="container-fluid">
    <input type="hidden" id="albumDialogAlbumID" />

    <table style="width:100%">
        <tr>
            <td style="text-align:right; width:5%; font-weight:bold">
                Album&nbsp;Name&nbsp;&nbsp;
            </td>
            <td style="width:95%">
                <input type="text" id="albumDialogAlbumName" style="width:100%;" />
            </td>
        </tr>
    </table>
    <br />
    <table style="width:100%">
        <tr>
            <td style="text-align:right; width:50%">
                <button type="button" class="btn btn-primary" onclick="Javascript:TagEditor.SaveAlbumDialog();">Save</button>
                &nbsp;
            </td>
            <td style="width:50%">
                &nbsp;
                <button type="button" class="btn" onclick="Javascript:TagEditor.CloseAlbumDialog();">Close</button>
            </td>
        </tr>
    </table>
</div>

<script type="text/javascript" src="TagEditor.js"></script>

