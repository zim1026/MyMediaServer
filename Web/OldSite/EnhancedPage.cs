using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;

public class EnhancedPage : System.Web.UI.Page
{
    private GridView _gv;
    private Label _lblRecCount;
    private DropDownList _ddlPageSize;
    private string _procName;
    private string _subjectMatter;

    protected enum SelectionAction {
        SelectAll,
        UnSelectAll,
        ZipAndDownload,
        Transfer,
        Recommend,
        AddToFavorites,
        ReTag,
        Delete
    }

    /*
    protected List<OracleParameter> Params {
        get { return _params; }
        set { _params = value; }
    }
    */

    protected Int32 GetUploadFileCounts() {
        Int32 ReturnValue = 0;

        System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(Server.MapPath("~/media/Uploads"));
        ReturnValue = dirInfo.GetFiles("*.mp3").Length;

        return ReturnValue;
    }

    protected void CheckForAuthorizedUser(string user, bool redirect11) {
        /*
        if(Session["admin"] != null) {
            if(Session["admin"].ToString() != "true") {
                Response.Redirect("~/Default.aspx");
            }
            else {
                if(!Catalog.Users.IsAdmin(user))
                    Response.Redirect("~/AccessDenied.aspx");
            }
        }
        else {
            Response.Redirect("~/Default.aspx");
        }
        */
    }

    protected void CheckForAuthorizedUser(string user) {
        CheckForAuthorizedUser(user, true);
    }

    protected int BoolToInt(bool value) {
        if(value)
            return 1;
        else
            return 0;
    }

    protected bool IntToBool(int value) {
        if(value==1)
            return true;
        else
            return false;
    }

    protected void SelectDropDownListItem(string value, DropDownList ddl) {
        foreach(ListItem item in ddl.Items) {
            item.Selected = false;
            if(Convert.ToString(item.Value) == value)
                item.Selected = true;
        }
    }

    protected void SelectDropDownListItem(int value, DropDownList ddl) {
        foreach(ListItem item in ddl.Items) {
            item.Selected = false;
            if(Convert.ToInt32(item.Value) == value)
                item.Selected = true;
        }
    }

    protected void SetPageHeading(string heading) {
        Session["PrevPage"] = Session["CurrentPage"];
        Session["CurrentPage"] = Request.Url.ToString();

        Label lblCurrentPage = (Label)Master.FindControl("lblCurrentPage");
        if(lblCurrentPage != null)
            lblCurrentPage.Text = heading;
    }

    public string GetImageFile(object objImage, object Height, object Width) {
        return Common.GetImageFile(objImage, Server.MapPath("~/media/temp_art/"), (float)Height, (float)Width);
    }
    
    public string GetImageFile(object objImage) {
        return GetImageFile(objImage, true);
    }

    public string GetImageFile(object objImage, bool resize) {
        return Common.GetImageFile(objImage, Server.MapPath("~/media/temp_art/"), resize);
    }

    protected Unit Height {
        get { return Common.Height; }
        set { Common.Height = value; }
    }

    protected Unit Width {
        get { return Common.Width; }
        set { Common.Width = value; }
    }


    protected void SelectTreeNode() {
        Label lblCurrentPage = (Label)Master.FindControl("lblCurrentPage");
        if(lblCurrentPage != null)
            SelectTreeNode(lblCurrentPage.Text);
    }

    protected void SelectTreeNode(string nodeText) {
        TreeView tree = (TreeView)Master.FindControl("menuTree");
        if(tree != null)
            foreach(TreeNode node in tree.Nodes) {
                SelectNode(node, nodeText);
            }
    }

    private void SelectNode(TreeNode node, string nodeText) {
        if(node.Text == nodeText)
            node.Selected = true;
        else if(node.ChildNodes.Count > 0)
            foreach(TreeNode tn in node.ChildNodes) {
                SelectNode(tn, nodeText);
            }
    }

    protected string GetGridViewTemplateItemValue(GridView gv, string ControlName) {
        using(Label lblDesc = (Label)gv.SelectedRow.FindControl(ControlName)) {
            return lblDesc.Text;
        }
    }

    protected Boolean GetGridViewCheckBoxValue(GridView gv, string ControlName) {
        using(CheckBox chkBox = (CheckBox)gv.SelectedRow.FindControl(ControlName)) {
            return chkBox.Checked;
        }
    }

    protected Int32 GetPageSize() {
        return Convert.ToInt32(DDLPageSize.SelectedValue);
    }

    protected string GetPageName() {
        return System.IO.Path.GetFileName(Request.PhysicalPath);
    }

    protected void HandleException(Exception ex) {
        Response.Write(ex.Source + " threw: <br />" + ex.Message);
    }

    protected void HandleApplicationError(ApplicationException aex, Panel ErrorPanel, Label lblErrorMessage) {
        ErrorPanel.Visible = true;
        lblErrorMessage.Text = aex.Message;
    }

    protected void HandleError(Exception ex, Panel ErrorPanel, Label lblErrorMessage) {
        ErrorPanel.Visible = true;
        ex.Source = System.IO.Path.GetFileName(Page.Request.PhysicalPath) + "--->" + ex.Source;

        lblErrorMessage.Text = ex.Source + " generated the following error:<br /><br /> " + ex.Message;
    }

    protected string SubjectMatter {
        get { return _subjectMatter; }
        set { _subjectMatter = value; }
    }

    protected string ProcName {
        get { return _procName; }
        set { _procName = value; }
    }

    protected GridView GV {
        get { return _gv; }
        set { _gv = value; }
    }

    protected Label RecordCountLabel {
        get { return _lblRecCount; }
        set { _lblRecCount = value; }
    }

    protected DropDownList DDLPageSize {
        get { return _ddlPageSize; }
        set { _ddlPageSize = value; }
    }
}
