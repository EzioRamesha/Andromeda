set location=D:\Aragon\Scheduler

call "%location%\SetDate.bat"
ConsoleApp.exe ProcessCutOff >> "%location%\%mymonth%\%mydate%\ProcessCutOffMonthly_%mydate%.output.log.txt" 2>&1
