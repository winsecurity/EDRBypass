using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Security.Principal;
using System.Security.AccessControl;

namespace get_dcsync
{
    class Program
    {

        public static string username;
        public static string password;



        static string getextendedrightname(string guid)
        {
            Hashtable ht = new Hashtable();
            ht.Add("00299570-246d-11d0-a768-00aa006e0529", "User-Force-Change-Password");
            ht.Add("ab721a53-1e2f-11d0-9819-00aa0040529b", "User-Change-Password");
            ht.Add("ee914b82-0a98-11d1-adbb-00c04fd8d5cd", "Abandon-Replication");
            ht.Add("440820ad-65b4-11d1-a3da-0000f875ae0d", "Add-GUID");
            ht.Add("1abd7cf8-0a99-11d1-adbb-00c04fd8d5cd", "Allocate-Rids");
            ht.Add("68b1d179-0d15-4d4f-ab71-46152e79a7bc", "Allowed-To-Authenticate");
            ht.Add("edacfd8f-ffb3-11d1-b41d-00a0c968f939", "Apply-Group-Policy");
            ht.Add("0e10c968-78fb-11d2-90d4-00c04f79dc55", "Certificate-Enrollment");
            ht.Add("014bf69c-7b3b-11d1-85f6-08002be74fab", "Change-Domain-Master");
            ht.Add("cc17b1fb-33d9-11d2-97d4-00c04fd8d5cd", "Change-Infrastructure-Master");
            ht.Add("bae50096-4752-11d1-9052-00c04fc2d4cf", "Change-PDC");
            ht.Add("d58d5f36-0a98-11d1-adbb-00c04fd8d5cd", "Change-Rid-Master");
            ht.Add("e12b56b6-0a95-11d1-adbb-00c04fd8d5cd", "Change-Schema-Master");
            ht.Add("e2a36dc9-ae17-47c3-b58b-be34c55ba633", "Create-Inbound-Forest-Trust");
            ht.Add("fec364e0-0a98-11d1-adbb-00c04fd8d5cd", "Do-Garbage-Collection");
            ht.Add("ab721a52-1e2f-11d0-9819-00aa0040529b", "Domain-Administer-Server");
            ht.Add("69ae6200-7f46-11d2-b9ad-00c04f79f805", "DS-Check-Stale-Phantoms");
            ht.Add("3e0f7e18-2c7a-4c10-ba82-4d926db99a3e", "DS-Clone-Domain-Controller");
            ht.Add("2f16c4a5-b98e-432c-952a-cb388ba33f2e", "DS-Execute-Intentions-Script");
            ht.Add("9923a32a-3607-11d2-b9be-0000f87a36b2", "DS-Install-Replica");
            ht.Add("4ecc03fe-ffc0-4947-b630-eb672a8a9dbc", "DS-Query-Self-Quota");
            ht.Add("1131f6aa-9c07-11d1-f79f-00c04fc2dcd2", "DS-Replication-Get-Changes");
            ht.Add("1131f6ad-9c07-11d1-f79f-00c04fc2dcd2", "DS-Replication-Get-Changes-All");

            ht.Add("89e95b76-444d-4c62-991a-0facbeda640c", "DS-Replication-Get-Changes-In-Filtered-Set");

            ht.Add("1131f6ac-9c07-11d1-f79f-00c04fc2dcd2", "DS-Replication-Manage-Topology");

            ht.Add("f98340fb-7c5b-4cdb-a00b-2ebdfa115a96", "DS-Replication-Monitor-Topology");

            ht.Add("1131f6ab-9c07-11d1-f79f-00c04fc2dcd2", "DS-Replication-Synchronize");

            ht.Add("05c74c5e-4deb-43b4-bd9f-86664c2a7fd5", "Enable-Per-User-Reversibly-Encrypted-Password");

            ht.Add("b7b1b3de-ab09-4242-9e30-9980e5d322f7", "Generate-RSoP-Logging");
            ht.Add("b7b1b3dd-ab09-4242-9e30-9980e5d322f7", "Generate-RSoP-Planning");

            ht.Add("7c0e2a7c-a419-48e4-a995-10180aad54dd", "Manage-Optional-Features");
            ht.Add("ba33815a-4f93-4c76-87f3-57574bff8109", "Migrate-SID-History");
            ht.Add("b4e60130-df3f-11d1-9c86-006008764d0e", "msmq-Open-Connector");
            ht.Add("06bd3201-df3e-11d1-9c86-006008764d0e", "msmq-Peek");
            ht.Add("4b6e08c3-df3c-11d1-9c86-006008764d0e", "msmq-Peek-computer-Journal");
            ht.Add("4b6e08c1-df3c-11d1-9c86-006008764d0e", "msmq-Peek-Dead-Letter");
            ht.Add("06bd3200-df3e-11d1-9c86-006008764d0e", "msmq-Receive");
            ht.Add("4b6e08c2-df3c-11d1-9c86-006008764d0e", "msmq-Receive-computer-Journal");
            ht.Add("4b6e08c0-df3c-11d1-9c86-006008764d0e", "msmq-Receive-Dead-Letter");
            ht.Add("06bd3203-df3e-11d1-9c86-006008764d0e", "msmq-Receive-journal");
            ht.Add("06bd3202-df3e-11d1-9c86-006008764d0e", "msmq-Send");
            ht.Add("a1990816-4298-11d1-ade2-00c04fd8d5cd", "Open-Address-Book");
            ht.Add("1131f6ae-9c07-11d1-f79f-00c04fc2dcd2", "Read-Only-Replication-Secret-Synchronization");
            ht.Add("45ec5156-db7e-47bb-b53f-dbeb2d03c40f", "Reanimate-Tombstones");
            ht.Add("0bc1554e-0a99-11d1-adbb-00c04fd8d5cd", "Recalculate-Hierarchy ");
            ht.Add("62dd28a8-7f46-11d2-b9ad-00c04f79f805", "Recalculate-Security-Inheritance");
            ht.Add("ab721a56-1e2f-11d0-9819-00aa0040529b", "Receive-As extended");
            ht.Add("9432c620-033c-4db7-8b58-14ef6d0bf477", "Refresh-Group-Cache");
            ht.Add("1a60ea8d-58a6-4b20-bcdc-fb71eb8a9ff8", "Reload-SSL-Certificate");
            ht.Add("7726b9d5-a4b4-4288-a6b2-dce952e80a7f", "Run-Protect-Admin-Groups-Task");
            ht.Add("91d67418-0135-4acc-8d79-c08e857cfbec", "SAM-Enumerate-Entire-Domain");
            ht.Add("ab721a54-1e2f-11d0-9819-00aa0040529b", "Send-As");
            ht.Add("ab721a55-1e2f-11d0-9819-00aa0040529b", "Send-To");
            ht.Add("ccc2dc7d-a6ad-4a7a-8846-c04e3cc53501", "Unexpire-Password");
            ht.Add("280f369c-67c7-438e-ae98-1d46f3c6f541", "Update-Password-Not-Required-Bit");
            ht.Add("be2bb760-7f46-11d2-b9ad-00c04f79f805", "Update-Schema-Cache");

            string extendedrightname = null;
            foreach (DictionaryEntry d in ht)
            {
                if (d.Key.ToString() == guid)
                {
                    extendedrightname = d.Value.ToString();
                }
            }
            return extendedrightname;

        }

