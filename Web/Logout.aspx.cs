namespace Web
{
    using System;
    using System.Web.Security;

    public partial class Logout : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            this.Session.Abandon();
        }
    }
}