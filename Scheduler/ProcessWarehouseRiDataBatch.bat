set location=D:\Aragon\Scheduler

:: RiDataWarehouse
call "%location%\SetDate.bat"
ConsoleApp.exe ProcessWarehouseRiDataBatch >> "%location%\%mymonth%\%mydate%\ProcessWarehouseRiDataBatch_%mydate%.output.log.txt" 2>&1
