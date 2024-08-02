using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace change_accountpassword
{
    class Program
    {
        public static string username = null;
        public static string password = null;



        static void Main(string[] args)
        {
            // change_accountpassword -samaccountname [name] -passwordtoset [password]
            // -domain domainname
            // -server serverip
            // -username username -password password

            cliparse cli = new cliparse();



            DirectoryEntry de;

            string domainname = cli.getargvalue(args, "-domain");
            string serverip = cli.getargvalue(args, "-server");
            Program.username = cli.getargvalue(args, "-username");
            Program.password = cli.getargvalue(args, "-password");

            string samaccountname = cli.getargvalue(args, "-samaccountname");
            string passwordtoset = cli.getargvalue(args, "-passwordtoset");

            if (samaccountname == null || passwordtoset ==null)
            {
                Console.WriteLine("-samaccountname [name] -passwordtoset [password] are mandatory");
                System.Environment.Exit(0);
            }

            if (domainname == null && serverip == null)
            {
                string domain = Domain.GetCurrentDomain().ToString();
                string[] dcs = domain.Split('.');

                for (int i = 0; i < dcs.Length; i++)
                {
                    dcs[i] = "DC=" + dcs[i];
                    //Console.WriteLine(dcs[i]);
                }
                de = new DirectoryEntry(String.Format("LDAP://{0}", String.Join(",", dcs)));

            }


            else if (serverip != null)
            {
                de = new DirectoryEntry(String.Format("LDAP://{0}", serverip));
            }

            else
            {
                string[] dcs = domainname.Split('.');

                for (int i = 0; i < dcs.Length; i++)
                {
                    dcs[i] = "DC=" + dcs[i];
                    //Console.WriteLine(dcs[i]);
                }
                de = new DirectoryEntry(String.Format("LDAP://{0}", String.Join(",", dcs)));

            }

            if (Program.username != null && Program.password != null)
            {
                de.Username = Program.username;
                de.Password = Program.password;
            }

            DirectorySearcher ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectclass=user)(samaccountname=" + samaccountname+ "))";

            

            DirectoryEntry obj = ds.FindOne().GetDirectoryEntry();
            obj.Invoke("SetPassword", new object[] { passwordtoset });
            obj.Properties["LockOutTime"].Value = 0;
             obj.CommitChanges();

            

        }
    }
}
