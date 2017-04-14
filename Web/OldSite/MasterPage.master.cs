using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : EnhancedMaster{

    protected void Page_Load(object sender, EventArgs e) {
        /*
        try {
            if(Session["username"] == null || Session["admin"] == null ||
               !Catalog.Users.ValidUser(Session["username"], Session["admin"]))
                Response.Redirect("~/Login.aspx");

            if(Session["admin"].ToString() != "true")
                DisableAdminFeatures();
        } catch(Exception ex) {
            Response.Redirect("~/Login.aspx");
        }
        */
    }
    
    protected void DisableAdminFeatures() {
        foreach(TreeNode node in menuTree.Nodes) {
            if(node.Text == "Administration") {
                DisableNode(node);
            }
        }
    }

    private void DisableNode(TreeNode node) {
        node.Expanded=false;
        node.SelectAction = TreeNodeSelectAction.None;
        node.ToolTip = "You do not have access to administrative features.";
        foreach(TreeNode child in node.ChildNodes) {
            DisableNode(child);
        }
    }

    protected void lnkShowHide_Click(object sender, EventArgs e) {
        if(pnlNavMenu.Visible) {
            pnlNavMenu.Visible = false;
            lnkShowHide.Text = "Show Navigation Menu";
        }
        else {
            pnlNavMenu.Visible = true;
            lnkShowHide.Text = "Hide Navigation Menu";
        }
    }
}
