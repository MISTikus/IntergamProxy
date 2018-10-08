param([boolean]$runTests = $true)

chcp 65001;

$cd = $pwd.ToString()
if ($cd.Contains("control")) {
    cd ..
}

dotnet restore
dotnet build

if ($runTests){
    ForEach ($folder in (Get-ChildItem -Path test -Directory)) { 
        if ($folder.Name -like '*tests*') {
            dotnet test --no-build ($folder.FullName + '\\\\' + $folder.Name + '.csproj') -p:ParallelizeTestCollections=true -p:ParallelizeAssemblies=true -p:MaxParallelThreads=20 
        }
    }
}

dotnet publish .\src\TfsNotifications\TfsNotifications.csproj -c Release -o "$(pwd)\out" -r win10-x64

cd $cd