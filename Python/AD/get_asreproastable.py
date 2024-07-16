
from ldap3 import Server,Connection,ALL
import argparse
import math



parser = argparse.ArgumentParser(prog="Get Kerberoastable users",
                                 description="Retrieve users that can be kerberoastable")


parser.add_argument("--domain",help="Domain name is required")
parser.add_argument("--server",help="IP address of DC")
parser.add_argument("--username",default="anonymous",help="Specify username to authenticate as, defaults to anonymous")
parser.add_argument("--password",default="anonymous",help="Specify password to authenticate as user, defaults to anonymous")
parser.add_argument("--searchbase",help="Specify the searcbase from which you want to enumerate")

args = parser.parse_args()




def getuacflags(uac):
    uacvalues = []
    uacvalues.append(""); uacvalues.append("ACCOUNTDISABLE"); uacvalues.append(""); uacvalues.append("HOMEDIR_REQUIRED");
    uacvalues.append("LOCKOUT"); uacvalues.append("PASSWD_NOTREQD"); uacvalues.append("PASSWD_CANT_CHANGE");
    uacvalues.append("ENCRYPTED_TEXT_PWD_ALLOWED"); uacvalues.append("TEMP_DUPLICATE_ACCOUNT");
    uacvalues.append("NORMAL_ACCOUNT"); uacvalues.append(""); uacvalues.append("INTERDOMAIN_TRUST_ACCOUNT");
    uacvalues.append("WORKSTATION_TRUST_ACCOUNT"); uacvalues.append("SERVER_TRUST_ACCOUNT");uacvalues.append(""); uacvalues.append("");
    uacvalues.append("DONT_EXPIRE_PASSWORD"); uacvalues.append("MNS_LOGON_ACCOUNT");
    uacvalues.append("SMARTCARD_REQUIRED"); uacvalues.append("TRUSTED_FOR_DELEGATION");
    uacvalues.append("NOT_DELEGATED"); uacvalues.append("USE_DES_KEY_ONLY");
    uacvalues.append("DONT_REQ_PREAUTH"); uacvalues.append("PASSWORD_EXPIRED");
    uacvalues.append("TRUSTED_TO_AUTH_FOR_DELEGATION");

    uacflags = [ ]
   
    uacbinarystring = "{0:b}".format(uac)

    for i in range(0,len(uacbinarystring)):
        result = uac & int((math.pow(2,i)))
        if result!=0:
            uacflags.append(uacvalues[i])

    return uacflags







if args.domain == None and args.server==None:
    parser.error("Domain name or Server IP is required")


server = Server(args.server if args.server else args.domain ,get_info="ALL")
myconnection = Connection(server,user=args.username,password=args.password)
myconnection.bind()


# finding naming context DC=TECH69,DC=LOCAL
l = str(server.info).splitlines()
namingcontext = ""
for i in range(0,len(l)):
    if "defaultNamingContext" in l[i]:
        namingcontext = (l[i+1]).strip()



myconnection.search(args.searchbase if args.searchbase else namingcontext,
                    "(&(objectclass=user)(useraccountcontrol>=4194303))",
                    "SUBTREE",attributes=["useraccountcontrol","name"])



for i in range(0,len(myconnection.entries)):
    l = str(myconnection.entries[i]).splitlines()
    for j in l:
        if "useraccountcontrol" in j.lower():
            uac = int(j.split(":")[-1].strip())
            uacflags = getuacflags(uac)
            for k in uacflags:
                if k=="DONT_REQ_PREAUTH":
                    print(myconnection.entries[i])
                    print(uacflags)
                    print("-"*100)

#print(myconnection)


#print(myconnection.extend.standard.who_am_i())

