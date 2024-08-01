using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace add_computer
{
    class Program
    {
        public static string username = null;
        public static string password = null;



        static void Main(string[] args)
        {
            // add-computer -computername [name] -computerpassword [pass] -dn [base]
            // -domain domainname
            // -server serverip
            // -username username -password password

            cliparse cli = new cliparse();



            DirectoryEntry de;

            string domainname = cli.getargvalue(args, "-domain");
            string serverip = cli.getargvalue(args, "-server");
            Program.username = cli.getargvalue(args, "-username");
            Program.password = cli.getargvalue(args, "-password");



            string computername = cli.getargvalue(args, "-computername");
            string computerpassword = cli.getargvalue(args, "-computerpassword");
            string dntoadd = cli.getargvalue(args, "-dn");

            string domaindn = "";

            if (computername == null||computerpassword==null)
            {
                Console.WriteLine("-computername [name] -computerpassword [pass] are mandatory");
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

                if (dntoadd == null)
                {
                    de = new DirectoryEntry(String.Format("LDAP://CN=Computers,{0}", String.Join(",", dcs)));
                   
                }
                else
                {
                    de = new DirectoryEntry(String.Format("LDAP://{0}", dntoadd));
                }

            }


            else if (serverip != null)
            {
                if (dntoadd == null)
                {
                    de = new DirectoryEntry(String.Format("LDAP://{0}", serverip));
                     domaindn = de.Properties["distinguishedname"][0].ToString();
                    de = new DirectoryEntry(String.Format("LDAP://{0}/CN=Computers,{1}",
                        serverip, domaindn));
                    
                }
                else
                {
                    de = new DirectoryEntry(String.Format("LDAP://{0}/{1}", serverip,dntoadd));

                }
            }

            else
            {
                string[] dcs = domainname.Split('.');

                for (int i = 0; i < dcs.Length; i++)
                {
                    dcs[i] = "DC=" + dcs[i];
                    //Console.WriteLine(dcs[i]);
                }
                if (dntoadd == null)
                {
                    de = new DirectoryEntry(String.Format("LDAP://CN=Computers,{0}", String.Join(",", dcs)));

                }
                else
                {
                    de = new DirectoryEntry(String.Format("LDAP://{0}", dntoadd));
                }
            }

            if (Program.username != null && Program.password != null)
            {
                de.Username = Program.username;
                de.Password = Program.password;
            }

            DirectorySearcher ds = new DirectorySearcher(de);

            DirectoryEntry ou = ds.FindOne().GetDirectoryEntry();
            Console.WriteLine(ou.Path);
            DirectoryEntry comp1 =  ou.Children.Add("CN="+computername, "Computer");

            //comp1.AuthenticationType = AuthenticationTypes.Secure;
            comp1.Properties["samaccountname"].Value = computername.ToUpper()+"$";
            //comp1.Properties["primarygroupid"].Value = 515;
            comp1.Properties["useraccountcontrol"].Value = 0x1000;

            comp1.CommitChanges();
            comp1.Password = computerpassword;

            comp1.CommitChanges();

            if (dntoadd == null)
            {
                Console.WriteLine("Successfully added new computer to CN=Computers");
            }
            else
            {
                Console.WriteLine("Successfully added new computer to {0}",dntoadd);
            }
        }
    }
}
