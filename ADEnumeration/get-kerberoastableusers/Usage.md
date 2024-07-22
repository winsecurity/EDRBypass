# Usage for get-kerberoastableusers

## Description: This binary can be used to retrieve all kerberoastable accounts (accounts with spn set)

## Usage

`get-kerberoastableusers.exe`

`get-kerberoastableusers.exe  -server 192.168.0.110`

`get-kerberoastableusers.exe  -domain tech69.local`

## Output
```
C:\Users\Administrator\Desktop>get-kerberoastableusers.exe  -server 192.168.0.110
Samaccountname: WIN2016$
spn: TERMSRV/WIN2016
spn: TERMSRV/win2016.tech69.local
```
