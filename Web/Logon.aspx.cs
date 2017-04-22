namespace Web
{
    using System;
    using System.Security.Cryptography;
    using System.Text;
    using System.Web;
    using System.Web.Security;
    using MediaLibrary;
    
    public partial class Logon : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (IsAuthorizedUser)
                {
                    Response.Redirect("~/Default.aspx");
                }
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

                    if (user.PASSWORD.Equals(UserSecurityManager.HashString(txtPassword.Text.Trim(), UserSecurityManager.HashName.SHA512)) ||
                        user.PASSWORD.Equals(txtPassword.Text.Trim()))
                    {
                        if (user.LAST_LOGIN_DATE.HasValue)
                        {
                            user.PREV_TO_LAST_LOGIN_DATE = user.LAST_LOGIN_DATE;
                        }

                        user.LAST_LOGIN_DATE = DateTime.Now;

                        if (user.LOGIN_FAILURE_COUNT > 0)
                        {
                            user.LOGIN_FAILURE_COUNT = user.LOGIN_FAILURE_COUNT - 1;
                        }
                        denyAccess = false;
                    }
                    else
                    {
                        user.LAST_FAILURE_DATE = DateTime.Now;
                        user.LOGIN_FAILURE_COUNT = user.LOGIN_FAILURE_COUNT + 1;
                        if (user.LOGIN_FAILURE_COUNT > 2)
                        {
                            user.LOCKED_FLAG = true;
                        }
                    }

                    user = UserSecurityManager.Save(user);

                    if (denyAccess)
                    {
                        Response.Redirect("~/AccessDenied.aspx", true);
                    }
                    else
                    {
                        /*
                        if (this.Session["user"] != null)
                        {
                            Session.Remove("user");
                        }
                        this.Session.Add("user", user);
                        */
                        this.User = user;

                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, user.USERNAME, DateTime.Now, DateTime.Now.AddMinutes(30), true, user.USER_SECURITY_ID.ToString());
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                        cookie.Expires = ticket.Expiration;
                        cookie.Path = FormsAuthentication.FormsCookiePath;
                        Response.Cookies.Add(cookie);

                        Response.Redirect(string.IsNullOrWhiteSpace(this.Request["ReturnUrl"]) ? "Default.aspx" : this.Request["ReturnUrl"], true);
                    }
                }
                else
                {
                    Response.Redirect("~/AccessDenied.aspx", true);
                }
            }
        }
    }
}