using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Upload:EnhancedPage {
    protected void Page_Load(object sender, EventArgs e) {
        if(!IsPostBack) {
            try {
                if(!Catalog.Users.ValidUser(Session["username"], Session["admin"]))
                    Response.Redirect("~/Login.aspx");
            } catch(Exception ex) {
                Response.Redirect("~/Login.aspx");
            }
        }

        SetPageHeading("Upload Songs");
        SelectTreeNode();
    }

    protected void cmdUpload_Click(object sender, EventArgs e) {
        try {
            foreach(HttpPostedFile file in FileUploader.PostedFiles) {
                try {
                    if(file.FileName.Contains("+"))
                        file.SaveAs("F:\\lib\\Uploads\\" + System.IO.Path.GetFileName(file.FileName).Replace("+", "-"));
                    else
                        file.SaveAs("F:\\lib\\Uploads\\" + System.IO.Path.GetFileName(file.FileName));
                } catch(Exception ex1) {
                    ex1.Source = "File: " + file.FileName + " ---> " +ex1.Source;
                    throw;
                }
            }
        } catch(Exception ex) {
            ex.Source="PostedFiles()--->"+ex.Source;
            Response.Write(ex.Source + " threw: " + ex.Source);
        }
    }
}
