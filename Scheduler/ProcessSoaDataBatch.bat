set location=D:\Aragon\Scheduler

:: SoaData
call "%location%\SetDate.bat"
ConsoleApp.exe ProcessSoaDataBatch >> "%location%\%mymonth%\%mydate%\ProcessSoaDataBatch_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe SummarySoaDataBatch >> "%location%\%mymonth%\%mydate%\SummarySoaDataBatch_%mydate%.output.log.txt" 2>&1
