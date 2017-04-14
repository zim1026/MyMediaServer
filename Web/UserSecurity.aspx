<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserSecurity.aspx.cs" Inherits="Web.UserSecurity" %>
<%@ MasterType virtualpath="~/Site.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">
    
    <div class="container-fluid">
        <div class="row">
            <div class="col-sm-12">
                <button type="button" oncancel="btn btn-primary" class="btn btn-primary" onclick="JavaScript: UserSecurity.AddNewUser();">Add New User</button>
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                &nbsp;
            </div>
        </div>
    </div>

    <div id="UserListContainer" class="table-responsive" style="overflow-x:hidden; font-size:12px;">
        <table id="UserListTable" class="dataTable table-bordered cell-border compact hover stripe">
            <thead class="dataTableHeader">
                <tr>
                    <th class="hidden">UserSecurityID</th>
                    <th><!--Details Button--></th>
                    <th>Username</th>
                    <th>Active?</th>
                    <th>Admin?</th>
                    <th>Locked?</th>
                    <th>Last Login</th>
                    <th>Prev Login</th>
                    <th>Failed Logins</th>
                    <th>Last Failure</th>
                </tr>
            </thead>
        </table>
    </div>
    
    <div id="UserDialog" class="container-fluid" style="overflow-x:hidden">
        <input type="hidden" id="userSecurityID" />
        <div class="row" style="padding-bottom:15px">
            <div class="col-sm-2 text-right-not-xs">
                <label for="UserName">UserName</label>
            </div>
            <div class="col-sm-10">
                <input type="text" id="UserName" class="form-control" />
            </div>
        </div>

        <div class="row" style="padding-bottom:10px">
            <div class="col-sm-2 text-right-not-xs">
                <label for="Password">Password</label>
            </div>
            <div class="col-sm-10">
                <input type="password" id="Password" class="form-control" />
            </div>
        </div>

        <div class="row">
            <div class="col-sm-2 text-right-not-xs">
                <label for="ActiveFlag">Active?</label>
            </div>
            <div class="col-sm-10">
                <input type="checkbox" id="ActiveFlag" />
            </div>
        </div>

        <div class="row">
            <div class="col-sm-2 text-right-not-xs">
                <label for="AdminFlag">Admin?</label>
            </div>
            <div class="col-sm-10">
                <input type="checkbox" id="AdminFlag" />
            </div>
        </div>

        <div class="row">
            <div class="col-sm-2 text-right-not-xs">
                <label for="LockedFlag">Locked?</label>
            </div>
            <div class="col-sm-10 text-left">
                <input type="checkbox" id="LockedFlag" />
            </div>
        </div>

        <div class="row">
            <div class="col-sm-12 text-center">
                <button type="button" class="btn btn-primary" id="SaveUser" onclick="JavaScript: UserSecurity.SaveUserDetails();">Save</button>
                &nbsp;&nbsp;
                <button type="button" class="btn btn-default" id="CloseUserDialog" onclick="JavaScript: UserSecurity.CloseUserDialog();">Close</button>
            </div>
        </div>

    </div>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script src="UserSecurity.js"></script>
</asp:Content>
