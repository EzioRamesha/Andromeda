set location=D:\Aragon\Scheduler

:: Direct Retro
call "%location%\SetDate.bat"
ConsoleApp.exe ProcessDirectRetro >> "%location%\%mymonth%\%mydate%\ProcessDirectRetro_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe ProcessRetroRegister >> "%location%\%mymonth%\%mydate%\ProcessRetroRegister_%mydate%.output.log.txt" 2>&1
