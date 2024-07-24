# Usage of get-pre2000computers script

## This tool can be used to fetch computers that are configured as pre-windows 2000, by default they have their computername in lowercase without $ as password

`get-pre2000computers.exe`

`get-pre2000computers.exe -domain tech69.local -server 192.168.0.110`

`get-pre2000computers.exe -domain tech69.local`

`get-pre2000computers.exe -domain tech69.local -server 192.168.0.110 -username test2 -password P@ssw0rd`

## Output 
```
C:\Users\Administrator\Desktop>get-pre2000computers.exe -domain tech69.local -server 192.168.0.110
Samaccountname: LEGACYCOMPUTER$
Distinguishedname: CN=legacycomputer,CN=Computers,DC=tech69,DC=local
```
