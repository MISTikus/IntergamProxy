param([boolean]$runTests = $true)

chcp 65001;

sc stop "TfsNotifications"

cd $cd