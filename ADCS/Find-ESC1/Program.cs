using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

//using PKI.CertificateTemplates;

//using SysadminsLV.PKI.Enrollment;

namespace get_ca
{
    class Program
    {
        public static string username = null;
        public static string password = null;

        public static List<String> getEnrollmentFlags(int flagnumber)
        {

            List<string> l = new List<string>();
            l.Add("CT_FLAG_INCLUDE_SYMMETRIC_ALGORITHMS");
            l.Add("CAManagerApproval-CT_FLAG_PEND_ALL_REQUESTS");
            l.Add("KraPublish-CT_FLAG_PUBLISH_TO_KRA_CONTAINER");
            l.Add("DsPublish-CT_FLAG_PUBLISH_TO_DS");

            l.Add("AutoenrollmentCheckDsCert-CT_FLAG_AUTO_ENROLLMENT_CHECK_USER_DS_CERTIFICATE");
            l.Add("Autoenrollment-CT_FLAG_AUTO_ENROLLMENT");
            l.Add("ReenrollExistingCert-CT_FLAG_PREVIOUS_APPROVAL_VALIDATE_REENROLLMENT");
            l.Add("RequireUserInteraction-CT_FLAG_USER_INTERACTION_REQUIRED");
            l.Add("RemoveInvalidFromStore-CT_FLAG_REMOVE_INVALID_CERTIFICATE_FROM_PERSONAL_STORE");
            l.Add("AllowEnrollOnBehalfOf-CT_FLAG_ALLOW_ENROLL_ON_BEHALF_OF");
            l.Add("IncludeOcspRevNoCheck-CT_FLAG_ADD_OCSP_NOCHECK");

            l.Add("ReuseKeyTokenFull-CT_FLAG_ENABLE_KEY_REUSE_ON_NT_TOKEN_KEYSET_STORAGE_FULL");
            l.Add("NoRevocationInformation-CT_FLAG_NOREVOCATIONINFOINISSUEDCERTS");
            l.Add("BasicConstraintsInEndEntityCerts-CT_FLAG_INCLUDE_BASIC_CONSTRAINTS_FOR_EE_CERTS");
            l.Add("IgnoreEnrollOnReenrollment-CT_FLAG_ALLOW_PREVIOUS_APPROVAL_KEYBASEDRENEWAL_VALIDATE_REENROLLMENT");
            l.Add("IssuancePoliciesFromRequest-CT_FLAG_ISSUANCE_POLICIES_FROM_REQUEST");
            l.Add("SkipAutoRenewal-CT_FLAG_SKIP_AUTO_RENEWAL");
            l.Add("DoNotIncludeSidExtension-CT_FLAG_NO_SECURITY_EXTENSION");


            string uac_binary = Convert.ToString(flagnumber, 2);
            List<string> flags = new List<string>();
            //Console.WriteLine(l.Count);
            //Console.WriteLine(uac_binary.Length);
            for (int i = 0; i < uac_binary.Length; i++)
            {
                int result2 = flagnumber & Convert.ToInt32(Math.Pow(2, i));
                if (result2 != 0)
                {
                    //Console.WriteLine(l[i]);
                    flags.Add(l[i]);
                }

            }

            return flags;


        }


        public static string getKeyUsage(string oid)
        {
            Dictionary<string, string> d = new Dictionary<string, string>();
            d.Add("1.3.6.1.5.5.7.3.2", "client authentication");
            d.Add("1.3.6.1.4.1.311.10.3.4", "encrypting file system");
            d.Add("1.3.6.1.5.5.7.3.3", "code signing");
            d.Add("1.3.6.1.4.1.311.20.2.1", "certificate request agent");
            d.Add("1.3.6.1.4.1.311.20.2.2", "smart card logon");
            d.Add("1.3.6.1.4.1.311.10.3.11", "key recovery");
            d.Add("1.3.6.1.4.1.311.21.6", "key recovery agent");
            d.Add("2.5.29.37.0", "any purpose");


            if (d.ContainsKey(oid))
            {
                return d[oid];
                
            }
            else
            {
                return null;
            }
        }


