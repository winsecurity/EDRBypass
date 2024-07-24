using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace get_pre2000computers
{
    class Program
    {

        public static string username = null;
        public static string password = null;
        static void Main(string[] args)
        {


            // get-pre2000computers  -domain domainname
            // -server serverip
            // -username username -password password

            cliparse cli = new cliparse();



            DirectoryEntry de;

            string domainname = cli.getargvalue(args, "-domain");
            string serverip = cli.getargvalue(args, "-server");
            Program.username = cli.getargvalue(args, "-username");
            Program.password = cli.getargvalue(args, "-password");

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
           
            ds.Filter = "(useraccountcontrol=4128)";
            ds.SearchRoot = de;
            

            foreach(SearchResult sr in ds.FindAll())
            {
                /*var pc = sr.GetDirectoryEntry().Properties.PropertyNames;
                foreach(string pname in pc)
                {
                    Console.WriteLine(pname);
                }*/

                Console.WriteLine("Samaccountname: {0}",sr.Properties["samaccountname"][0]);
                Console.WriteLine("Distinguishedname: {0}",sr.Properties["distinguishedname"][0]);
                Console.WriteLine();

            }
            


        }
    }
}
