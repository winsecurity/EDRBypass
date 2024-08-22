# Usage of Find-ESC2.exe

## this binary can be used to find certificate templates that are vulnerable to ESC2 configuration
## ESC2 Misconfiguration
- Number of authorized signatured required: 0
- Manage Approval needed: false
- Must have AnyEKU or NoEKU
- Low privileged users must have enroll permissions

`\Find-ESC2.exe -server 192.168.0.110 -username test1 -password P@ssw0rd`

`find-esc2.exe`

`find-esc2.exe -domain tech69.local`

## Output

```
C:\Users\Administrator\Desktop>Find-ESC2.exe
Template Name: User_AnyEKU
Who can enroll: TECH69\Domain Users
```

```
PS C:\Users\nagas\source\repos\Find-ESC2\bin\Release> .\Find-ESC2.exe -server 192.168.0.110 -username test1 -password P@ssw0rd
Template Name: User_AnyEKU
Who can enroll: S-1-5-21-2846196120-3918715802-2505943323-513
```


