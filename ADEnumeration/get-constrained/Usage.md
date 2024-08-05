# Usage of get-constrained.exe

## Enumerates objects who have msds-allowedtodelegateto attribute set

```
C:\Users\Administrator\Desktop>get-constrained.exe
Name: CN=WIN2016,OU=Domain Controllers,DC=tech69,DC=local
delegated to: cifs/win2016.tech69.local/tech69.local
delegated to: cifs/win2016.tech69.local
delegated to: cifs/WIN2016
delegated to: cifs/win2016.tech69.local/TECH69
delegated to: cifs/WIN2016/TECH69
TRUSTED_TO_AUTH_FOR_DELEGATION

Name: CN=test2,CN=Users,DC=tech69,DC=local
delegated to: cifs/win2016.tech69.local/tech69.local
delegated to: cifs/win2016.tech69.local
delegated to: cifs/WIN2016
delegated to: cifs/win2016.tech69.local/TECH69
delegated to: cifs/WIN2016/TECH69
TRUSTED_TO_AUTH_FOR_DELEGATION

Name: CN=EHJACEIJHC,CN=Computers,DC=tech69,DC=local
delegated to: cifs/win2016.tech69.local/tech69.local
delegated to: cifs/win2016.tech69.local
delegated to: cifs/WIN2016
delegated to: cifs/win2016.tech69.local/TECH69
delegated to: cifs/WIN2016/TECH69
```
