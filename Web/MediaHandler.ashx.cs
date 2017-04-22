namespace Web
{
    using System.Web;
    using System.Web.SessionState;
    using MediaLibrary;

    public class MediaHandler : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context.Session["user"] != null)
            {
                if ((context.Session["user"] as USER_SECURITY).ACTIVE_FLAG && !(context.Session["user"] as USER_SECURITY).LOCKED_FLAG)
                {
                    if (context.Request.QueryString["SONG_ID"] != null)
                    {
                        SONG song = SongManager.GetSong(decimal.Parse(context.Request.QueryString["SONG_ID"]));

                        if (song != null)
                        {
                            context.Response.ContentType = "audio/mpeg";
                            context.Response.AppendHeader("Content-Disposition", "  attachment;  filename  = " + "\"" + song.FILENAME + "\"");
                            context.Response.TransmitFile(song.ABS_FILE_PATH.StartsWith("~") ? song.ABS_FILE_PATH : "~/" + song.ABS_FILE_PATH);
                        }
                        //non-existent song
                        else
                        {
                            context.Response.Redirect("Default.aspx");
                        }
                    }
                    //missing or invalid query-string argument
                    else
                    {
                        context.Response.Redirect("Default.aspx");
                    }
                }
                //unauthorized user
                else
                {
                    context.Response.Redirect("AccessDenied.aspx");
                }
            }
            //null session
            else
            {
                context.Response.Redirect("Logon.aspx");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}