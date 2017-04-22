<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Logon.aspx.cs" Inherits="Web.Logon" ClientIDMode="Static" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>My Media Server - Logon</title>

    <meta name="viewport" content="width=device-width, initial-scale=1" />

    <link type="text/css" rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css" />
    <link type="text/css" rel="stylesheet" href="Content/bootstrap.min.css" />
    <link type="text/css" rel="stylesheet" href="Content/Custom.css" />

    <script type="text/javascript" src="Scripts/jquery-3.1.1.min.js"></script>
    <script type="text/javascript" src="Scripts/bootstrap.min.js"></script>
    <script type="text/javascript" src="Scripts/jquery-ui-1.12.1.min.js"></script>
</head>

<body>
    <form id="form1" runat="server">
        <div class="container">
            <br />
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
    </form>
</body>
</html>
