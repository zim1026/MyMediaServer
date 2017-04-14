namespace Web
{
    using System;
    public partial class Default : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!AuthorizedUser)
            {
                Response.Redirect("Logon.aspx");
            }
        }
    }
}