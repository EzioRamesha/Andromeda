set location=D:\Aragon\Scheduler

call "%location%\SetDate.bat"
ConsoleApp.exe DeleteRiDataBatch -d >> "%location%\%mymonth%\%mydate%\DeleteRiDataBatch_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe ProcessRiDataBatch >> "%location%\%mymonth%\%mydate%\ProcessRiDataBatch_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe FinaliseRiDataBatch >> "%location%\%mymonth%\%mydate%\FinaliseRiDataBatch_%mydate%.output.log.txt" 2>&1
