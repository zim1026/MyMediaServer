namespace Web
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using MediaLibrary;

    public partial class Logon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        protected void cmdOK_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtUsername.Text) && !string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                USER_SECURITY user = UserSecurityManager.GetUser(txtUsername.Text);

                if (user != null)
                {
                    bool denyAccess = true;
                    
                    if (user.PASSWORD == txtPassword.Text)
                    {
                        if (user.LAST_LOGIN_DATE.HasValue)
                        {
                            user.PREV_TO_LAST_LOGIN_DATE = user.LAST_LOGIN_DATE;
                        }

                        user.LAST_LOGIN_DATE = DateTime.Now;

                        denyAccess = false;
                    }
                    else
                    {
                        user.LAST_FAILURE_DATE = DateTime.Now;
                        user.LOGIN_FAILURE_COUNT = user.LOGIN_FAILURE_COUNT + 1;
                    }

                    user = UserSecurityManager.Save(user);

                    if (denyAccess)
                    {
                        Response.Redirect("~/AccessDenied.aspx");
                    }
                    else
                    {
                        if (this.Session["user"] != null)
                        {
                            Session.Remove("user");
                        }

                        this.Session.Add("user", user);
                        Response.Redirect("Default.aspx");
                    }
                }
                else
                {
                    Response.Redirect("~/AccessDenied.aspx");
                }
            }
        }
    }
}