using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Management.Automation;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices;
using System.Security.AccessControl;
using System.Security.Principal;


namespace sharppractice
{


   


    class Program
    {

        public static void randomsleep()
        {
            Random r1 = new Random();
            int randomseconds = r1.Next(10, 100);
            System.Threading.Thread.Sleep(randomseconds * 1000);
        }

        static void Main(string[] args)
        {
            if (args.Length <1)
            {
                Console.WriteLine("[+] Usage: filename [actioncommand] arguments");
                System.Environment.Exit(0);
            }


            switch (args[0].ToLower())
            {
                case "get-domainforest":
                    adenum.f1(args);
                    break;
                case "get-domain":
                    adenum.f2(args);
                    break;
                case "get-domainsid":
                    adenum.f3(args);
                    break;
                case "get-domaintrust":
                    adenum.f4(args);
                    break;
                default:
                    Console.WriteLine("Invalid command");
                    System.Environment.Exit(0);
                    break; 
            }

        
            /*cliparse cli = new cliparse();
            string domain = cli.getargvalue(args, "-domain");
            string server = cli.getargvalue(args, "-server");
            string searchbase = cli.getargvalue(args, "-searchbase");
            
            Console.WriteLine("Domain: {0}",domain);
            Console.WriteLine("{0}",!(server is null));*/


            

            
            
            /*Forest f = Forest.GetCurrentForest();
            
            DomainCollection domains = f.Domains;
            
            foreach(Domain d in domains)
            {
                Console.WriteLine("Domain: {0}",d.Name);
                Console.WriteLine("Domain Mode: {0}", d.DomainMode) ;
                Console.WriteLine("Domain Mode Level: {0}", d.DomainModeLevel);
                TrustRelationshipInformationCollection trusts =  d.GetAllTrustRelationships();
                foreach(TrustRelationshipInformation trustinfo in trusts)
                {
                    
                    Console.WriteLine("{0} -> {1} -> {2} ",trustinfo.SourceName,
                        trustinfo.TrustDirection,trustinfo.TargetName);
                }

                DomainControllerCollection dcs =  d.FindAllDiscoverableDomainControllers();
                foreach(DomainController dc in dcs)
                {
                    Console.WriteLine("---------Domain Controller-------------");
                    Console.WriteLine("DC: {0}",dc.Name);
                    Console.WriteLine("IP Address: {0}", System.Net.IPAddress.Parse(dc.IPAddress));
                    Console.WriteLine("isGlobalCatalog? : {0}",dc.IsGlobalCatalog());
                    Console.WriteLine("OS Version: {0}",dc.OSVersion);
                   
                    
                    //randomsleep();
                }



                // tech69.local  , DC=tech69,DC=local
                string domainName = d.Name.ToString();
                string[] dnname = domainName.Split('.');

                for (int i = 0; i < dnname.Length; i++)
                {
                    dnname[i] = "DC=" + dnname[i];
                    //Console.WriteLine(dcs[i]);
                }

                DirectoryEntry de = new DirectoryEntry(String.Format("LDAP://{0}",args[0]) );//String.Join(",", dnname)));
                DirectorySearcher ds = new DirectorySearcher(de);
                ds.SearchScope = SearchScope.Subtree;
                
                ds.Filter = "(objectclass=computer)";
                foreach(SearchResult sr in ds.FindAll())
                {
                    try
                    {
                        if (sr.Properties["serviceprincipalname"][0] != null)
                        {
                            DirectoryEntry comp1 = sr.GetDirectoryEntry();
                            ActiveDirectorySecurity ads =  comp1.ObjectSecurity;
                            foreach(ActiveDirectoryAccessRule arc in ads.GetAccessRules(true,true,typeof(NTAccount)))
                            {
                                Console.WriteLine();
                            }
                            Console.WriteLine("User samaccountname: {0}", sr.Properties["samaccountname"][0]);
                            Console.WriteLine("User spn: {0}", sr.Properties["serviceprincipalname"][0]);

                        }


                    }
                    catch (Exception e)
                    {

                    }

                }

                
            }
            */
        }
    }
}