        public static List<string> getSubjectNameFlags(int flag)
        {

            Dictionary<string, uint> d = new Dictionary<string, uint>();

            List<string> flags = new List<string>();

            d.Add("CT_FLAG_ENROLLEE_SUPPLIES_SUBJECT", 1);
            d.Add("CT_FLAG_ENROLLEE_SUPPLIES_SUBJECT_ALT_NAME", 0x00010000);
            d.Add("CT_FLAG_SUBJECT_ALT_REQUIRE_DOMAIN_DNS", 0x00400000);
            d.Add("CT_FLAG_SUBJECT_ALT_REQUIRE_SPN", 0x00800000);
            d.Add("CT_FLAG_SUBJECT_ALT_REQUIRE_DIRECTORY_GUID", 0x01000000);
            d.Add("CT_FLAG_SUBJECT_ALT_REQUIRE_UPN", 0x02000000);
            d.Add("CT_FLAG_SUBJECT_ALT_REQUIRE_EMAIL", 0x04000000);
            d.Add("CT_FLAG_SUBJECT_ALT_REQUIRE_DNS", 0x08000000);
            d.Add("CT_FLAG_SUBJECT_REQUIRE_DNS_AS_CN", 0x10000000);
            d.Add("CT_FLAG_SUBJECT_REQUIRE_EMAIL", 0x20000000);
            d.Add("CT_FLAG_SUBJECT_REQUIRE_COMMON_NAME", 0x40000000);
            d.Add("CT_FLAG_SUBJECT_REQUIRE_DIRECTORY_PATH", 0x80000000);
            d.Add("CT_FLAG_OLD_CERT_SUPPLIES_SUBJECT_AND_ALT_NAME", 0x00000008);

            foreach(string keyname in d.Keys)
            {
                if ((flag & d[keyname]) == d[keyname])
                {
                    flags.Add(keyname);
                }
            }
            return flags;
        }


        public static string getextendedrightname(string guid)
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


        public static List<string> checkiflowusercanenroll(AuthorizationRuleCollection arc)
        {
            List<string> ids = new List<string>();

            List<string> highpriv = new List<string>();
            highpriv.Add("enterprise admins");
            highpriv.Add("domain admins");
            highpriv.Add("builtin");
            highpriv.Add("schema admins");
            highpriv.Add("nt authority");
            
            foreach (ActiveDirectoryAccessRule ace in arc)
            {
                //0e10c968-78fb-11d2-90d4-00c04f79dc55

                if (ace.ObjectType.ToString() == "0e10c968-78fb-11d2-90d4-00c04f79dc55")
                {
                    // we need to filter out the high privileged users/groups
                    bool ishighpriv = false;
                    foreach (string j in highpriv)
                    {
                        if (ace.IdentityReference.ToString().ToLower().Contains(j))
                        {
                            // this identity reference is a high privileged
                            ishighpriv = true;
                            break;
                        }


                        // checking if sids containing 512, 519 or 500 at the end
                        string[] rid = ace.IdentityReference.ToString().Split('-');
                        if (rid.Length > 0)
                        {
                            if (rid[rid.Length-1] == "512" || rid[rid.Length-1] == "519" || rid[rid.Length - 1] == "500")
                            {
                                ishighpriv = true;
                                break;
                            }
                        }

                    }
                    if (ishighpriv == false)
                    {
                        ids.Add(ace.IdentityReference.ToString());
                    }
                    //Console.WriteLine("Certificate Enrollment rights are present");
                }
            }

            return ids;

        }

