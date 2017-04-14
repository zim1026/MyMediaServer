namespace Web
{
    using MediaLibrary;

    public partial class BasePage : System.Web.UI.Page
    {
        protected bool AuthorizedUser
        {
            get
            {
                if (this.Session["user"] != null)
                {
                    return (Session["user"] as USER_SECURITY).ACTIVE_FLAG && !(Session["user"] as USER_SECURITY).LOCKED_FLAG;
                }
                else
                {
                    return false;
                }
            }
        }
        protected bool IsAdmin {
            get
            {
                if (this.Session["user"] != null)
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