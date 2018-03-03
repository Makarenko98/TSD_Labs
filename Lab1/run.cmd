csc /target:library /out:"bin/Lib/Lib.dll" Student.cs Teacher.cs IPerson.cs LibAssemblyInfo.cs /keyfile:sgPublicKey.snk /delaysign

:: sn -Vr ./bin/Lib/Lib.dll sgKey.snk

:: sn -R ./bin/Lib/Lib.dll sgKey.snk

copy App.exe.config bin

csc /target:exe /r:./bin/Lib.dll /out:"bin/App.exe" Program.cs

start .\bin\App.exe