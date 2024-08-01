
from ldap3 import Server,Connection,ALL, MODIFY_ADD, MODIFY_REPLACE
import argparse



parser = argparse.ArgumentParser(prog="Set the object's serviceprincipalname",
                                 description="Set the object's serviceprincipalname when the spn is given ")


parser.add_argument("--domain",help="Domain name is required")
parser.add_argument("--server",help="IP address of DC")
parser.add_argument("--username",default="anonymous",help="Specify username to authenticate as, defaults to anonymous")
parser.add_argument("--password",default="anonymous",help="Specify password to authenticate as user, defaults to anonymous")
parser.add_argument("--searchbase",help="Specify the searcbase from which you want to enumerate")
parser.add_argument("--samaccountname",required=True,help="Specify the object sid")
parser.add_argument("--attributename",required=True,help="Name of attribute you want to add/modify")
parser.add_argument("--attributevalue",required=True,help="Value to be added to attribute of object")

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
                    "(samaccountname={0})".format(args.samaccountname),
                    # (&(objectclass=user)(serviceprincipalname=*)) might be filtered at dc
                    "SUBTREE",attributes=[ args.attributename,"distinguishedname"])



for i in myconnection.entries:
    print("DN: {0}".format(i["distinguishedname"]))
    #mylist = []
    #if i["serviceprincipalname"]==None:
        #mylist.append[args.spn]
    myconnection.modify(str(i["distinguishedname"]),
                    {
                        args.attributename:[ (MODIFY_ADD, [args.attributevalue])]
                    }    )
        
        
print(myconnection.result)
#print(myconnection)


#print(myconnection.extend.standard.who_am_i())

