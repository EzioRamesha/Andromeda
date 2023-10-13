set location=D:\Aragon\Scheduler

::call "%location%\SetDate.bat"
::ConsoleApp.exe SendInactiveUserReport >> "%location%\%mymonth%\%mydate%\SendInactiveUserReport_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe SuspendInactiveUser >> "%location%\%mymonth%\%mydate%\SuspendInactiveUser_%mydate%.output.log.txt" 2>&1
