<%@ Page Title="File Management" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="FileUpload.aspx.cs" Inherits="Web.FileUpload" ClientIDMode="Static" %>
<%@ MasterType virtualpath="~/Site.Master" %>
<%@ Register TagPrefix="CWC" Assembly="CustomWebControls" Namespace="CustomWebControls" %>
<%@ Register TagPrefix="TE" TagName="TagEditor" Src="TagEditor.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-6 col-xs-12 panel panel-default" style="font-size:12px">
                <div class="panel-heading">Upload List (Max: 25 Files)</div>
                <div class="row">
                    <div class="col-sm-12">
                        <table>
                            <tr>
                                <td>
                                    <CWC:MultiFileUpload runat="server" ID="FileUploader" MaxFiles="25" />
                                </td>
                                <td style="vertical-align:top; padding-left:15px">
                                    <asp:Button runat="server" ID="cmdUpload" OnClick="cmdUpload_Click" Text="Upload" CssClass="btn btn-primary" />
                                </td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>

            <div class="col-sm-6 col-xs-12 panel panel-default" style="font-size:12px">
                <div class="panel-heading">Uploaded Songs</div>
                <div class="row">
                    <div class="col-sm-12" style="padding-bottom:4px;">
                        <button type="button" id="ProcessFile" class="btn btn-default" onclick="JavaScript: FileUpload.ProcessSelectedFile();"
                            title="Process the Selected Uploaded File">Process Selected File</button>
                        &nbsp;
                        <button type="button" id="ProcessFiles" class="btn btn-primary" onclick="JavaScript: FileUpload.ProcessAllFiles();"
                            title="Process All Uploaded Files">Process All Files</button>
                    </div>
                </div>
                <div class="row">
                    <div class="col-sm-12">
                        <asp:ListBox runat="server" ID="lstUploadedFiles" SelectionMode="Single" Rows="10" />
                    </div>
                </div>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12" style="font-size:12px">
                <TE:TagEditor runat="server" ID="te" />
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="FileUpload.js"></script>
</asp:Content>
