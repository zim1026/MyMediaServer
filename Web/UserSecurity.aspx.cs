namespace Web
{
    using System;

    public partial class UserSecurity : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsAuthorizedUser || !IsAdmin)
            {
                Response.Redirect("AccessDenied.aspx");
            }
        }
    }
}