:: echo ##########################
:: echo # Custom PRE-Build Event #
:: echo ########################## 

:: SET ProjectDir=%~1
:: SET ProjectFileName=%~2
:: SET BuildConfiguration=%~3
:: cd "%ProjectDir%"
:: echo %BuildConfiguration%
:: if "%BuildConfiguration%" == "Release" (
:: echo "Updating SOLIDplate.* references..."
:: ..\.nuget\NuGet.exe update packages.config -Verbose -NonInteractive -Verbosity detailed -Safe
:: ping 127.0.0.1 -n 6 > nul
:: ping 127.0.0.1 -n 6 > nul
:: echo "Updating SOLIDplate.* references COMPLETE."
:: )