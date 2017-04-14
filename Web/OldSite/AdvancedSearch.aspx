<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPage.master" AutoEventWireup="true" CodeFile="AdvancedSearch.aspx.cs" Inherits="AdvancedSearch" %>
<%@ MasterType VirtualPath="~/MasterPage.Master" %>
<%--<%@ Register TagPrefix="SD" TagName="SongData" Src="~/SongData.ascx" %>--%>
<%@ Register Assembly="ZNet.Controls" Namespace="ZNet.Controls" TagPrefix="ZNet" %>

<asp:Content ID="Content1" ContentPlaceHolderID="masterContent" Runat="Server">
    <ajaxToolkit:UpdatePanelAnimationExtender runat="server" ID="upae"
        TargetControlID="upnlMain" BehaviorID="Animations" Enabled="false">
        <Animations>
            <OnUpdating>
                <Sequence>
                    <Parallel duration=".25" Fps="90">
                        <FadeOut AnimationTarget="gvData" minimumOpacity=".4" />                    
                    </Parallel>    
                </Sequence>
            </OnUpdating>            

            <OnUpdated>
                <Sequence>
                    <Parallel duration=".25" Fps="90">
                        <FadeIn AnimationTarget="gvData" minimumOpacity=".4" />                        
                    </Parallel>
                </Sequence>
            </OnUpdated>
        </Animations>
    </ajaxToolkit:UpdatePanelAnimationExtender>

    <asp:UpdatePanel runat="server" ID="upnlMain">
        <ContentTemplate>
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

            <div class="content">
                <table width="100%">
                    <!--Update Animation-->
                    <tr>
                        <td align="center" style="width:100%">
                            <asp:UpdateProgress runat="server" ID="upnlMainStatus">
                                <ProgressTemplate>
                                    Searching...
                                    <br />
                                    <img src="../ajax-loader.gif" />
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                        </td>
                    </tr>
                    <tr>
                        <td align="center" style="width:100%">
                            <table width="100%">
                                <tr>
                                    <td style="font-weight:bold; width:10%" align="right">
                                        Artist
                                    </td>
                                    <td align="left" style="width:80%">
                                        <asp:TextBox runat="server" ID="txtArtistPhrase" Width="100%" />
                                    </td>
                                    <td align="left" valign="top" style="width:10%; white-space:nowrap">
                                        <asp:CheckBox runat="server" ID="chkMatchArtist" Checked="false" Text="Exact Match?" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-weight:bold; width:10%" align="right">
                                        Album
                                    </td>
                                    <td align="left" style="width:80%">
                                        <asp:TextBox runat="server" ID="txtAlbumPhrase" Width="100%" />
                                    </td>
                                    <td align="left" valign="top" style="width:10%; white-space:nowrap">
                                        <asp:CheckBox runat="server" ID="chkMatchAlbum" Checked="false" Text="Exact Match?" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-weight:bold; width:10%; white-space:nowrap" align="right">
                                        Song Title
                                    </td>
                                    <td align="left" style="width:80%">
                                        <asp:TextBox runat="server" ID="txtSongPhrase" Width="100%" />
                                    </td>
                                    <td align="left" valign="top" style="width:10%; white-space:nowrap">
                                        <asp:CheckBox runat="server" ID="chkMatchSong" Checked="false" Text="Exact Match?" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-weight:bold; width:10%" align="right">
                                        Genre
                                    </td>
                                    <td align="left" style="width:80%">
                                        <asp:TextBox runat="server" ID="txtGenre" Width="100%" />
                                    </td>
                                    <td align="left" valign="top" style="width:10%; white-space:nowrap">
                                        <asp:CheckBox runat="server" ID="chkGenre" Checked="false" Text="Exact Match?" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="font-weight:bold; width:10%" align="right">
                                        Year
                                    </td>
                                    <td align="left" style="width:80%">
                                        <asp:TextBox runat="server" ID="txtYear" Width="100%" />
                                    </td>
                                    <td align="left" valign="top" style="width:10%; white-space:nowrap">
                                        <asp:CheckBox runat="server" ID="chkYear" Checked="false" Text="Exact Match?" />
                                    </td>
                                </tr>                            
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td style="width:100%" align="center">
                            <table width="25%">
                                <tr>
                                    <td align="right" style="width:48%">
                                        <asp:LinkButton runat="server" ID="lnkSearch" Text="Search" OnClick="lnkSearch_Click" />
                                    </td>
                                    <td style="width:4%">
                                        &nbsp;
                                    </td>
                                    <td align="left" style="width:48%">
                                        <asp:LinkButton runat="server" ID="cmdReset" Text="Reset" onclick="cmdReset_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>

                    <tr>
                        <td align="center" style="width:100%">
                            <asp:Label runat="server" ID="lblRecCount" Text="" />
                        </td>
                    </tr>

                    <!--Grouping Notice-->
                    <tr runat="server" id="tblGroupRow" visible="false">
                        <td align="center" style="width:100%">
                            Songs are grouped by artist.
                        </td>
                    </tr>

                    <!--Zip Link-->
                    <asp:Panel runat="server" ID="pnlZipDownlaod" Visible="false">
                        <tr>
                            <td align="center" style="width:100%">
                                <asp:HyperLink runat="server" ID="lnkZipDownload" Text="Files.zip" />
                            </td>
                        </tr>
                    </asp:Panel>

                    <!--Pageer Size-->
                    <tr>
                        <td align="center" style="width:100%">
                            <table>
                                <tr>
                                    <td align="right">
                                        Records Per Page
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList runat="server" ID="ddlPageSize" AutoPostBack="true" OnSelectedIndexChanged="ddlPageSize_SelectedIndexChanged">
                                            <asp:ListItem Text="10" Value="10" />
                                            <asp:ListItem Text="25" Value="25" Selected="True" />
                                            <asp:ListItem Text="50" Value="50" />
                                            <asp:ListItem Text="100" Value="100" />
                                            <asp:ListItem Text="150" Value="150" />
                                            <asp:ListItem Text="200" Value="200" />
                                            <asp:ListItem Text="300" Value="300" />
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>
                </table>

                <asp:Panel ID="pnlSearchResultsHeader" runat="server" CssClass="collapsePanelHeader" Height="20px" BackColor="#000033" Width="100%">
                    <div style="cursor:pointer; width:100%">
                        <asp:Label ID="lblSearchResultsHeader" runat="server" Text="(Show Search Results...)" Width="100%" Font-Underline="true" />
                    </div>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlSearchResults" CssClass="collapsePanel" Height="0" BorderStyle="Solid" BorderWidth="1px" BorderColor="Black">
                    <ZNet:GridView runat="server" ID="gvData"
                        EnableGrouping="true" GroupingDataKeyIndex="1" 
                        AllowPaging="True" AllowSorting="True"
                        GroupHeaderBackColor="#999999" GroupHeaderForeColor="Black" GroupHeaderHeight="20"
                        DataKeyNames="song_id, artist_name_sort, album_name_sort" Width="100%"
                        AutoGenerateColumns="false" AutoGenerateDeleteButton="false"
                        AutoGenerateEditButton="false" AutoGenerateSelectButton="false"
                        
                        OnPageIndexChanging="gvData_PageIndexChanging"
                        OnSorting="gvData_Sorting"
                        OnRowCreated="gvData_RowCreated"
                        onselectedindexchanged="gvData_SelectedIndexChanged"
                        onrowcommand="gvData_RowCommand"
                        onrowdatabound="gvData_RowDataBound"
                        
                        BorderStyle="None" HeaderStyle-BorderStyle="None"
                        HeaderStyle-BorderWidth="0px" GridLines="Horizontal" RowStyle-Height="1" AlternatingRowStyle-Height="1"
                        CellPadding="2" CellSpacing="2"
                        Visible="true">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox runat="server" ID="chkSelect" Checked="false" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField ItemStyle-Width="0">
                                <ItemTemplate>
                                    <asp:Panel runat="server" ID="pnlHoverMenu" CssClass="popupLink">
                                        <table class="popupLink" cellpadding="0" cellspacing="0" style="width:290px; background-color:Black; border-style:ridge; border-width:6; border-color:White">
                                            <tr>
                                                <td colspan="2" style="width:100%; background-color:#CCCCCC">
                                                    <table width="100%">
                                                        <tr>
                                                            <td align="center" style="width:50%; background-color:#CCCCCC; color:Black; font-weight:bold; white-space:nowrap">
                                                                Current Song
                                                            </td>

                                                            <td align="center" style="width:50%; background-color:#CCCCCC; color:Black;font-weight:bold; white-space:nowrap;">
                                                                Selected Songs
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="left" style="width:44%; white-space:nowrap;
                                                                        background-color:#000033;
                                                                        border-top-style:ridge; border-top-color:Black;
                                                                        border-bottom-style:ridge; border-bottom-color:Black" valign="top">
                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" style="width:100%; overflow:hidden">
                                                                <asp:LinkButton runat="server" ID="cmdViewDetails" Text="View/Edit ID3 Tags" CssClass="popupLink" Width="100%"
                                                                    OnCommand="gvData_SelectedIndexChanged" CommandName="Select" CommandArgument="Select" Font-Underline="false" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td align="left" style="width:100%">
                                                                <asp:HyperLink runat="server" ID="lnkFilePath" NavigateUrl='<%# Eval("file_path") %>' Width="100%"
                                                                    Text="Download" CssClass="popupLink" Font-Underline="false" />
                                                            </td>
                                                        </tr>

                                                        <tr>
                                                            <td align="left" style="width:100%">
                                                                <asp:HyperLink runat="server" ID="lnkListen" NavigateUrl='<%# "~/Listen.aspx?SongID=" + Eval("song_id") %>' Target="_blank" Width="100%" Text="Listen" CssClass="popupLink" Font-Underline="false" />
                                                            </td>
                                                        </tr>

                                                        <tr runat="server" id="delete" visible='<%# IsAdmin() %>' style="width:100%">
                                                            <td align="left">
                                                                <asp:LinkButton runat="server" ID="cmdDeleteSong" Text="Delete Song" Width="100%" CssClass="popupLink" 
                                                                    CommandName="DeleteSong" Font-Underline="false" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>

                                                <td align="left" style="width:56%; white-space:nowrap;
                                                                        border-left-style:ridge; border-left-color:Black;
                                                                        border-bottom-style:ridge; border-bottom-color:Black;
                                                                        border-top-style:ridge; border-top-color:Black" valign="top">

                                                    <table width="100%" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left" style="width:100%">
                                                                <table width="100%" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td align="center" style="width:100%; background-color:Maroon">
                                                                            <table width="100%">
                                                                                <tr>
                                                                                    <td align="right" style="width:45%; background-color:Maroon">
                                                                                        <asp:LinkButton runat="server" ID="cmdSelectAll" onclick="cmdSelectAll_Click" Width="100%"
                                                                                            Text="Select All" CssClass="popupLink" Font-Underline="false" />
                                                                                    </td>            
                                                                        
                                                                                    <td style="width:10%">
                                                                                        &nbsp;
                                                                                    </td>

                                                                                    <td align="left" style="width:45%; background-color:Maroon">
                                                                                        <asp:LinkButton runat="server" ID="cmdClearSelection" Text="Clear" Width="100%"
                                                                                            onclick="cmdUnSelectAll_Click" CssClass="popupLink" Font-Underline="false" />                                                                        
                                                                                    </td>
                                                                                </tr>
                                                                    
                                                                            </table>
                                                                        </td>
                                                                    </tr>

                                                                    <tr>
                                                                        <td align="left" style="width:100%; background-color:Maroon">
                                                                            <asp:LinkButton runat="server" ID="cmdZipSelection" Text="Zip and Download" Width="100%"
                                                                                onclick="cmdZipAndDownload_Click" CssClass="popupLink" Font-Underline="false" />
                                                                        </td>
                                                                    </tr>
                                                        
                                                                    <tr runat="server" id="transfer" visible='<%# IsAdmin() %>' style="width:100%">
                                                                        <td align="left" style="width:100%; background-color:Maroon">
                                                                            <asp:LinkButton runat="server" ID="cmdCopytoXfer" Text="Copy to Transfer Folder" Width="100%"
                                                                                CommandName="Copy" CssClass="popupLink" Font-Underline="false" />
                                                                        </td>
                                                                    </tr>

                                                                    <tr runat="server" id="recommend" visible='<%# IsAdmin() %>' style="width:100%">
                                                                        <td align="left" style="width:100%; background-color:Maroon">
                                                                            <asp:LinkButton runat="server" ID="cmdRecommend" Text="Recommend" Width="100%"
                                                                                CommandName="Recommend" CssClass="popupLink" Font-Underline="false" />
                                                                        </td>
                                                                    </tr>

                                                                    <tr runat="server" id="favorites" visible='<%# IsAdmin() %>' style="width:100%">
                                                                        <td align="left" style="width:100%; background-color:Maroon">
                                                                            <asp:LinkButton runat="server" ID="cmdFavorites" Text="Add to Favorites" Width="100%"
                                                                                CommandName="Favorites" CssClass="popupLink" Font-Underline="false" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>

                                            <tr>
                                                <td align="center" colspan="2" style="width:100%; white-space:nowrap" valign="top">
                                                    <asp:Image runat="server" ID="imgFile" ImageAlign="Middle" 
                                                        ImageUrl='<%#GetImageFile(Eval("song_id"),(float)285,(float)285)%>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>

                                    <ajaxToolkit:HoverMenuExtender ID="hme" runat="server" HoverCssClass="popupHover"
                                        PopupControlID="pnlHoverMenu"
                                        TargetControlID="pnlHoverMenu"
                                        PopupPosition="Left"
                                        PopDelay="150"
                                        HoverDelay="75"
                                        OffsetY="-115" />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblFilePath" Text='<%# Eval("abs_file_path") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField Visible="false">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSongID" Text='<%# Eval("song_id") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Song Title" SortExpression="song_title_sort">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lnkSongTitle" Text='<%# Eval("song_title") %>' Font-Bold="false" Font-Underline="false" ForeColor="Maroon" />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Wrap="false" />
                                <ItemStyle HorizontalAlign="Left" Wrap="true" Font-Bold="false" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderText="Artist"
                                ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Top" ItemStyle-Wrap="true"
                                SortExpression="artist_name_sort" ItemStyle-Font-Bold="false" Visible="false">
                                <ItemTemplate>
                                    <asp:HyperLink ID="lnkArtistName" runat="server" Font-Bold="false"
                                        Text='<%# Eval("artist_name") %>'
                                        NavigateUrl='<%# "~/Search/CustomSearch.aspx?Artist=" + Eval("artist_name")%>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="True" />
                            </asp:TemplateField>


                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" 
                                ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="true"
                                ItemStyle-Font-Bold="false" Visible="true">
                                <ItemTemplate>

