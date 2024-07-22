using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;


namespace get_groupmembersrecurse
{
    class Program
    {

        public static List<string> mymembers = new List<string>();

        public static void GetAllMembers(string groupName)
        {
            

            PrincipalContext p = new PrincipalContext(ContextType.Domain);
            GroupPrincipal gp = GroupPrincipal.FindByIdentity(p, groupName);
            foreach (Principal group in gp.GetMembers())
            {
                if (group.StructuralObjectClass == "user")
                {
                    Console.WriteLine("User: {0} is memberOf {1}", group.Name, groupName);
                }
            }
            foreach(Principal group in gp.GetMembers())
            {
                if (group.StructuralObjectClass == "group")
                {
                    foreach (string g in mymembers)
                    {
                        if (g == group.Name)
                        {
                            return;
                        }

                    }

                    Console.WriteLine("Group: {0} is memberOf {1}", group.Name, groupName);


                    mymembers.Add(group.Name);
                    GetAllMembers(group.Name);
                }
            }
                
            
        }
        public static string username1 = "";
        public static string password1 = "";

        static void Main(string[] args)
        {
           
            // getgroupmembers -groupname groupname -domain domainname
            // -server serverip -username username -password password

            cliparse cli = new cliparse();

            string groupname = cli.getargvalue(args, "-groupname");
            if (groupname == null)
            {
                Console.WriteLine("Please supply -groupname [groupname]");
                System.Environment.Exit(0);
            }



            DirectoryEntry de;

            string domainname = cli.getargvalue(args, "-domain");
            string serverip = cli.getargvalue(args, "-server");
            Program.username1 = cli.getargvalue(args, "-username");
            Program.password1 = cli.getargvalue(args, "-password");

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
            if (Program.username1 != null && Program.password1 != null)
            {
                de.Username = Program.username1;
                de.Password = Program.password1;
            }


            DirectorySearcher ds = new DirectorySearcher(de);
            GetAllMembers(groupname);


        }
    }
}
