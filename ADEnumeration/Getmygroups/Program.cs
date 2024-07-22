using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;


namespace getmygroups
{
    class Program
    {

        public static string username1 = "";
        public static string password1 = "";
        public static List<string> mygroups = new List<string>();
        static void checkgroupmembership(PrincipalContext pc,Principal user,List<string> groups)
        {
            

            foreach(string groupname in groups)
            {
                GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, groupname);
                
                if (user.IsMemberOf(group))
                {
                    Console.WriteLine("[+] {0}",group.Name);
                    checkgroupmembership(pc,(Principal)group , groups);
                }
            }

        }

        static void nestedgroups(string groupdn)
        {
            foreach(string i in mygroups)
            {
                if (groupdn == i)
                {

                    return;
                }
            }
            
            DirectoryEntry de2 = new DirectoryEntry("LDAP://" + groupdn);
            if (Program.username1 != null && Program.password1 != null)
            {
                de2.Username = Program.username1;
                de2.Password = Program.password1;
            }
            PropertyValueCollection pvc = de2.Properties["memberOf"];
            if (pvc.Count > 0)
            {
                foreach (string g in pvc)
                {
                    Console.WriteLine("[+] {0}",g);
                    mygroups.Add(g);
                    nestedgroups(g);
                }
            }
        }

        static void nestedgroupswithip(string groupdn,string ip)
        {
            foreach (string i in mygroups)
            {

                if (groupdn ==i)
                {
                    return;
                }
            }
            DirectoryEntry de2 = new DirectoryEntry(String.Format("LDAP://{0}/{1}" ,ip, groupdn));
            if (Program.username1 != null && Program.password1 != null)
            {
                de2.Username = Program.username1;
                de2.Password = Program.password1;
            }
            PropertyValueCollection pvc = de2.Properties["memberOf"];
            if (pvc.Count > 0)
            {
                foreach (string g in pvc)
                {
                    Console.WriteLine("[+] {0}", g);
                    mygroups.Add(g);

                    nestedgroupswithip(g,ip);
                }
            }
        }




        static void Main(string[] args)
        {

            // getmygroups -user username -domain domainname -server serverip

            cliparse cli = new cliparse();

            string username = cli.getargvalue(args, "-user");
            if (username == null)
            {
                Console.WriteLine("Please supply -user [username]");
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
            if(Program.username1!=null && Program.password1 != null)
            {
                de.Username = Program.username1;
                de.Password = Program.password1;
            }

            DirectorySearcher ds = new DirectorySearcher(de);
            ds.Filter = "(&(objectclass=user)(samaccountname=" + username + "))";
            //ds.Filter = "(objectclass=group)";
            ds.SearchScope = SearchScope.Subtree;
            ds.PropertiesToLoad.AddRange(new string[] { "memberOf" });

            foreach(SearchResult sr in ds.FindAll())
            {
                foreach(string i in sr.Properties["memberOf"])
                {
                    Console.WriteLine("[+] {0}",i);

                    if (serverip != null)
                    {
                        nestedgroupswithip(i, serverip);
                    }
                    else
                    {
                        nestedgroups(i);

                    }



                }
            }

            /*
            List<string> groups = new List<string>();
            foreach(SearchResult sr in ds.FindAll())
            {
                string groupname = sr.Properties["name"][0].ToString();
                groups.Add(groupname);
            }


            PrincipalContext pc = new PrincipalContext(ContextType.Domain, domainname);
            
            UserPrincipal user = UserPrincipal.FindByIdentity(pc, username);

            checkgroupmembership(pc, (Principal) user, groups);*/



        }
    }
}
