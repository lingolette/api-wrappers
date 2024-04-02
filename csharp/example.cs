using System;
using lingolette;

namespace example
{
    class MainClass
    {
        public static void Main (string[] args)
        {
            var API = new CSharpAPI ("YOUR_ORG_ID", "YOUR_ORG_SECRET");

            string result = API.Exec ("org", "createUserSession", "{\"userId\":\"YOUR_ORG_USER_ID\"}");
            Console.WriteLine (result);
        }
    }
}
