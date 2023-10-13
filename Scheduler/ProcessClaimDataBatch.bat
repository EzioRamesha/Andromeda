set location=D:\Aragon\Scheduler

:: Claim
call "%location%\SetDate.bat"
ConsoleApp.exe ProcessClaimDataBatch >> "%location%\%mymonth%\%mydate%\ProcessClaimDataBatch_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe ReportingClaimDataBatch >> "%location%\%mymonth%\%mydate%\ReportingClaimDataBatch_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe ProcessClaimRegisterBatch >> "%location%\%mymonth%\%mydate%\ProcessClaimRegisterBatch_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe ProcessReferralClaimAssessmentBatch >> "%location%\%mymonth%\%mydate%\ProcessReferralClaimAssessmentBatch_%mydate%.output.log.txt" 2>&1

call "%location%\SetDate.bat"
ConsoleApp.exe ReprocessProvisionClaimRegisterBatch >> "%location%\%mymonth%\%mydate%\ReprocessProvisionClaimRegisterBatch_%mydate%.output.log.txt" 2>&1
