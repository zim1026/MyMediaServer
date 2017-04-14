<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Web.Logon" ClientIDMode="Static" %>
<%@ MasterType virtualpath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <div class="container" style="width:800px">
        <div class="row" style="padding-bottom:15px">
            <div class="col-sm-2 col-xs-12 text-right-not-xs">
                <asp:Label runat="server" ID="lblUsername" AssociatedControlID="txtUsername" Text="Username"></asp:Label>
            </div>
            <div class="col-sm-4 col-xs-12 text-left">
                <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control" Text="" />
            </div>
            <div class="col-sm-6 col-xs-12 text-left">
                <asp:RequiredFieldValidator runat="server" ID="rfvUsername" ErrorMessage="Required" SetFocusOnError="true" ControlToValidate="txtUsername" />
            </div>
        </div>

        <div class="row">
            <div class="col-sm-2 col-xs-12 text-right-not-xs">
                <asp:Label runat="server" ID="lblPassword" AssociatedControlID="txtPassword" Text="Password"></asp:Label>
            </div>
            <div class="col-sm-4 col-xs-12 text-left">
                <asp:TextBox runat="server" ID="txtPassword" TextMode="Password" CssClass="form-control" Text="" />
            </div>
            <div class="col-sm-6 col-xs-12 text-left">
                <asp:RequiredFieldValidator runat="server" ID="rfvPassword" ErrorMessage="Required" SetFocusOnError="true" ControlToValidate="txtPassword" />
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                &nbsp;
            </div>
        </div>

        <div class="row">
            <div class="col-sm-2">
                &nbsp;
            </div>
            <div class="col-sm-4 col-xs-12 text-center-not-xs">
                <asp:Button runat="server" ID="cmdOK" Text="OK" CssClass="btn btn-primary" OnClick="cmdOK_Click" CausesValidation="true" />
                &nbsp;&nbsp;
                <button type="reset" class="btn btn-default">Reset</button>
            </div>
            <div class="col-sm-6">
                &nbsp;
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="Logon.js"></script>
</asp:Content>
