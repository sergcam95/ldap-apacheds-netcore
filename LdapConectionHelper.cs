using System;
using System.DirectoryServices.Protocols;
using System.Net;

namespace ldap_with_netcore
{
    public static class LdapConectionHelper
    {

        static string connectionString = "ldap://localhost:10389";

        public static bool IsAuthenticated (string username, string password, string domain)
        {
            bool authenticated = false;
            //pass the connectionString here
            using (LdapConnection connection = new LdapConnection (connectionString))
            {
                try
                {
                    username = username + domain;
                    connection.AuthType = AuthType.Basic;
                    connection.SessionOptions.ProtocolVersion = 3;
                    var credential = new NetworkCredential (username, password);
                    connection.Bind (credential);
                    authenticated = true;
                    return authenticated;
                }
                catch (LdapException e)
                {
                    Console.WriteLine("Error: " + e.Message);
                    return authenticated;
                }
                finally
                {
                    connection.Dispose ();
                }
            }
        }

    }
}