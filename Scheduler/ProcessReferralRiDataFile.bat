set location=D:\Aragon\Scheduler

call "%location%\SetDate.bat"
ConsoleApp.exe ProcessReferralRiDataFile >> "%location%\%mymonth%\%mydate%\ProcessReferralRiDataFile_%mydate%.output.log.txt" 2>&1
