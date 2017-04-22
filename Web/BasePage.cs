namespace Web
{
    using MediaLibrary;

    public partial class BasePage : System.Web.UI.Page
    {
        protected bool IsAuthorizedUser
        {
            get
            {
                if (this.Session["user"] != null)
                {
                    return User.ACTIVE_FLAG && !User.LOCKED_FLAG;
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
                    return User.ADMIN_FLAG;
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
                    return User.USERNAME;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
        new protected USER_SECURITY User
        {
            get
            {
                return (USER_SECURITY)this.Session["user"];
            }
            set
            {
                if (this.Session["user"] != null)
                {
                    this.Session["user"] = value;
                }
                else
                {
                    this.Session.Add("user", value);
                }
            }
        }
    }
}