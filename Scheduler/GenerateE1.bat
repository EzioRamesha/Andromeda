set location=D:\Aragon\Scheduler

call "%location%\SetDate.bat"
ConsoleApp.exe GenerateE1 -d >> "%location%\%mymonth%\%mydate%\GenerateE1_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe GenerateE2 -d >> "%location%\%mymonth%\%mydate%\GenerateE2_%mydate%.output.log.txt" 2>&1
