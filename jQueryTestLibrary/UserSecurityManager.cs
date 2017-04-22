namespace MediaLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using Utilities.Web.JQuery;

    public static class UserSecurityManager
    {
        public enum HashName
        {
            SHA,
            SHA1,
            MD5,
            SHA256,
            SHA384,
            SHA512
        }

        public static string HashString(string value, HashName hashName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("value");
            
            HashAlgorithm algorithm = HashAlgorithm.Create(hashName.ToString());
            if (algorithm == null)
            {
                throw new ArgumentException("Unrecognized hash name:", hashName.ToString());
            }
            byte[] hash = algorithm.ComputeHash(Encoding.UTF8.GetBytes(value));

            return Convert.ToBase64String(hash);
        }
        
        public static USER_SECURITY Save(USER_SECURITY us)
        {
            using (var ctx = NewUserSecurityContext())
            {
                if (us.USER_SECURITY_ID == 0)
                {
                    us.PASSWORD = HashString(us.PASSWORD, HashName.SHA512);
                    ctx.USER_SECURITYS.Add(us);
                }
                else
                {
                    USER_SECURITY u = ctx.USER_SECURITYS
                        .Where(q => q.USER_SECURITY_ID == us.USER_SECURITY_ID)
                        .FirstOrDefault();

                    if (!u.PASSWORD.Equals(us.PASSWORD))
                    {
                        u.PASSWORD = HashString(us.PASSWORD, HashName.SHA512);
                    }
                    
                    u.ACTIVE_FLAG = us.ACTIVE_FLAG;
                    u.ADMIN_FLAG = us.ADMIN_FLAG;
                    u.LAST_FAILURE_DATE = us.LAST_FAILURE_DATE;
                    u.LAST_LOGIN_DATE = us.LAST_LOGIN_DATE;
                    u.LOCKED_FLAG = us.LOCKED_FLAG;
                    u.LOGIN_FAILURE_COUNT = us.LOGIN_FAILURE_COUNT;
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
