namespace Web
{
    using System;

    public partial class Logout : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Session.Clear();
            this.Session.Abandon();
        }
    }
}