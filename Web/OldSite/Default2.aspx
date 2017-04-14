<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default2.aspx.cs" Inherits="jQueryTestWeb.Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>

        <script src="Scripts/jquery-3.1.1.min.js"></script>
        <script src="Scripts/jquery-ui-1.12.1.min.js"></script>
        <script src="Scripts/jquery.dataTables.min.js"></script>

        <script src="Default.js"></script>
    </head>
    <body>
        <form id="form1" runat="server">

            <a href="FileUpload.aspx" target="_blank">Upload</a>
            <br />
            <br />

            <div>
                <button type="button" onclick="JavaScript:Default.OpenArtistDialog();">Open</button>
                <br />
            </div>

            <div id="ArtistDialog">
                <div style="width: 675px;">
                    <table style="border-collapse: separate; color: Red; padding: 2px; border-spacing: 2px">
                        <colgroup>
                            <col style="width: 120px; text-align: right" />
                            <col style="color: #11D;" />
                        </colgroup>
                        <tr>
                            <td class="RWLabel">Artist Name</td>
                            <td class="RWText">
                                <input type="text" id="ArtistName" maxlength="200" style="width: 250px" />
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div id="AlbumDialog">
                <div style="width: 675px;">
                    <table style="border-collapse: separate; color: Red; padding: 2px; border-spacing: 2px;">
                        <colgroup>
                            <col style="width: 120px; text-align: right" />
                            <col style="color: #11D;" />
                        </colgroup>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </div>
            </div>

            <div id="SongDialog">
                <div style="width: 675px;">
                    <table style="border-collapse: separate; color: Red; padding: 2px; border-spacing: 2px">
                        <colgroup>
                            <col style="width: 120px; text-align: right" />
                            <col style="color: #11D;" />
                        </colgroup>
                    </table>
                </div>
            </div>
        </form>
    </body>
</html>
