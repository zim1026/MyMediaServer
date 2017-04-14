namespace MediaLibrary
{
    using System;

    public partial class USER_SECURITY
    {
        public USER_SECURITY()
        {
            CREATE_DATE = DateTime.Now;
            ACTIVE_FLAG = true;
            ADMIN_FLAG = false;
            LOCKED_FLAG = false;
            LOGIN_FAILURE_COUNT = 0;
        }
    }
}
