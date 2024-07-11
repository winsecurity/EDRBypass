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
            DirectoryEntry de2 = new DirectoryEntry("LDAP://" + groupdn);
            PropertyValueCollection pvc = de2.Properties["memberOf"];
            if (pvc.Count > 0)
            {
                foreach (string g in pvc)
                {
                    Console.WriteLine("[+] {0}",g);
                    nestedgroups(g);
                }
            }
        }

        static void nestedgroupswithip(string groupdn,string ip)
        {
            DirectoryEntry de2 = new DirectoryEntry(String.Format("LDAP://{0}/{1}" ,ip, groupdn));
            PropertyValueCollection pvc = de2.Properties["memberOf"];
            if (pvc.Count > 0)
            {
                foreach (string g in pvc)
                {
                    Console.WriteLine("[+] {0}", g);
                    nestedgroupswithip(g,ip);
                }
            }
        }




        static void Main(string[] args)
        {

            // getmygroups -user username -domain domainname -server serverip

            cliparse cli = new cliparse();

            string username = cli.getargvalue(args, "-username");
            if (username == null)
            {
                Console.WriteLine("Please supply -username [username]");
                System.Environment.Exit(0);
            }

          

            DirectoryEntry de;

            string domainname = cli.getargvalue(args, "-domain");
            string serverip = cli.getargvalue(args, "-server");

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
