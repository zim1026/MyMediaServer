<%@ Page Title="User Playlist" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Playlist.aspx.cs" Inherits="Web.Playlist" ClientIDMode="Static" %>
<%@ MasterType virtualpath="~/Site.Master" %>
<%@ Register TagPrefix="TE" TagName="TagEditor" Src="TagEditor.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
    <link type="text/css" rel="stylesheet" href="Content/Playlist.css" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="container">
        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <div class="col-sm-12" style="padding-top:20px; padding-bottom:15px">
                        <table style="margin-left:auto; margin-right:auto">
                            <tr>
                                <td colspan="4" style="padding-bottom:10px">
                                    <audio id="AudioPlayer" preload="metadata" controls="controls" title="PlayList Audio Player">
                                        This browser does not support HTML5 audio elements.
                                    </audio>
                                </td>
                            </tr>
                            <tr>
                                <td style="padding-right:10px">
                                    <button type="button" id="PlayAll" title="Play All Songs In Playlist"
                                        class="btn btn-primary" onclick="JavaScript:Playlist.PlayAll();">Play All</button>
                                </td>
                                <td style="padding-right:10px">
                                    <button type="button" id="Previous" title="Go Back to Previous Song"
                                        class="btn btn-default" onclick="JavaScript: Playlist.Previous();">Previous</button>
                                </td>
                                <td style="padding-right:30px">
                                    <button type="button" id="Next" title="Skip to Next Song"
                                        class="btn btn-default" onclick="JavaScript: Playlist.Next();">Next</button>
                                </td>
                                <td>
                                    <button type="button" id="RemoveSelected" title="Remove Selected Song From Playlist"
                                        class="btn btn-default" onclick="JavaScript:Playlist.RemoveSelection();">Remove From List</button>
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>

                <div class="row panel panel-default">
                    <div class="panel-heading" id="SongCount"></div>
                    <div class="row">
                        <div class="col-sm-12">
                            <asp:ListBox runat="server" ID="lstPlayListFiles" SelectionMode="Single" Rows="10" Width="100%" CssClass="form-control" />
                        </div>
                    </div>
                </div>

                <div class="row">
                    <div class="col-sm-12">
                        &nbsp;
                    </div>
                </div>

                <div class="row panel panel-default" id="NowPlaying">
                    <div class="panel-heading text-center">Now Playing</div>
                    <div class="row">
                        <div class="col-sm-12" style="font-size:12px">
                            <TE:TagEditor runat="server" ID="te" />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="Playlist.js"></script>
</asp:Content>
