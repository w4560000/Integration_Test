#!/bin/sh

# Launch MSSQL and send to background
/opt/mssql/bin/sqlservr &
pid=$!
# Wait for it to be available
echo "Waiting for MS SQL to be available ⏳"

# 1. 確認服務是否啟動
/opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P ${SA_PASSWORD} -Q "SET NOCOUNT ON SELECT N'MS SQL is available 🔥' , @@servername"
is_up=$?
while [ $is_up -ne 0 ] ; do 
    echo -e $(date) 
    /opt/mssql-tools/bin/sqlcmd -l 30 -S localhost -h-1 -V1 -U sa -P ${SA_PASSWORD} -Q "SET NOCOUNT ON SELECT N'MS SQL is available 🔥' , @@servername"
    is_up=$?
    sleep 5 
done

# 2. 執行預設 Script - 建立 Database
for foo in /MSSQL/CreateDatabase/*.sql
do 
    echo "Ready to execute ${foo}";
    /opt/mssql-tools/bin/sqlcmd -U sa -P ${SA_PASSWORD} -l 30 -e -i ${foo}
    echo "Execute ${foo} completed successfully 🌳";
done

# 3. 執行預設 Script - 建立 Table
for foo in /MSSQL/CreateTable/*.sql
do 
    echo "Ready to execute ${foo}";
    /opt/mssql-tools/bin/sqlcmd -U sa -P ${SA_PASSWORD} -l 30 -e -i ${foo}
    echo "Execute ${foo} completed successfully 🌳";
done

# 4. 執行預設 Script - 建立 SP
for foo in /MSSQL/SP/*.sql
do 
    echo "Ready to execute ${foo}";
    /opt/mssql-tools/bin/sqlcmd -U sa -P ${SA_PASSWORD} -l 30 -e -i ${foo}
    echo "Execute ${foo} completed successfully 🌳";
done

# Wait on the sqlserver process
wait $pid