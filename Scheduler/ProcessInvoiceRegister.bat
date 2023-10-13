set location=D:\Aragon\Scheduler

:: Invoice
call "%location%\SetDate.bat"
ConsoleApp.exe ProcessInvoiceRegister >> "%location%\%mymonth%\%mydate%\ProcessInvoiceRegister_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe UpdateInvoiceRegister >> "%location%\%mymonth%\%mydate%\UpdateInvoiceRegister_%mydate%.output.log.txt" 2>&1
