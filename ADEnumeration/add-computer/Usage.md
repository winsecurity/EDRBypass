# Usage of add-computer.exe

## Adds a new computer object to CN=Computers by default or to specified dn with the specified password

` .\add-computer.exe -computername testing -computerpassword P@ssw0rd  -server 192.168.0.110`

`.\add-computer.exe -computername testing -computerpassword P@ssw0rd  -dn "CN=Computers,DC=TECH69,DC=LOCAL"`

`.\add-computer.exe -computername testing -computerpassword P@ssw0rd  -server 192.168.0.110 -domain tech69.local`

## Output

```
 .\add-computer.exe -computername testing -computerpassword P@s
sw0rd  -server 192.168.0.110
LDAP://192.168.0.110/CN=Computers,DC=tech69,DC=local
Successfully added new computer to CN=Computers
```
