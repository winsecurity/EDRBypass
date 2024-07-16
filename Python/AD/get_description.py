
from ldap3 import Server,Connection,ALL
import argparse



parser = argparse.ArgumentParser(prog="Get Kerberoastable users",
                                 description="Retrieve users that can be kerberoastable")


parser.add_argument("--domain",help="Domain name is required")
parser.add_argument("--server",help="IP address of DC")
parser.add_argument("--username",default="anonymous",help="Specify username to authenticate as, defaults to anonymous")
parser.add_argument("--password",default="anonymous",help="Specify password to authenticate as user, defaults to anonymous")
parser.add_argument("--searchbase",help="Specify the searcbase from which you want to enumerate")
parser.add_argument("--onlyusersdescription",action="store_true",help="Retrieve only users' description")

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
    if "defaultNamingContext" in l[i]:
        namingcontext = (l[i+1]).strip()

if args.onlyusersdescription!=None:
    if args.onlyusersdescription==True:
        myconnection.search(args.searchbase if args.searchbase else namingcontext,
                    "(&(objectclass=user)(description=*))",
                    # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                    "SUBTREE",attributes=["description","name"])
        print(myconnection.entries)
    else:
        myconnection.search(args.searchbase if args.searchbase else namingcontext,
                    "(&(objectclass=*)(description=*))",
                    # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                    "SUBTREE",attributes=["description","name"])

        print(myconnection.entries)

else:

    myconnection.search(args.searchbase if args.searchbase else namingcontext,
                        "(&(objectclass=*)(description=*))",
                        # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                        "SUBTREE",attributes=["description","name"])

    print(myconnection.entries)

#print(myconnection)


#print(myconnection.extend.standard.who_am_i())