        static void Main(string[] args)
        {
            // get_ca   -domain domainname
            // -server serverip
            // -username username -password password

            cliparse cli = new cliparse();

            
            DirectoryEntry de;

            string domainname = cli.getargvalue(args, "-domain");
            string serverip = cli.getargvalue(args, "-server");
            Program.username = cli.getargvalue(args, "-username");
            Program.password = cli.getargvalue(args, "-password");

            string domaindn = null;
           

            if (domainname == null && serverip == null)
            {
                string domain = Domain.GetCurrentDomain().ToString();
                string[] dcs = domain.Split('.');

                for (int i = 0; i < dcs.Length; i++)
                {
                    dcs[i] = "DC=" + dcs[i];
                    //Console.WriteLine(dcs[i]);
                }
                de = new DirectoryEntry(String.Format("LDAP://CN=Configuration,{0}", String.Join(",", dcs)));

            }


            else if (serverip != null)
            {
                de = new DirectoryEntry(String.Format("LDAP://{0}/rootDSE", serverip));
                if (Program.username != null && Program.password != null)
                {
                    de.Username = Program.username;
                    de.Password = Program.password;
                }
                domaindn = de.Properties["defaultnamingcontext"][0].ToString();
                de = new DirectoryEntry(String.Format("LDAP://{0}/CN=Configuration,{1}", serverip, domaindn));
            }

            else
            {
                string[] dcs = domainname.Split('.');

                for (int i = 0; i < dcs.Length; i++)
                {
                    dcs[i] = "DC=" + dcs[i];
                    //Console.WriteLine(dcs[i]);
                }
                de = new DirectoryEntry(String.Format("LDAP://CN=Configuration,{0}", String.Join(",", dcs)));

            }

            if (Program.username != null && Program.password != null)
            {
                de.Username = Program.username;
                de.Password = Program.password;
            }

            
            DirectorySearcher ds = new DirectorySearcher(de);
            ds.Filter = "(objectclass=pkicertificatetemplate)";
           
            
            foreach (SearchResult sr in ds.FindAll())
            {
                
                
                string oid = sr.Properties["msPKI-Cert-Template-OID"][0].ToString();
                string enrollmentflag= sr.Properties["msPKI-Enrollment-Flag"][0].ToString();
                int numberofauthorizationsignaturesrequired = Convert.ToInt32(sr.Properties["msPKI-RA-Signature"][0]);
                if (numberofauthorizationsignaturesrequired > 0)
                {
                    continue;
                }

                /*CertificateTemplate ct =  CertificateTemplate.FromOid(oid);

                Console.WriteLine(ct.DisplayName);
                Console.WriteLine("Auto Enrollment allowed: {0}",ct.AutoenrollmentAllowed);
                Console.WriteLine("Manager approval required?: {0}",ct.Settings.CAManagerApproval);
                
                Console.WriteLine(ct.Settings.EnrollmentOptions);*/


                List<string> eflags = getEnrollmentFlags(
                    Convert.ToInt32(enrollmentflag));

                bool managerapproval = false;

                foreach(string flag in eflags)
                {
                    if (flag.ToLower().Contains("camanagerapproval"))
                    {

                        managerapproval = true;
                    }
                    
                }
                if (managerapproval == false)
                {

                    int flag = Convert.ToInt32(sr.Properties["msPKI-Certificate-Name-Flag"][0]);
                    List<string> flags = getSubjectNameFlags(flag);
                    foreach(string i in flags)
                    {
                        if(i== "CT_FLAG_ENROLLEE_SUPPLIES_SUBJECT")
                        {

                            ActiveDirectorySecurity ads = sr.GetDirectoryEntry().ObjectSecurity;
                            AuthorizationRuleCollection arc = ads.GetAccessRules(true, true, typeof(NTAccount));

                            List<string> ids = checkiflowusercanenroll(arc);
                            if (ids.Count>0)
                            {

                                Console.WriteLine("Template Name: {0}", sr.Properties["name"][0]);
                                Console.WriteLine("Manager approval needed: false");
                                Console.WriteLine("Number of Authorized signatures required: {0}",numberofauthorizationsignaturesrequired);
                                Console.WriteLine("mspki-Certificate-Name-Flag: {0}", String.Join(", ", flags));

                                Console.WriteLine("mspki-Enrollment-Flag: {0}", String.Join(", ", eflags));

                                Console.WriteLine("Enrollment rights: {0}",String.Join(", ",ids));
                                foreach (var oid2 in sr.Properties["pKIExtendedKeyUsage"])
                                {
                                    Console.WriteLine("pKI-Extended-KeyUsage: {0}",String.Join(", ",getKeyUsage((string)oid2)));
                                }

                                Console.WriteLine("-------------------------------------------------");


                            }







                        }
                    }
                    



                }
               // Console.WriteLine("-------------------------------------------------");
            }

        }
    }
}
