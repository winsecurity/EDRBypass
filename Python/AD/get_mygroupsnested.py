
from ldap3 import Server,Connection,ALL
import argparse



parser = argparse.ArgumentParser(prog="Get my groups nested",
                                 description="Retrieve groups recursively in which user is member of")


parser.add_argument("--domain",help="Domain name is required")
parser.add_argument("--server",help="IP address of DC")
parser.add_argument("--username",default="anonymous",help="Specify username to authenticate as, defaults to anonymous")
parser.add_argument("--password",default="anonymous",help="Specify password to authenticate as user, defaults to anonymous")
parser.add_argument("--searchbase",help="Specify the searcbase from which you want to enumerate")
parser.add_argument("--user",required=True,help="specify the user you wish to find his groups")

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





myconnection.search(args.searchbase if args.searchbase else namingcontext,
                        "(&(objectclass=user)(name={0}))".format(args.user),
                        # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                        "SUBTREE",attributes=["distinguishedname",
                                              "name",
                                            "memberof"])

userdn = str(myconnection.entries[0]["distinguishedname"])


usergroups = list(myconnection.entries[0]["memberof"])


def listgroupmembers(myconnection,groupdn):
    myconnection.search(args.searchbase if args.searchbase else namingcontext,
                        "(&(objectcategory=Group))",
                        # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                        "SUBTREE",attributes=["distinguishedname",
                                              "name","objectcategory",
                                              "memberof"])


    for i in myconnection.entries:
        if str(i["distinguishedname"]) in usergroups:
            continue
        if groupdn in str(i["memberof"]):
            usergroups.append(str(i["distinguishedname"]))


for i in usergroups:
    listgroupmembers(myconnection,str(i))

for i in usergroups:
    print(i)

#currentgroupdn = myconnection.entries[0]["distinguishedname"]
#print(currentgroupdn)
#getgroupmembers(myconnection,str(currentgroupdn))

#getgroupmembers(myconnection,args.groupname)

#print(myconnection)


#print(myconnection.extend.standard.who_am_i())

