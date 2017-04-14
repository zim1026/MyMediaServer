namespace Web
{
    using System;
    using MediaLibrary;
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected bool IsAdmin 
        { 
            get {
                if (Session["user"] != null)
                {
                    return (Session["user"] as USER_SECURITY).ADMIN_FLAG;
                }
                else
                {
                    return false;
                }
            }
        }
        
        protected string Username
        {
            get
            {
                if (Session["user"] != null)
                {
                    return (Session["user"] as USER_SECURITY).USERNAME;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
}