<%--
                                    <asp:Label runat="server" id="lblMobileDownload"
                                        Text='<%# "https://zim.homelinux.net/Mobile" + Eval("file_path").ToString().Substring(2) %>' Width="100%" />
--%>

<%--
                                    <img src='<%# "http://chart.apis.google.com/chart?cht=qr&chs=100x100&chl=https://zim.homelinux.net/Mobile" + Eval                                                       ("file_path").ToString().Substring(2) %>' alt="2D QR Link" />
--%>

                                    <img src='<%# "http://chart.apis.google.com/chart?cht=qr&chs=50x50&chl=https://zim.homelinux.net/MobileListen.aspx?SongID=" + Eval("song_id") %>' alt="2D QR Link" />

<%--
                                    <asp:HyperLink ID="lnkArtistName" runat="server" Font-Bold="false"
                                        Text='<%# Eval("artist_name") %>'
                                        NavigateUrl='<%# "~/Search/CustomSearch.aspx?Artist=" + Eval("artist_name")%>' />
--%>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="True" />
                            </asp:TemplateField>
                            
                            <asp:BoundField HeaderText="Duration" DataField="Duration" SortExpression="Duration"
                                HeaderStyle-HorizontalAlign="Left" ItemStyle-ForeColor="Maroon"
                                ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" />

                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderText="Album"
                                ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="true"
                                SortExpression="album_name_sort" ItemStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td align="left" style="width:5%">
                                                <asp:Image runat="server" ID="Image1" ImageAlign="Left" 
                                                    ImageUrl='<%#GetImageFile(Eval("song_id"),(float)50,(float)50)%>' />
                                            </td>
                                            <td align="left" style="width:95%; padding-left:5px">
                                                <asp:Label ID="lblAlbumName" runat="server" Text='<%# Eval("album_name") %>' Width="100%" />
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle ForeColor="Maroon" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="True" />
                            </asp:TemplateField>
                            
                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderText="Genre"
                                ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="true"
                                SortExpression="Genre" ItemStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblGenre" runat="server" Text='<%# Eval("Genre") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle ForeColor="Maroon" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderStyle-HorizontalAlign="Left" HeaderStyle-Wrap="false" HeaderText="Added"
                                ItemStyle-ForeColor="Black" ItemStyle-HorizontalAlign="Left" ItemStyle-VerticalAlign="Middle" ItemStyle-Wrap="true"
                                SortExpression="create_date" ItemStyle-Font-Bold="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateAdded" runat="server" Text='<%# Eval("date_added_short") %>' />
                                </ItemTemplate>
                                <HeaderStyle HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle ForeColor="Maroon" HorizontalAlign="Left" VerticalAlign="Middle" Wrap="false" />
                            </asp:TemplateField>

                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" Font-Bold="false" />
                        <SelectedRowStyle BackColor="Yellow" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle BackColor="#000033" Font-Bold="True" ForeColor="White" />
                        <%--<AlternatingRowStyle BackColor="#DCDCDC" />--%>
                    </ZNet:GridView>
                </asp:Panel>

                <ajaxToolkit:CollapsiblePanelExtender ID="pnlSearchResultsExtender" runat="Server" Collapsed="false" CollapsedSize="0" SuppressPostBack="false"
                    TargetControlID="pnlSearchResults" ExpandControlID="pnlSearchResultsHeader" CollapseControlID="pnlSearchResultsHeader"
                    TextLabelID="lblSearchResultsHeader" ImageControlID="" ExpandedText="(Hide Search Results...)" CollapsedText="(Show Search Results...)" />

                <asp:Panel ID="pnlSongDataHeader" runat="server" CssClass="collapsePanelHeader" Height="20px" BackColor="White" Width="100%">
                    <div style="padding: 5px; cursor: pointer; vertical-align: middle; width:100%">
                        <div style="float: left; margin-left: 20px; width:100%;">
                            <asp:Label ID="lblSongDataHeader" runat="server" Text="(Show Song Details...)" Width="100%" Font-Underline="true" />
                        </div>
                    </div>
                </asp:Panel>

                <asp:Panel runat="server" ID="pnlSongData" CssClass="collapsePanel" Height="2000px" Width="100%" Visible="false">
                    <asp:Literal id="Literal1" runat="server" />
                </asp:Panel>

                <ajaxToolkit:CollapsiblePanelExtender ID="SongDataExtender" runat="Server" TargetControlID="pnlSongData"
                    ExpandControlID="pnlSongDataHeader" CollapseControlID="pnlSongDataHeader" Collapsed="true" CollapsedSize="0"
                    TextLabelID="lblSongDataHeader" ImageControlID="" ExpandedText="(Hide Song Details...)" CollapsedText="(Show Song Details...)"
                    SuppressPostBack="false" />

            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
