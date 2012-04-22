@ECHO OFF

powershell %~dp0\package.ps1

IF NOT ERRORLEVEL 0 EXIT /B %ERRORLEVEL%