<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Web.Default" ClientIDMode="Static" %>
<%@ MasterType virtualpath="~/Site.Master" %>
<%@ Register TagPrefix="TE" TagName="TagEditor" Src="TagEditor.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div id="Search" class="container-fluid">
        <div class="row">
            <div class="col-sm-2 col-xs-12" style="font-size:12px">
                <div class="panel panel-default">
                    <div class="panel-heading text-center">
                        Summary
                    </div>
                    <div class="row">
                        <div class="col-sm-12">
                            <table style="margin-left:auto; margin-right:auto">
                                <tr>
                                    <td>
                                        # Artists:
                                    </td>
                                    <td style="padding-left:7px">
                                        <div id="artistCount"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        # Albums:
                                    </td>
                                    <td style="padding-left:7px">
                                        <div id="albumCount"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        # Songs:
                                    </td>
                                    <td style="padding-left:7px">
                                        <div id="songCount"></div>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        # Genres:
                                    </td>
                                    <td style="padding-left:7px">
                                        <div id="genreCount"></div>
                                    </td>
                                </tr>                                                                        
                            </table>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-8 col-xs-12" style="font-size:12px">
                <div class="row panel panel-default">
                    <div class="panel-heading text-center">Search Criteria</div>
                    <div class="col-sm-12">
                        <label class="control-label col-sm-2 text-right-not-xs" for="artistSearch" title="Artist Name">
                            Artist
                        </label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="artistSearch" title="Search by Artist Name (Partial Match)" />
                        </div>

                        <label class="control-label col-sm-2 text-right-not-xs" for="albumSearch" title="Album Name">
                            Album
                        </label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="albumSearch" title="Search by Album Name (Partial Match)" />
                        </div>
                        
                        <label class="control-label col-sm-2 text-right-not-xs" for="songSearch" title="Song Title">
                            Song
                        </label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="songSearch" title="Search by Song Title (Partial Match)" />
                        </div>
                        
                        <label class="control-label col-sm-2 text-right-not-xs" for="genreSearch" title="Music Genre">
                            Genre
                        </label>
                        <div class="col-sm-10">
                            <input type="text" class="form-control" id="genreSearch" title="Search by Genre (Partial Match)" />
                        </div>
                        
                        <label class="control-label col-sm-2 text-right-not-xs" for="searchDateAdded" title="Date Added">
                            Added
                        </label>

                        <div class="col-sm-10">
                            <input type="date" class="form-control" id="searchDateAdded" title="Search by Date Added (On and After Match)" />
                        </div>
                        <div class="col-sm-12 text-center">
                            <div style="padding-top:15px;" class="text-center">
                                <button type="button" id="search" class="btn btn-primary" onclick="JavaScript:Default.QuerySongList();">Search</button>
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <button type="button" id="reset" class="btn" onclick="JavaScript:Default.ResetForm();">Reset</button>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <div class="col-sm-2 col-xs-12">
                <div class="panel panel-default" style="font-size:12px">
                    <div class="panel-heading text-center">Last Updated</div>
                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <div id="LastUpdated"></div>
                        </div>
                    </div>
                    
                    <!--
                    <div class="row">
                        <div class="col-sm-12">
                            &nbsp;
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 text-center" style="padding-bottom:10px">
                            <a href="PlayList.aspx" target="_blank" class="btn btn-primary">Play List</a>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 text-center" style="padding-bottom:10px">
                            <a href="FileUpload.aspx" target="_blank" class="btn btn-primary" title="Upload / Process Uploaded Songs" style="width:79px">Files</a>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-12 text-center">
                            <a href="UserSecurity.aspx" target="_blank" class="btn btn-primary" title="Manage Users" style="width:79px">Users</a>
                        </div>
                    </div>
                    --> 

                </div>
            </div>

        </div>
    </div>
    
    <div id="SongListContainer" class="table-responsive" style="overflow-x:hidden; font-size:12px">
        <table id="SongListTable" class="dataTable table-bordered cell-border compact hover stripe">
            <thead class="dataTableHeader">
                <tr>
                    <th class="hidden">SongId</th>
                    <th><!--Details Button--></th>
                    <th>Artist</th>
                    <th>Album</th>
                    <th>Title</th>
                    <th>Duration</th>
                    <th>Playlisted</th>
                    <th>Genre</th>
                    <th>Added</th>
                </tr>
            </thead>
        </table>
    </div>

    <div id="SongDialog" style="font-size:12px">
        <TE:TagEditor runat="server" ID="te" />
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="Default.js"></script>
</asp:Content>
