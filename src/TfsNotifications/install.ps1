param([boolean]$runTests = $true)

chcp 65001;

sc create "TfsNotifications" binpath= "$($pwd)\TfsNotifications.exe"

cd $cd