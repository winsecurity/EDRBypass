# Usage of get-groupmembersrecurse

## Get the group members recursively and also nested groups as well

## Usage
`get-groupmembersrecurse.exe -server 192.168.0.110 -groupname "Domain Admins"`

`get-groupmembersrecurse.exe -server 192.168.0.110 -groupname "group1" -username test2 -password P@ssw0rd`

## Output
```
C:\Users\Administrator\Desktop>get-groupmembersrecurse.exe -server 192.168.0.110 -groupname "Remote Desktop Users"
User: test3 is memberOf Remote Desktop Users
User: test2 is memberOf Remote Desktop Users
```

---
```
C:\Users\Administrator\Desktop>get-groupmembersrecurse.exe -server 192.168.0.110 -groupname "group1"
User: test2 is memberOf group1
Group: group2 is memberOf group1
User: test1 is memberOf group2
Group: group3 is memberOf group2
```
