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
    class adenum
    {

        public static void f1(string[] args)
        {
            // Get-DomainForest
            // get-domainforest 

            //cliparse cli = new cliparse();
            //string domain = cli.getargvalue(args, "-domain");


            Forest f = Forest.GetCurrentForest();
            Console.WriteLine("Forest name: {0}",f.Name);
            Console.WriteLine("Root Domain: {0}",f.RootDomain);
            Console.WriteLine("Schema: {0}",f.Schema.Name);
            Console.WriteLine("Schema role owner: {0}",f.SchemaRoleOwner.Name);
            Console.WriteLine("Forest Mode: {0}",f.ForestMode);
            Console.WriteLine("Forest Mode Level: {0}",f.ForestModeLevel);

            Console.WriteLine();
            Console.WriteLine("Domains in the forest");
            foreach(Domain domain in f.Domains)
            {
                Console.WriteLine("--------------------------");
                Console.WriteLine("Domain name: {0}",domain.Name);
                foreach(DomainController dc in domain.FindAllDiscoverableDomainControllers())
                {
                    Console.WriteLine("Domain Controller: {0}",dc.Name);
                    
                }
                Console.WriteLine("--------------------------");
                Console.WriteLine();
            }



        }

        public static void f2(string[] args)
        {
            // Get-Domain
            // get-domain -domain tech69.local 


            cliparse cli = new cliparse();
            string domain = cli.getargvalue(args,"-domain");
            Console.WriteLine(domain);
            Forest f = Forest.GetCurrentForest();
            if(!(domain is null))
            {
                foreach(Domain d in f.Domains)
                {
                    if (d.Name.ToLower() == domain.ToLower())
                    {
                        Console.WriteLine("Forest: {0}",d.Forest);
                        Console.WriteLine("Domain name: {0}", d.Name);
                        //Console.WriteLine("Parent domain: {0}", d.Parent.Name);
                        SecurityIdentifier si = new SecurityIdentifier((byte[])d.GetDirectoryEntry().Properties["objectsid"][0],0);
                        Console.WriteLine("Domain sid: {0}",si.ToString());
                        foreach (DomainController dc in d.FindAllDomainControllers())
                        {
                            Console.WriteLine("Domain Controller: {0}", dc.Name);
                        }
                        Console.WriteLine("Pdc role owner: {0}",d.PdcRoleOwner);
                        Console.WriteLine("Infrastructure role owner: {0}",d.InfrastructureRoleOwner);

                        return;


                    }

                }
            }

            foreach(Domain d in f.Domains)
            {
                Console.WriteLine("Forest: {0}", d.Forest);
                Console.WriteLine("Domain name: {0}", d.Name);
                //Console.WriteLine("Parent domain: {0}", d.Parent.Name);
                SecurityIdentifier si = new SecurityIdentifier((byte[])d.GetDirectoryEntry().Properties["objectsid"][0], 0);
                Console.WriteLine("Domain sid: {0}", si.ToString());
                foreach (DomainController dc in d.FindAllDomainControllers())
                {
                    Console.WriteLine("Domain Controller: {0}", dc.Name);
                }
                Console.WriteLine("Pdc role owner: {0}", d.PdcRoleOwner);
                Console.WriteLine("Infrastructure role owner: {0}", d.InfrastructureRoleOwner);
                Console.WriteLine();
            }

        }


        public static void f3(string[] args)
        {
            //get-domainsid 
            // get-domainsid -domain tech69.local -server 10.1.1.1

            cliparse cli = new cliparse();
            string domain = cli.getargvalue(args, "-domain");
            string server = cli.getargvalue(args, "-server");
            
            if(domain is null && server is null)
            {
                domain = Domain.GetCurrentDomain().Name;
            }

            DirectoryEntry de;

            if(!(server is null))
            {
                de = new DirectoryEntry(String.Format("LDAP://{0}", server));
            }
            else 
            {
                string[] dcs = domain.Split('.');

                for (int i = 0; i < dcs.Length; i++)
                {
                    dcs[i] = "DC=" + dcs[i];
                    //Console.WriteLine(dcs[i]);
                }
                de = new DirectoryEntry(String.Format("LDAP://{0}", String.Join(",", dcs)));
            }


            SecurityIdentifier si = new SecurityIdentifier((byte[])de.Properties["objectsid"][0], 0);
            Console.WriteLine("{0}",si.ToString());

        }


    }
}
