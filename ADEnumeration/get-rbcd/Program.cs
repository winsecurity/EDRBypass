using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Security.Principal;
using System.Security.AccessControl;
using System.DirectoryServices.AccountManagement;
using System.IO;

namespace get_rbcd
{
    class Program
    {
        public static string username = null;
        public static string password = null;



        static void Main(string[] args)
        {
            // get_rbcd   -domain domainname
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
            ds.Filter = "(objectclass=computer)";

            foreach (SearchResult sr in ds.FindAll())
            {

                DirectoryEntry obj =  sr.GetDirectoryEntry();
                ActiveDirectorySecurity ads = obj.ObjectSecurity;
                
                AuthorizationRuleCollection arc = ads.GetAccessRules(true,true,typeof(NTAccount));


                Dictionary<string, List<string>> d = new Dictionary<string, List<string>>();
                string username = "";
                List<string> adrights = new List<string>();

                foreach (ActiveDirectoryAccessRule ace in arc)
                {


                    string ireference = ace.IdentityReference.ToString().ToLower();
                    string rights = ace.ActiveDirectoryRights.ToString().ToLower();
                    if(ireference.Contains("nt authority")||
                       ireference.Contains("domain admins")||
                       ireference.Contains("system") ||
                       ireference.Contains("enterprise admins")||
                       ireference.Contains("key admins")||
                       ireference.Contains("built in")
                      )
                    {
                        
                        continue;
                    }

                    if (rights.Contains("write") ||
                        rights.Contains("generic all")||
                        rights.Contains("generic write")||
                        rights.Contains("owner") ||
                        rights.Contains("full control")||
                        rights.Contains("genericall") ||
                        rights.Contains("fullcontrol")
                           
                        )
                    {

                      

                        try
                        {
                            //PrincipalContext pc = new PrincipalContext(ContextType.Domain,domainname);
                            //Console.WriteLine(ace.IdentityReference.Translate(typeof(SecurityIdentifier)).Value);
                            //UserPrincipal user = UserPrincipal.FindByIdentity(pc, ace.IdentityReference.Translate(typeof(NTAccount)).Value);

                            string identitysid = ace.IdentityReference.Translate(typeof(SecurityIdentifier)).Value;
                            string[] rids = identitysid.Split('-');
                            int rid = Convert.ToInt32(rids[rids.Length - 1]);

                            if (rid >= 600)
                            {
                                username = ace.IdentityReference.ToString();
                                adrights.Add(ace.ActiveDirectoryRights.ToString());

                                if (d.ContainsKey(username)==false)
                                {
                                    d.Add(username, adrights);
                                    
                                }
                                else
                                {
                                    // key is already present
                                    List<string> contents = d[username];
                                    contents.AddRange(adrights);
                                    d.Remove(username);
                                    d.Add(username, contents);
                                }

                                
                                
                                
                                /*Console.WriteLine("Computer name: {0}", obj.Name);
                                Console.WriteLine("Identity Reference: {0}", ace.IdentityReference.Translate(typeof(NTAccount)));
                                Console.WriteLine("GUID: {0}", ace.ObjectType);
                                Console.WriteLine("Ace Allowed or Denied: {0}", ace.AccessControlType);
                                Console.WriteLine("Rights: {0}", ace.ActiveDirectoryRights);
                                Console.WriteLine();*/

                                

                            }

                        }
                        catch (Exception e) { Console.WriteLine(e.ToString()); }
                    }

                  

                }

                Console.WriteLine("Computer: {0}",obj.Name);
                foreach(var key in d.Keys)
                {
                    Console.WriteLine("User: {0}",key);
                    //d[key].Distinct<string>().ToArray();
                    Console.WriteLine(String.Join(",", d[key].Distinct<string>().ToArray()));
                }
                Console.WriteLine();
            }

        }
    }
}
