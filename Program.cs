using System;

namespace ldap_with_netcore
{
    class Program
    {
        static void Main (string[] args)
        {
            Program programObj = new Program ();
            int option = 1;

            switch (option)
            {

                case 1:
                    NovellLdapConnectionHelper.UserList ("employeeNumber=100,ou=users,o=Company", "password123", "ou=people,dc=wdgautomation,dc=com");
                    break;
                case 2:
                    // Method #2: Using Microsoft.Windows.Compatibility
                    // By the time the demo is done, it is not available on MacOS.
                    // LdapConectionHelper.IsAuthenticated("admin","secret","ou=users,o=Company");

                case 3:
                    var result = NovellLdapConnectionHelper.VerifyPassword ("employeeNumber=100,ou=users,o=Company", "password123");
                    if (result) Console.WriteLine ("Correct password");
                    else
                        Console.WriteLine ("Incorrect password");
                    break;

                default:
                    break;
            }

            Console.WriteLine ("\n\nPress any key...");
            Console.ReadLine ();
        }

    }
}