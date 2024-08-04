# Usage of get-unconstrained.exe

## Enumerates all accounts with useraccountcontrol value having the value TRUSTED_FOR_DELEGATION

`get-unconstrained.exe`

`get-unconstrained.exe -domain tech69.local`

`get-unconstrained.exe -server 192.168.0.110`

## Output 

```
convertfrom-sid.exe -server 192.168.0.110
CN=DBCJICGHBF,CN=Computers,DC=tech69,DC=local
CN=legacycomputer,CN=Computers,DC=tech69,DC=local
```
