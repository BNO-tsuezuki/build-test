@echo off

REM reg query "HKEY_CURRENT_USER\Software\BNO\BNOStarter\EvoGame" /v "Path"
for /f "tokens=1,2*" %%i in ('reg query "HKEY_CURRENT_USER\Software\BNO\BNOStarter\EvoGame" /v "Path"') do ( if "%%i"=="Path" (set installedpath=%%k))

if "%installedpath%"=="" goto NotInstalled

echo %installedpath%

reg add "HKEY_CLASSES_ROOT\devevostart" /v "URL Protocol" /t REG_SZ /d "" /f
if not %ERRORLEVEL% == 0 goto Failed

reg add "HKEY_CLASSES_ROOT\devevostart\shell\open\command" /t REG_SZ /d "\"%installedpath%\devevo\WindowsNoEditor\EvoGame.exe\" -ExecCmdsInit=DisableAllScreenMessages -AuthToken=\"%%1\"" /f
if not %ERRORLEVEL% == 0 goto Failed

echo Setting Complete!
pause
exit /B 0


:NotInstalled
echo No evo's client installed.
pause
exit /B 0

:Failed
echo Setting Failed!
pause
exit /B 0
