<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="Upload.aspx.cs" Inherits="Upload" %>
<%@ Register TagPrefix="CWC" Assembly="CustomWebControls" Namespace="CustomWebControls" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="masterContent" Runat="Server">
    <asp:Panel runat="server" ID="pnlError" Visible="false">
        <div class="error-content">
            <h3>ERROR</h3>
            <asp:Label runat="server" ID="lblError" />
        </div>
    </asp:Panel>
    
    <asp:Panel runat="server" ID="pnlWarning" Visible="false">
        <div class="warning-content">
            <h3>WARNING</h3>r                                                                                                               
            <asp:Label runat="server" ID="lblWarning" />
        </div>
    </asp:Panel>

            <center>    
    <div class="content">
                <table>
                    <tr>
                        <td align="center">
                <table width="100%">
                    <tr>
                        <td align="left">
                            <CWC:MultiFileUpload runat="server" ID="FileUploader" MaxFiles="25" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <table width="100%">
                                <tr>
                                    <td align="left">
                                        Limit of 25 files per upload.
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:LinkButton runat="server" ID="cmdUpload" Text="Upload" OnClick="cmdUpload_Click" />
                                    </td>
                                </tr>                        
                            </table>
                        </td>
                    </tr>

                </table>                        
                        </td>
                    </tr>
                </table>

            
    </div>
    </center>
</asp:Content>

