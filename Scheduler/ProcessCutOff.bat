set location=D:\Aragon\Scheduler

:: CutOff
call "%location%\SetDate.bat"
ConsoleApp.exe ProcessCutOff --process >> "%location%\%mymonth%\%mydate%\ProcessCutOff_%mydate%.output.log.txt" 2>&1
