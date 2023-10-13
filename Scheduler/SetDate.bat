set location=D:\Aragon\Scheduler

for /f "tokens=2-4 delims=/ " %%a in ('date /t') do (
	set mydate=%%c%%a%%b
	set mymonth=%%c%%a
)

if not exist "%location%\%mymonth%" mkdir "%location%\%mymonth%"
if not exist "%location%\%mymonth%\%mydate%" mkdir "%location%\%mymonth%\%mydate%"

C:
cd "C:\Aragon\ConsoleApp"
