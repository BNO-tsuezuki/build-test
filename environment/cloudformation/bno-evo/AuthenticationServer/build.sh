#!/bin/sh

set -eu

cd $SRC_ROOT_PATH
PACK_NAME=`date -u '+%Y%m%d_%H%M%S'`-`git rev-parse --short HEAD`

DST_ROOT_PATH=~/tmp/$PACK_NAME

RDB_CONNECTIONS_JSON=$(cat << EOS
{
    "RdbConnections": {
        "AuthDB":    "server=${DB_WRITE_ENDPOINT}; database=evoauthdb; user=evouser; password=evopassword; MaxPoolSize=100;",
    }
}
EOS
)
KVS_CONNECTIONS_JSON=$(cat << EOS
{
    "KvsConnections": {
        "Common": [
            "${KVS_ENDPOINT}, abortConnect=false,"
        ],
    }
}
EOS
)

echo $RDB_CONNECTIONS_JSON > $SRC_ROOT_PATH/outgame/RdbConnections.json
echo $KVS_CONNECTIONS_JSON > $SRC_ROOT_PATH/outgame/KvsConnections.json

echo Do you want to build packages? [yes or no]
read IS_BUILD
if [ "$IS_BUILD" = "yes" -o "$IS_BUILD" = "y" ]
then
    echo "--------------------------"
    echo "<<<< evo AuthenticationServer packaging >>>>"
    mkdir -p $DST_ROOT_PATH/AuthenticationServer/AuthenticationServer
    
    echo "Copy settings"
    echo $RDB_CONNECTIONS_JSON >                                        $DST_ROOT_PATH/AuthenticationServer/RdbConnections.json
    echo $KVS_CONNECTIONS_JSON >                                        $DST_ROOT_PATH/AuthenticationServer/KvsConnections.json
    
    echo "Build"
    cd $SRC_ROOT_PATH/outgame/AuthenticationServer
    dotnet publish -c Debug -o $DST_ROOT_PATH/AuthenticationServer/AuthenticationServer
fi

echo "------------------------------"
echo "<<<< Migrations Databases >>>>"

echo Do you want to migrations database? [yes or no]
read IS_MIGRATIONS
if [ "$IS_MIGRATIONS" = "yes" -o "$IS_MIGRATIONS" = "y" ]
then
    DB_NAME=evoauthdb

    SQL_PATH=$DST_ROOT_PATH/$DB_NAME.sql

    echo [Please input DataBase Password 2times.]

    EXIST_HISTORYTABLE=`mysql -u evouser -p -e "SHOW TABLES LIKE '__EFMigrationsHistory';" -h $DB_WRITE_ENDPOINT $DB_NAME`

    if [ "$EXIST_HISTORYTABLE" = "" ]
    then
        FROM_MIGRATIONID=0
    else
        FROM_MIGRATIONID=`mysql -u evouser -p -e "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY  MigrationId DESC LIMIT 1;" -h $DB_WRITE_ENDPOINT -N -s $DB_NAME`
    fi

    echo Generate From [$FROM_MIGRATIONID]

    cd $SRC_ROOT_PATH/outgame/AuthenticationServer

    dotnet ef -s AuthenticationServer.csproj migrations script -c AuthDBContext --output $SQL_PATH $FROM_MIGRATIONID # -v
fi

echo "------------------------------"
echo $PACK_NAME build complete!
