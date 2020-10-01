using System;
using System.Collections;
using System.IO;
using Novell.Directory.Ldap;

namespace ldap_with_netcore
{
    public static class NovellLdapConnectionHelper
    {
        static string ldapHost = "localhost";
        static int ldapPort = 10389;
        static int ldapVersion = LdapConnection.LdapV3;
        static string loginDN = "uid=admin, ou=system";
        static string adminPassword = "secret";
        static string searchBase = "ou=users,o=Company";
        static string searchFilter = "objectClass=inetOrgPerson";

        public static void UserList (string objectDN, string password)
        {
            LdapConnection conn = new LdapConnection ();

            try
            {
                Console.WriteLine ("Connecting to " + ldapHost);
                // Connect to the LDAP server using the host and the port
                // ldap//<host>:<port>
                conn.Connect (ldapHost, ldapPort);
                conn.Bind (objectDN, password);

                string[] requiredAttributes = { "cn", "sn", "uid", "userPassword" };

                ILdapSearchResults lsc = conn.Search (searchBase,
                    LdapConnection.ScopeSub,
                    searchFilter,
                    requiredAttributes,
                    false);

                while (lsc.HasMore ())
                {
                    LdapEntry nextEntry = null;

                    try
                    {
                        nextEntry = lsc.Next ();
                    }
                    catch (LdapException e)
                    {
                        Console.WriteLine ("Error : " + e.LdapErrorMessage);
                        continue;
                    }

                    Console.WriteLine ("\n" + nextEntry.Dn);
                    LdapAttributeSet attributeSet = nextEntry.GetAttributeSet ();
                    IEnumerator ienum = attributeSet.GetEnumerator ();

                    while (ienum.MoveNext ())
                    {
                        LdapAttribute attribute = (LdapAttribute) ienum.Current;
                        string attributeName = attribute.Name;
                        string attributeVal = attribute.StringValue;
                        Console.WriteLine ("\t" + attributeName + "\tvalue = \t" + attributeVal);
                    }
                }

                conn.Disconnect ();
            }
            catch (LdapException e)
            {
                Console.WriteLine ("Error: " + e.LdapErrorMessage);
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine ("Error: " + e.Message);
                return;
            }
            finally
            {
                conn.Disconnect ();
            }

        }

        public static bool VerifyPassword (string objectDN, string password)
        {
            LdapConnection conn = new LdapConnection ();

            try
            {
                // connect to the server
                conn.Connect (ldapHost, ldapPort);

                // Try to authenticate
                conn.Bind (ldapVersion, objectDN, password);

                // disconnect with the server
                conn.Disconnect ();

                return true;
            }
            catch (LdapException e)
            {
                if (e.ResultCode == LdapException.ConnectError)
                {
                    Console.Error.WriteLine ("Error: Server down");
                }
                else if (e.ResultCode == LdapException.NoSuchObject)
                {
                    Console.Error.WriteLine ("Error: No such entry");
                }
                else if (e.ResultCode == LdapException.NoSuchAttribute)
                {
                    Console.Error.WriteLine ("Error: No such attribute");
                }
                else
                {
                    Console.Error.WriteLine ("Error: " + e.ToString () + "\nType: " + e.ResultCode);
                }
            }
            catch (IOException e)
            {
                Console.Error.WriteLine ("Error: " + e.ToString ());
            }
            catch (Exception e)
            {
                Console.Error.WriteLine ("Error: " + e.ToString ());
            }
            finally
            {
                // disconnect with the server
                conn.Disconnect ();
            }

            return false;

        }

    }

}