namespace MediaLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Itst.Utilities.Web.JQuery;

    public static class UserSecurityManager
    {
        public static USER_SECURITY Save(USER_SECURITY us)
        {
            using (var ctx = NewUserSecurityContext())
            {
                if (us.USER_SECURITY_ID == 0)
                {
                    ctx.USER_SECURITYS.Add(us);
                }
                else
                {
                    USER_SECURITY u = ctx.USER_SECURITYS
                        .Where(q => q.USER_SECURITY_ID == us.USER_SECURITY_ID)
                        .FirstOrDefault();

                    u.ACTIVE_FLAG = us.ACTIVE_FLAG;
                    u.ADMIN_FLAG = us.ADMIN_FLAG;
                    u.LAST_FAILURE_DATE = us.LAST_FAILURE_DATE;
                    u.LAST_LOGIN_DATE = us.LAST_LOGIN_DATE;
                    u.LOCKED_FLAG = us.LOCKED_FLAG;
                    u.LOGIN_FAILURE_COUNT = us.LOGIN_FAILURE_COUNT;
                    u.PASSWORD = us.PASSWORD;
                    u.PREV_TO_LAST_LOGIN_DATE = us.PREV_TO_LAST_LOGIN_DATE;
                    u.UPDATE_DATE = DateTime.Now;
                    u.USERNAME = us.USERNAME;
                }

                ctx.SaveChanges();

                return ctx.USER_SECURITYS
                    .Where(q => q.USER_SECURITY_ID == us.USER_SECURITY_ID)
                    .FirstOrDefault();
            }
        }

        public static USER_SECURITY GetUser(decimal userSecurityID)
        {
            using (var ctx = NewUserSecurityContext())
            {
                return ctx.USER_SECURITYS
                    .Where(q => q.USER_SECURITY_ID == userSecurityID)
                    .FirstOrDefault();
            }
        }

        public static USER_SECURITY GetUser(string username)
        {
            using (var ctx = NewUserSecurityContext())
            {
                return ctx.USER_SECURITYS
                    .Where(q => q.USERNAME == username)
                    .FirstOrDefault();
            }
        }

        public static List<USER_SECURITY> GetUsers(RequestData data)
        {
            using (var ctx = NewUserSecurityContext())
            {
                IQueryable<USER_SECURITY> query = ctx.USER_SECURITYS.AsQueryable();

                query = data.ApplySorting(query);
                data.TotalRecords = query.Count();

                return data.ApplyPaging(query).ToList();
            }
        }

        private static Entities NewUserSecurityContext()
        {
            return new Entities();
        }
    }
}
