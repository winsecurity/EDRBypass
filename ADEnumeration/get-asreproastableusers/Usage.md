# Usage of get-asreproastableusers.exe

## Enumreates objects' uac value and check if it has DONT_REQ_PREAUTH 

`get-asreproastableusers.exe -domain tech69.local`

`get-asreproastableusers.exe -server 192.168.0.110`

## Output

```
C:\Users\Administrator\Desktop>get-asreproastableusers.exe
[+] CN=test3,CN=Users,DC=tech69,DC=local

[+] CN=test1,CN=Users,DC=tech69,DC=local

[+] CN=test2,CN=Users,DC=tech69,DC=local


C:\Users\Administrator\Desktop>get-asreproastableusers.exe -server 192.168.0.110
[+] CN=test3,CN=Users,DC=tech69,DC=local

[+] CN=test1,CN=Users,DC=tech69,DC=local

[+] CN=test2,CN=Users,DC=tech69,DC=local
```
