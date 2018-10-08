param([boolean]$runTests = $true)

chcp 65001;

sc delete "TfsNotifications"

cd $cd