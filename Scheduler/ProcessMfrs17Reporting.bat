set location=D:\Aragon\Scheduler

::MFRS 17 Report
call "%location%\SetDate.bat"
ConsoleApp.exe ProcessMfrs17Reporting >> "%location%\%mymonth%\%mydate%\ProcessMfrs17Reporting_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe GenerateMfrs17Reporting >> "%location%\%mymonth%\%mydate%\GenerateMfrs17Reporting_%mydate%.output.log.txt" 2>&1
