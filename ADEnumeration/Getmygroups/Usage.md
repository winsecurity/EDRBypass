# Usage of getmygroups.exe

### Get the user's group recursively, Eg: if user1 is member of group1 and group1 is member of group2, get all the groups

`getmygroups.exe -server 192.168.0.110 -user Administrator`

`getmygroups.exe -server 192.168.0.110 -user Administrator -username test2 -password P@ssw0rd`

`getmygroups.exe -server 192.168.0.110 -user test1 -domain tech69.local`


## Output:

```
C:\Users\Administrator\Desktop>getmygroups.exe -server 192.168.0.110 -user Administrator
[+] CN=Group Policy Creator Owners,CN=Users,DC=tech69,DC=local
[+] CN=Denied RODC Password Replication Group,CN=Users,DC=tech69,DC=local
[+] CN=Domain Admins,CN=Users,DC=tech69,DC=local
[+] CN=Denied RODC Password Replication Group,CN=Users,DC=tech69,DC=local
[+] CN=Administrators,CN=Builtin,DC=tech69,DC=local
[+] CN=Enterprise Admins,CN=Users,DC=tech69,DC=local
[+] CN=Denied RODC Password Replication Group,CN=Users,DC=tech69,DC=local
[+] CN=Administrators,CN=Builtin,DC=tech69,DC=local
[+] CN=Schema Admins,CN=Users,DC=tech69,DC=local
[+] CN=Denied RODC Password Replication Group,CN=Users,DC=tech69,DC=local
[+] CN=Administrators,CN=Builtin,DC=tech69,DC=local
```
