echo ##########################
echo # Custom POST-Build Event #
echo ########################## 

SET ProjectDir=%~1
SET ProjectFileName=%~2
SET BuildConfiguration=%~3
cd "%ProjectDir%"
echo %BuildConfiguration%
if "%BuildConfiguration%" == "Release" (
echo %cd%
echo "Creating package ..."
..\.nuget\NuGet.exe pack %ProjectFileName% -Prop Configuration=Release -IncludeReferencedProjects -OutputDirectory "..\SOLIDplate.Packages"
rem ping 127.0.0.1 -n 6 > nul
echo "Creating package COMPLETE."
)