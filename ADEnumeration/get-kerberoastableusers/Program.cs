using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;

namespace get_kerberoastableusers
{
    class Program
    {

        public static string username = null;
        public static string password = null;

        static void Main(string[] args)
        {

            // get-kerberoastableusers  -domain domainname
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
            // "( serviceprincipalname = * )" ldap query might get detected

            ds.Filter = "(objectclass=user)";
            ds.SearchRoot = de;

            foreach(SearchResult sr in ds.FindAll())
            {
                try
                {
                    
                    if (sr.Properties["serviceprincipalname"].Count < 1)
                    {
                        continue;
                    }

                    string name = sr.Properties["samaccountname"][0].ToString();

                    Console.WriteLine("Samaccountname: {0}", name);

                    foreach (string spn in sr.Properties["serviceprincipalname"])
                    {
                        
                        
                        Console.WriteLine("spn: {0}",spn);
                    }

                    Console.WriteLine();
                }
                catch(Exception e) { }
            }


        }
    }
}
