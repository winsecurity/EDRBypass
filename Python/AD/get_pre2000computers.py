
from ldap3 import Server,Connection,ALL
import argparse



parser = argparse.ArgumentParser(prog="Get Pre windows 2000 computers",
                                 description="Retrieve computers that are configured as pre windows 2000,\
                                 default password can be the lowercase computername without $ ")


parser.add_argument("--domain",help="Domain name is required")
parser.add_argument("--server",help="IP address of DC")
parser.add_argument("--username",default="anonymous",help="Specify username to authenticate as, defaults to anonymous")
parser.add_argument("--password",default="anonymous",help="Specify password to authenticate as user, defaults to anonymous")
parser.add_argument("--searchbase",help="Specify the searcbase from which you want to enumerate")

args = parser.parse_args()

if args.domain == None and args.server==None:
    parser.error("Domain name or Server IP is required")


server = Server(args.server if args.server else args.domain ,get_info="ALL")
myconnection = Connection(server,user=args.username,password=args.password)
myconnection.bind()


# finding naming context DC=TECH69,DC=LOCAL
l = str(server.info).splitlines()
namingcontext = ""
for i in range(0,len(l)):
    if "defaultNamingContext".lower() in l[i].lower():
        namingcontext = (l[i+1]).strip()



myconnection.search(args.searchbase if args.searchbase else namingcontext,
                    "(&(useraccountcontrol=4128))",
                    # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                    "SUBTREE",attributes=["samaccountname", "distinguishedname"])



for i in myconnection.entries:
    print(i["samaccountname"])
    print(i["distinguishedname"])
    print()

#print(myconnection)


#print(myconnection.extend.standard.who_am_i())

