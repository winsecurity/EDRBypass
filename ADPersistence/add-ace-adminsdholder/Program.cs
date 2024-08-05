using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Security.AccessControl;

namespace add_ace_adminsdholder
{
    class Program
    {
        public static string username = null;
        public static string password = null;



        static void Main(string[] args)
        {
            // add_ace_adminsdholder -samaccountname [name] -right [rightname]
            // -domain domainname
            // -server serverip
            // -username username -password password

            cliparse cli = new cliparse();



            DirectoryEntry de;

            string domainname = cli.getargvalue(args, "-domain");
            string serverip = cli.getargvalue(args, "-server");
            Program.username = cli.getargvalue(args, "-username");
            Program.password = cli.getargvalue(args, "-password");

            string samname = cli.getargvalue(args, "-samaccountname");
            string right = cli.getargvalue(args, "-right");

            if(samname==null || right == null)
            {
                Console.WriteLine("-samaccountname [name] -right [rightname] are mandatory");
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
            
            ds.Filter = "(name=AdminSDHolder)";

            foreach (SearchResult sr in ds.FindAll())
            {
                string name = sr.Properties["name"][0].ToString();
                //Console.WriteLine(name);

                DirectoryEntry sdholder =  sr.GetDirectoryEntry();
                ActiveDirectorySecurity ads = sdholder.ObjectSecurity;

                foreach(ActiveDirectoryAccessRule ar in ads.GetAccessRules(true,true,typeof(NTAccount)))
                {
                    //Console.WriteLine(ar.IdentityReference);
                    //Console.WriteLine(ar.ActiveDirectoryRights);
                    //Console.WriteLine(ar.AccessControlType);
                    //Console.WriteLine();


                   


                }



                PrincipalContext pc = new PrincipalContext(ContextType.Domain);
                UserPrincipal user = UserPrincipal.FindByIdentity(pc, samname);

                if (right.ToLower().Contains("genericall"))
                {
                    SecurityIdentifier sid1 = user.Sid;
                    ActiveDirectoryAccessRule ar2 = new ActiveDirectoryAccessRule(
                        sid1, ActiveDirectoryRights.GenericAll,
                        AccessControlType.Allow,
                        ActiveDirectorySecurityInheritance.All);


                    ads.AddAccessRule(ar2);
                    sdholder.CommitChanges();
                }

                else if (right.ToLower().Contains("genericread"))
                {
                    SecurityIdentifier sid1 = user.Sid;
                    ActiveDirectoryAccessRule ar2 = new ActiveDirectoryAccessRule(
                        sid1, ActiveDirectoryRights.GenericRead,
                        AccessControlType.Allow,
                        ActiveDirectorySecurityInheritance.All);


                    ads.AddAccessRule(ar2);
                    sdholder.CommitChanges();
                }

                else if (right.ToLower().Contains("genericwrite"))
                {
                    SecurityIdentifier sid1 = user.Sid;
                    ActiveDirectoryAccessRule ar2 = new ActiveDirectoryAccessRule(
                        sid1, ActiveDirectoryRights.GenericWrite,
                        AccessControlType.Allow,
                        ActiveDirectorySecurityInheritance.All);


                    ads.AddAccessRule(ar2);
                    sdholder.CommitChanges();
                }


                else if (right.ToLower().Contains("readproperty"))
                {
                    SecurityIdentifier sid1 = user.Sid;
                    ActiveDirectoryAccessRule ar2 = new ActiveDirectoryAccessRule(
                        sid1, ActiveDirectoryRights.ReadProperty,
                        AccessControlType.Allow,
                        ActiveDirectorySecurityInheritance.All);


                    ads.AddAccessRule(ar2);
                    sdholder.CommitChanges();
                }


                else if (right.ToLower().Contains("writeproperty"))
                {
                    SecurityIdentifier sid1 = user.Sid;
                    ActiveDirectoryAccessRule ar2 = new ActiveDirectoryAccessRule(
                        sid1, ActiveDirectoryRights.WriteProperty,
                        AccessControlType.Allow,
                        ActiveDirectorySecurityInheritance.All);


                    ads.AddAccessRule(ar2);
                    sdholder.CommitChanges();
                }

                else if (right.ToLower().Contains("writeowner"))
                {
                    SecurityIdentifier sid1 = user.Sid;
                    ActiveDirectoryAccessRule ar2 = new ActiveDirectoryAccessRule(
                        sid1, ActiveDirectoryRights.WriteOwner,
                        AccessControlType.Allow,
                        ActiveDirectorySecurityInheritance.All);


                    ads.AddAccessRule(ar2);
                    sdholder.CommitChanges();
                }

                else if (right.ToLower().Contains("writedacl"))
                {
                    SecurityIdentifier sid1 = user.Sid;
                    ActiveDirectoryAccessRule ar2 = new ActiveDirectoryAccessRule(
                        sid1, ActiveDirectoryRights.WriteDacl,
                        AccessControlType.Allow,
                        ActiveDirectorySecurityInheritance.All);


                    ads.AddAccessRule(ar2);
                    sdholder.CommitChanges();
                }

            }

        }
    }
}
