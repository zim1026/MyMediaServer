using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;

public class EnhancedPage:Catalog.Cache {
    #region Privates
    private GridView _gv;
    private Label _lblRecCount;
    private DropDownList _ddlPageSize;
    private string _procName;
    private string _subjectMatter;
    private List<OracleParameter> _params = new List<OracleParameter>();
    private Common _common = new Common();
    #endregion

    protected List<OracleParameter> Params {
        get { return _params; }
        set { _params = value; }
    }

    public string GetConnectionString {
        get { return _common.GetConnectionString; }
    }

    protected Int32 GetUploadFileCounts() {
        Int32 ReturnValue = 0;

        System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(Server.MapPath("~/media/Uploads"));
        ReturnValue = dirInfo.GetFiles("*.mp3").Length;

        return ReturnValue;
    }

    protected void CheckForAuthorizedUser(string user, bool redirect11) {
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

    #region Image Methods
    public string GetImageFile(object objImage) {
        return _common.GetImageFile(objImage, Server.MapPath("~/media/temp_art/"));
    }

    protected Unit Height {
        get { return _common.Height; }
        set { _common.Height = value; }
    }

    protected Unit Width {
        get { return _common.Width; }
        set { _common.Width = value; }
    }

    #endregion Image Methods

    #region MenuTree Methods
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
    #endregion MenuTree Methods

    #region GridView Methods
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

    protected DataView GetDataView() {
        try {
            return new DataView(GetDataTable());
        } catch(Exception ex) {
            ex.Source = GetPageName() + ".GetDataView()--->"+ex.Source;
            throw;
        }
    }

    protected DataTable GetDataTable() {
        try {
            DataSet oDS = null;
            using(DBUtil.ODP dbUtil = new DBUtil.ODP(GetConnectionString,true)) {
                LoadCache(dbUtil);
                using(OracleCommand oCmd = new OracleCommand()) {
                    oCmd.CommandType = CommandType.StoredProcedure;
                    oCmd.CommandText = ProcName;
                    dbUtil.GetStoredProcParameters(oCmd);
                    oDS = dbUtil.ExecuteQueryAsDataSet(oCmd);

                    RecordCountLabel.Text = oDS.Tables[0].Rows.Count + " " + SubjectMatter + " found";
                    FetchCache(dbUtil);
                }
            }

            if(oDS != null && oDS.Tables.Count > 0) {
                oDS.Tables[0].Columns.Add(new DataColumn("RecNum", typeof(Int32)));

                for(Int32 i = 0;i<oDS.Tables[0].Rows.Count;i++) {
                    oDS.Tables[0].Rows[i]["RecNum"] = i + 1;
                }

                return oDS.Tables[0];
            }
            else {
                return new DataTable();
            }

        } catch(Exception ex) {
            ex.Source = System.IO.Path.GetFileName(Request.PhysicalPath) + ".GetDataTable(" + ProcName + ")--->" + ex.Source;
            throw;
        }
    }

    protected void Search(bool Bind) {
        try {
            if(Cache["data"] == null)
                Cache["data"] = GetDataView();

            if(Bind) {
                GV.DataSource = Cache["data"];
                GV.DataBind();
            }
        } catch(Exception ex) {
            ex.Source = GetPageName() + ".Search("+ Convert.ToString(Bind) + ")--->"+ex.Source;
            throw;
        }
    }

    #endregion GridView Methods

    #region Event Handlers
    protected void gvData_Sorting(object sender, GridViewSortEventArgs e) {
        try {
            if(Cache["data"] == null)
                Search(false);

            using(GridViewEnhancement.Sorter sorter = new GridViewEnhancement.Sorter((GridView)sender, e, (String)ViewState["sorted"])) {
                ViewState["sorted"] = sorter.Sort((DataView)Cache["data"]);
            }
        } catch(Exception ex) {
            ex.Source = "gvData_Sorting()--->"+ex.Source;
            throw;
        }
    }

    protected void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        try {
            if(Cache["data"] == null)
                Search(false);

            using(GridViewEnhancement.Pager pager = new GridViewEnhancement.Pager((GridView)sender, e)) {
                pager.ChangePage((DataView)Cache["data"]);
            }
        } catch(Exception ex) {
            ex.Source = "gvData_PageIndexChanging()--->"+ex.Source;
            throw;
        }
    }

    protected void ddlPageSize_SelectedIndexChanged(object sender, EventArgs e) {
        GV.PageSize = GetPageSize();
        Search(true);
    }

    #endregion Event Handlers

    #region Exception/Error Handling
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
    #endregion Exception/Error Handling

    #region Virtual Page Elements
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
    #endregion Virtual Page Elements
}
