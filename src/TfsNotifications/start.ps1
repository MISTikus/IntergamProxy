param([boolean]$runTests = $true)

chcp 65001;

sc start "TfsNotifications"

cd $cd