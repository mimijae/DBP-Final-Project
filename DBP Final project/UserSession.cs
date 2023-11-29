using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBP_Final_project
{
    public static class UserSession
    {
        public static string UserId { get; private set; }
        public static string UserType { get; private set; }

        public static void SetUser(string id, string type)
        {
            UserId = id;
            UserType = type;
        }

        public static void ClearUser()
        {
            UserId = null;
            UserType = null;
        }
    }
}
