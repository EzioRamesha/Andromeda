set location=D:\Aragon\Scheduler

:: Provision
call "%location%\SetDate.bat"
ConsoleApp.exe ProvisionClaimRegisterBatch >> "%location%\%mymonth%\%mydate%\ProvisionClaimRegisterBatch_%mydate%.output.log.txt" 2>&1
