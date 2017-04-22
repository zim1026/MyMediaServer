namespace Web
{
    using System;

    public partial class Playlist : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAuthorizedUser)
            {
                Response.Redirect("Logon.aspx");
            }
            
            if (!IsPostBack)
            {

            }
        }
    }
}