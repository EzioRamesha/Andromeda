set location=D:\Aragon\Scheduler

call "%location%\SetDate.bat"
ConsoleApp.exe GenerateExportData >> "%location%\%mymonth%\%mydate%\GenerateExportData_%mydate%.output.log.txt" 2>&1
