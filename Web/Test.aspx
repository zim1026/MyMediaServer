<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Web.Test" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeaderContent" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="Content" runat="server">

 <a data-toggle="modal" href="#myModal" class="btn btn-primary">Launch modal</a>

<div class="modal fade" id="myModal">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 class="modal-title">Modal 1</h4>
            </div>

            <div class="container"></div>

            <div class="modal-body">Content for the dialog / modal goes here.
                <br />
                <br />
                <br />
                <p>more content</p>
                <br />
                <br />
                <br />
                <a data-toggle="modal" href="#myModal2" class="btn btn-primary">Launch modal</a>
            </div>

            <div class="modal-footer">
                <a href="#" data-dismiss="modal" class="btn">Close</a>
                <a href="#" class="btn btn-primary">Save changes</a>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="myModal2">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 class="modal-title">Modal 2</h4>
            </div>
            
            <div class="container"></div>
            
            <div class="modal-body">Content for the dialog / modal goes here.
                <br />
                <br />
                <p>come content</p>
                <br />
                <br />
                <br />
                <a data-toggle="modal" href="#myModal3" class="btn btn-primary">Launch modal</a>
            </div>
            
            <div class="modal-footer">
                <a href="#" data-dismiss="modal" class="btn">Close</a>
                <a href="#" class="btn btn-primary">Save changes</a>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="myModal3">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 class="modal-title">Modal 3</h4>
            </div>

            <div class="container"></div>

            <div class="modal-body">Content for the dialog / modal goes here.
                <br />
                <br />
                <br />
                <br />
                <br />
                <a data-toggle="modal" href="#myModal4" class="btn btn-primary">Launch modal</a>
            </div>

            <div class="modal-footer">
                <a href="#" data-dismiss="modal" class="btn">Close</a>
                <a href="#" class="btn btn-primary">Save changes</a>
            </div>
        </div>
    </div>
</div>

<div class="modal fade" id="myModal4">
    <div class="modal-dialog">
        <div class="modal-content">
            <div class="modal-header">
                <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                <h4 class="modal-title">Modal 4</h4>
            </div>

            <div class="container"></div>

            <div class="modal-body">
                Content for the dialog / modal goes here.
            </div>
            
            <div class="modal-footer">
                <a href="#" data-dismiss="modal" class="btn">Close</a>
                <a href="#" class="btn btn-primary">Save changes</a>
            </div>
        </div>
    </div>
</div>


    <%--
    <a data-toggle="modal" href="#myModal" class="btn btn-primary" style="visibility:hidden" id="fuckerLauncher">Launch modal</a>

    <div class="modal" id="myModal">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Modal title</h4>
                </div>
                <div class="container"></div>
                <div class="modal-body">
                    Content for the dialog / modal goes here.
                    <br>
                    <br>
                    <br>
                    <br>
                    <br>
                    <a data-toggle="modal" href="#myModal2" class="btn btn-primary">Launch modal</a>
                </div>
                <div class="modal-footer">
                    <a href="#" data-dismiss="modal" class="btn">Close</a>
                    <a href="#" class="btn btn-primary">Save changes</a>
                </div>
            </div>
        </div>
    </div>

    <div class="modal" id="myModal2" data-backdrop="static">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                    <h4 class="modal-title">Second Modal title</h4>
                </div>
                <div class="container"></div>
                <div class="modal-body">
                    Content for the dialog / modal goes here.
                </div>
                <div class="modal-footer">
                    <a href="#" data-dismiss="modal" class="btn">Close</a>
                    <a href="#" class="btn btn-primary">Save changes</a>
                </div>
            </div>
        </div>
    </div>
    --%>

    <%--
    <a href="#firstModal" class="btn btn-default" data-toggle="modal">Show Modal</a>

    <div id="firstModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h5>FIRST</h5>
                </div>
                <div class="modal-body">

                    <div id="secondModal" class="modal fade">
                        <div class="modal-dialog">
                            <div class="modal-content">
                                <div class="modal-header">
                                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                                        &times;
                                    </button>
                                    <h5>SECOND</h5>
                                </div>
                                <div class="modal-body">
                                    <p>Content</p>
                                </div>
                            </div>
                        </div>
                    </div>


                    <p>Content</p>
                    <br />
                    <a href="#secondModal" class="btn btn-default" data-toggle="modal" id="fucker">Show Modal</a>
                    <br />
                    <button type="button" class="btn btn-default" data-toggle="modal" onclick="JavaScript:Test.OpenSubModal();">Open Sub</button>
                    <br />
                    <button type="button" class="btn btn-default" data-toggle="modal" onclick="JavaScript:Test.ToggleSubModal();">Toggle Sub</button>
                    <br />
                    <button type="button" class="btn btn-default" data-toggle="modal" onclick="JavaScript:Test.CloseSubModal();">Close Sub</button>
                </div>
            </div>
        </div>
    </div>
    --%>

    <%--
    <div id="thirdModal" class="modal fade">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">
                        &times;
                    </button>
                    <h5>THIRD</h5>
                </div>
                <div class="modal-body">
                    <p>Content</p>
                </div>
            </div>
        </div>
    </div>
    --%>
</asp:Content>

<asp:Content ID="Content3" ContentPlaceHolderID="ScriptContent" runat="server">
    <script type="text/javascript" src="Test.js"></script>
</asp:Content>
