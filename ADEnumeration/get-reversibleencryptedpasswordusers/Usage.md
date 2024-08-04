# Usage of get-reversibleencryptedpasswordusers.exe

## Enumerates uac values of objects and looks for ENCRYPTED_TEXT_PWD_ALLOWED


`get-reversibleencryptedpasswordusers.exe` 

`get-reversibleencryptedpasswordusers.exe -server 192.168.0.110`

`get-reversibleencryptedpasswordusers.exe -domain tech69.local`

## Output

```
C:\Users\Administrator\Desktop>get-reversibleencryptedpasswordusers.exe
[+] CN=test2,CN=Users,DC=tech69,DC=local


C:\Users\Administrator\Desktop>get-reversibleencryptedpasswordusers.exe -server 192.168.0.110
[+] CN=test2,CN=Users,DC=tech69,DC=local
```

