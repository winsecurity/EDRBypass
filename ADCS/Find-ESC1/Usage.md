# Usage of Find-ESC1.exe

## This retrieves all the certificate templates that are vulnerable to ESC1 configuration
- User can supply the subjectname
- Manager approval not needed
- Number of authorized signatures required are zero
- Template is used for clientauthentication/serverauthentication/smartcardlogon or any authentication
- Low privileged user can enroll in these certificate templates

`Find-ESC1.exe`

`Find-ESC1.exe -server 192.168.0.110`

`Find-ESC1.exe -domain tech69.local`

## Output

```
PS C:\Users\nagas\source\repos\get-ca\bin\Release> .\convertfrom-sid.exe  -server 192.168.0.110 -username test1 -password TestP@ssw0rd
Template Name: User_Copy
Manager approval needed: false
Number of Authorized signatures required: 0
mspki-Certificate-Name-Flag: CT_FLAG_ENROLLEE_SUPPLIES_SUBJECT
mspki-Enrollment-Flag: CT_FLAG_INCLUDE_SYMMETRIC_ALGORITHMS, DsPublish-CT_FLAG_PUBLISH_TO_DS
Enrollment rights: S-1-5-21-2846196120-3918715802-2505943323-513
pKI-Extended-KeyUsage: client authentication
pKI-Extended-KeyUsage:
pKI-Extended-KeyUsage: encrypting file system
-------------------------------------------------
Template Name: ESC1-Template
Manager approval needed: false
Number of Authorized signatures required: 0
mspki-Certificate-Name-Flag: CT_FLAG_ENROLLEE_SUPPLIES_SUBJECT
mspki-Enrollment-Flag: CT_FLAG_INCLUDE_SYMMETRIC_ALGORITHMS, DsPublish-CT_FLAG_PUBLISH_TO_DS
Enrollment rights: S-1-5-21-2846196120-3918715802-2505943323-513
pKI-Extended-KeyUsage: client authentication
pKI-Extended-KeyUsage:
pKI-Extended-KeyUsage: encrypting file system
-------------------------------------------------
```
