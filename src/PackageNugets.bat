del *.nupkg
.nuget\nuget.exe pack .\Cloud.Storage\Cloud.Storage.csproj
.nuget\nuget.exe pack .\Cloud.Storage.Azure\Cloud.Storage.Azure.csproj
.nuget\nuget.exe push *.nupkg 2538d218-eab8-41ce-8da6-02f3173b4328