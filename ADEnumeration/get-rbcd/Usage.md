# Usage of get-rbcd.exe

## Enumerates computer objects' aces and print any normal user having write or genericall access.

`get-rbcd.exe`

`get-rbcd.exe -domain tech69.local`

`get-rbcd.exe -server 192.168.0.110`

`get-rbcd.exe -username test2 -password P@ssw0rd`

## Output

```
 .\get-rbcd.exe
Computer: CN=WIN2016
User: TECH69\test2
ListChildren, ReadProperty, GenericWrite,ReadProperty, WriteProperty, GenericExecute

Computer: CN=NIKKI-PC
User: TECH69\test1
WriteProperty,ReadProperty, WriteProperty, GenericExecute
User: TECH69\test2
WriteProperty,ReadProperty, WriteProperty, GenericExecute

Computer: CN=DBCJICGHBF
User: TECH69\test2
WriteProperty,Self, WriteProperty,GenericAll,ReadProperty, WriteProperty, GenericExecute

Computer: CN=EHJACEIJHC
User: TECH69\test2
WriteProperty,Self, WriteProperty,GenericAll,ReadProperty, WriteProperty, GenericExecute

Computer: CN=legacycomputer
User: TECH69\test1
GenericAll,WriteProperty,Self, WriteProperty,ReadProperty, WriteProperty, GenericExecute
User: TECH69\test2
GenericAll,WriteProperty,Self, WriteProperty,ReadProperty, WriteProperty, GenericExecute
```
