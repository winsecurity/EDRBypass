using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace get_constrained
{
    class Program
    {
        public static string username = null;
        public static string password = null;

        public static List<string> getuacvalues(int uacnumber)
        {

            List<string> l = new List<string>();

            l.Add(""); l.Add("ACCOUNTDISABLE"); l.Add(""); l.Add("HOMEDIR_REQUIRED");
            l.Add("LOCKOUT"); l.Add("PASSWD_NOTREQD"); l.Add("PASSWD_CANT_CHANGE");
            l.Add("ENCRYPTED_TEXT_PWD_ALLOWED"); l.Add("TEMP_DUPLICATE_ACCOUNT");
            l.Add("NORMAL_ACCOUNT"); l.Add(""); l.Add("INTERDOMAIN_TRUST_ACCOUNT");
            l.Add("WORKSTATION_TRUST_ACCOUNT"); l.Add("SERVER_TRUST_ACCOUNT"); l.Add(""); l.Add("");
            l.Add("DONT_EXPIRE_PASSWORD"); l.Add("MNS_LOGON_ACCOUNT");
            l.Add("SMARTCARD_REQUIRED"); l.Add("TRUSTED_FOR_DELEGATION");
            l.Add("NOT_DELEGATED"); l.Add("USE_DES_KEY_ONLY");
            l.Add("DONT_REQ_PREAUTH"); l.Add("PASSWORD_EXPIRED");
            l.Add("TRUSTED_TO_AUTH_FOR_DELEGATION");


            string uac_binary = Convert.ToString(uacnumber, 2);
            List<string> flags = new List<string>();
            //Console.WriteLine(l.Count);
            //Console.WriteLine(uac_binary.Length);
            for (int i = 0; i < uac_binary.Length; i++)
            {
                int result2 = uacnumber & Convert.ToInt32(Math.Pow(2, i));
                if (result2 != 0)
                {
                    //Console.WriteLine(l[i]);
                    flags.Add(l[i]);
                }

            }

            return flags;

        }


        static void Main(string[] args)
        {
            // get_constrained   -domain domainname
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
            ds.Filter = "(msds-allowedtodelegateto=*)";

            foreach (SearchResult sr in ds.FindAll())
            {
                int uac = Convert.ToInt32(sr.Properties["useraccountcontrol"][0]);
                string name = sr.Properties["distinguishedname"][0].ToString();
                Console.WriteLine("Name: {0}",name);
                foreach(var i in sr.Properties["msds-allowedtodelegateto"])
                {
                    Console.WriteLine("delegated to: {0}",i);
                }

                List<string> uacnames = getuacvalues(uac);
                foreach (string i in uacnames)
                {
                    if(i.ToUpper()== "TRUSTED_TO_AUTH_FOR_DELEGATION")
                    {
                        Console.WriteLine(i);
                    }
                }
                Console.WriteLine();
            }


        }
    }
}