        static void Main(string[] args)
        {

            // get-dcsync  -domain domainname -server serverip -domaindn domaindn
            // -username username -password password

            cliparse cli = new cliparse();

            string domaindn = cli.getargvalue(args, "-domaindn");
            if (domaindn == null)
            {
                Console.WriteLine("Please supply -domainDN DC=TECH69,DC=LOCAL");
                System.Environment.Exit(0);
            }



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

            if(Program.username!=null && Program.password != null)
            {
                de.Username = Program.username;
                de.Password = Program.password;
            }

            DirectorySearcher ds = new DirectorySearcher(de);
            ds.Filter = "(distinguishedname=" + domaindn +")";
            ds.SearchRoot = de;
            //ds.SearchScope = SearchScope.Base;

            string[] dcsyncrights = new string[6] { "1131f6aa-9c07-11d1-f79f-00c04fc2dcd2", 
                "1131f6ad-9c07-11d1-f79f-00c04fc2dcd2", 
                "89e95b76-444d-4c62-991a-0facbeda640c","1131f6ac-9c07-11d1-f79f-00c04fc2dcd2",
            "f98340fb-7c5b-4cdb-a00b-2ebdfa115a96","1131f6ab-9c07-11d1-f79f-00c04fc2dcd2"};
            
            
            foreach(SearchResult sr in ds.FindAll())
            {
                DirectoryEntry tempde = sr.GetDirectoryEntry();
               
                
                    //Console.WriteLine(tempde.Name);
                    ActiveDirectorySecurity ads = tempde.ObjectSecurity;
                    AuthorizationRuleCollection arc = ads.GetAccessRules(true, true, typeof(NTAccount));

                    foreach (ActiveDirectoryAccessRule ace in arc)
                    {

                        //ht.Add("1131f6aa-9c07-11d1-f79f-00c04fc2dcd2", "DS-Replication-Get-Changes");
                        //ht.Add("1131f6ad-9c07-11d1-f79f-00c04fc2dcd2", "DS-Replication-Get-Changes-All");
                        //ht.Add("89e95b76-444d-4c62-991a-0facbeda640c", "DS-Replication-Get-Changes-In-Filtered-Set");

                        foreach (string rightguid in dcsyncrights)
                        {
                            if (rightguid == ace.ObjectType.ToString())
                            {
                                Console.WriteLine("Identity Reference: {0}",ace.IdentityReference);
                                Console.WriteLine("Rights: {0}",ace.ActiveDirectoryRights);
                                Console.WriteLine("Access type: {0}",ace.AccessControlType);


                                // rights guid
                                Console.WriteLine("Right guid: {0}",ace.ObjectType);
                                Console.WriteLine("Resolved guid name: {0}",getextendedrightname(ace.ObjectType.ToString()));
                                Console.WriteLine();
                            }
                        }

                    }

                


            }





        }
    }
}
