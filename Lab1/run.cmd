csc /target:library /out:"bin/Lib.dll" Student.cs Teacher.cs IPerson.cs LibAssemblyInfo.cs /keyfile:sgPublicKey.snk /delaysign

::al ./bin/Lib.netmodule /out:"bin/Lib.dll" /keyfile:sgPublicKey.snk /delaysign  /version:2.0.0.0

sn -Vr ./bin/Lib.dll sgKey.snk

::sn -R ./bin/Lib.dll sgKey.snk

copy App.exe.config bin

csc /target:exe /r:Lib.dll /out:"bin/App.exe" Program.cs

:: start .\bin\App.exe