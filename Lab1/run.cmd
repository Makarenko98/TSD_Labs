csc /target:module /out:"bin/Lib.netmodule" Student.cs Teacher.cs IPerson.cs

al ./bin/Lib.netmodule /out:"bin/Lib.dll" /keyfile:sgKey.snk /delaysign  /version:2.0.0.0

sn -Vr ./bin/Lib.dll sgKey.snk

::sn -R ./bin/Lib.dll sgKey.snk

copy App.exe.config bin

csc /target:exe /r:Lib.dll /out:"bin/App.exe" Program.cs

:: start .\bin\App.exe