@echo off
setlocal ENABLEEXTENSIONS

REM Set default
set DB_PORT=1433

REM Parse key=value arguments
:parse
if "%~1"=="" goto run
for /f "tokens=1,2 delims==" %%A in ("%~1") do (
    if "%%A"=="db_user" set DB_USER=%%B
    if "%%A"=="db_password" set DB_PASSWORD=%%B
    if "%%A"=="db_name" set DB_NAME=%%B
    if "%%A"=="app_port" set APP_PORT=%%B
)
shift
goto parse

:run
if not defined DB_USER goto help
if not defined DB_PASSWORD goto help
if not defined DB_NAME goto help
if not defined APP_PORT goto help

docker-compose up --build
goto :eof

:help
echo Usage: run.bat db_user=sa db_password=pass db_name=FilesDb app_port=5201
exit /b 1
