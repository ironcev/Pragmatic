FOR /F "tokens=*" %%G IN ('dir /b Artifacts\Nupkg\Release\*.nupkg') DO CALL "..\Tools\nuget.exe" push Artifacts\Nupkg\Release\%%G
