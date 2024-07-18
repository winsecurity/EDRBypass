
from ldap3 import Server,Connection,ALL
import argparse



parser = argparse.ArgumentParser(prog="Enumerate users by bruteforcing rids",
                                 description="Retrieve users by bruteforcing [rids]")


parser.add_argument("--domain",help="Domain name is required")
parser.add_argument("--server",help="IP address of DC")
parser.add_argument("--username",default="anonymous",help="Specify username to authenticate as, defaults to anonymous")
parser.add_argument("--password",default="anonymous",help="Specify password to authenticate as user, defaults to anonymous")
parser.add_argument("--searchbase",help="Specify the searcbase from which you want to enumerate")
parser.add_argument("--domainsid",help="Specify the domainsid if you know already")
parser.add_argument("--startrid",default=500,type=int,help="Specify the rid you want bruteforce to start with")
parser.add_argument("--endrid",default=2000,type=int,help="Specify the rid you want bruteforce to end with")




args = parser.parse_args()




if args.domain == None and args.server==None:
    parser.error("Domain name or Server IP is required")


if args.startrid and args.endrid:
    if args.startrid>args.endrid:
        parser.error("starting rid should not be greater than end rid")


server = Server(args.server if args.server else args.domain ,get_info="ALL")
myconnection = Connection(server,user=args.username,password=args.password)
myconnection.bind()


# finding naming context DC=TECH69,DC=LOCAL
l = str(server.info).splitlines()
namingcontext = ""
for i in range(0,len(l)):
    if "defaultNamingContext" in l[i]:
        namingcontext = (l[i+1]).strip()

domainname = ""
if args.domainsid==None:
    if args.domain==None:
        domainname = str(args.domain).split(".")[0]
    else:
        domainname = namingcontext.split(",")[0].split("=")[1]
    myconnection.search(args.searchbase if args.searchbase else namingcontext,
                        "(name={0})".format(domainname),
                        # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                        "SUBTREE",attributes=["objectsid"])
    for i in myconnection.entries:
        if "objectsid" in str(i).lower():
            args.domainsid = str(i).split(":")[-1].strip()



for i in range(args.startrid,args.endrid+1):

    myconnection.search(args.searchbase if args.searchbase else namingcontext,
                        "(objectsid={0}-{1})".format(args.domainsid,i),
                        # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                        "SUBTREE",attributes=["name","objectsid"])

    if len(myconnection.entries)>0:
        for i in myconnection.entries:
            for j in str(i).splitlines():
                if "name" in str(j).lower():
                    print(j)
                if "objectsid" in str(j).lower():
                    print(j)
            print()    

#print(myconnection)


#print(myconnection.extend.standard.who_am_i())

