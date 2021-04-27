#!/bin/sh

set -eu

cd $SRC_ROOT_PATH
PACK_NAME=`date -u '+%Y%m%d_%H%M%S'`-`git rev-parse --short HEAD`

GIT_COMMITTER_DATE=`git log -1 --format='%cd' --date=format:'%y%m%d-%H%M%S'`' ('`git rev-parse --short HEAD`')'

DST_ROOT_PATH=~/tmp/$PACK_NAME


aws configure set region "$REGION"
MATCHINGSERVERS_ADDRLIST=\
`
    aws cloudformation list-exports |
    jq '.Exports[] | select(.Name | startswith($key)) | .Value' --arg key "jpop-evomatching-ip-${ENV_NAME}-areaIndex"
`
MATCHINGSERVERS_PAYLOAD=""
for addr in ${MATCHINGSERVERS_ADDRLIST[@]}
do
    MATCHINGSERVERS_PAYLOAD+=$(cat << EOS
    {
        "addr": ${addr},
        "port": "",
    },
EOS
)
done

RDB_CONNECTIONS_JSON=$(cat << EOS
{
    "RdbConnections": {
        "GameDB":       "server=${DB_WRITE_ENDPOINT}; database=evogamedb; user=evouser; password=evopassword; MaxPoolSize=20;",
        "AccountDB":    "server=${DB_WRITE_ENDPOINT}; database=evoaccountdb; user=evouser; password=evopassword; MaxPoolSize=20;",

        "Personal": [
            "server=${DB_WRITE_ENDPOINT}; database=evopersonal001; user=evouser; password=evopassword; MaxPoolSize=20;",
            "server=${DB_WRITE_ENDPOINT}; database=evopersonal002; user=evouser; password=evopassword; MaxPoolSize=20;",
            "server=${DB_WRITE_ENDPOINT}; database=evopersonal003; user=evouser; password=evopassword; MaxPoolSize=20;",
            "server=${DB_WRITE_ENDPOINT}; database=evopersonal004; user=evouser; password=evopassword; MaxPoolSize=20;",
            "server=${DB_WRITE_ENDPOINT}; database=evopersonal005; user=evouser; password=evopassword; MaxPoolSize=20;",
            "server=${DB_WRITE_ENDPOINT}; database=evopersonal006; user=evouser; password=evopassword; MaxPoolSize=20;",
            "server=${DB_WRITE_ENDPOINT}; database=evopersonal007; user=evouser; password=evopassword; MaxPoolSize=20;",
            "server=${DB_WRITE_ENDPOINT}; database=evopersonal008; user=evouser; password=evopassword; MaxPoolSize=20;"
        ],

        "Common1": "server=${DB_WRITE_ENDPOINT}; database=evocommon1; user=evouser; password=evopassword; MaxPoolSize=20;",
        "Common2": "server=${DB_WRITE_ENDPOINT}; database=evocommon2; user=evouser; password=evopassword; MaxPoolSize=20;",
        "Common3": "server=${DB_WRITE_ENDPOINT}; database=evocommon3; user=evouser; password=evopassword; MaxPoolSize=20;",

        "GMTool": "server=${DB_WRITE_ENDPOINT}; database=evogmtool; user=evouser; password=evopassword; MaxPoolSize=20;",

        "GameLog": "server=${DB_WRITE_ENDPOINT}; database=evogamelog; user=evouser; password=evopassword; MaxPoolSize=20;"
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
        "Personal": [
            "${KVS_ENDPOINT}, abortConnect=false,"
        ],
        "Tool": [
            "${KVS_ENDPOINT}, abortConnect=false,"
        ]
    }
}
EOS
)
MATCHINGSERVER_JSON=$(cat << EOS
{
    "MatchingServers": [
        ${MATCHINGSERVERS_PAYLOAD}
    ]
}
EOS
)
SEQUENCINGSERVER_JSON=$(cat << EOS
{
    "SequencingServer": {
        "addr": "${SEQUENCINGSERVER_IP}",
        "port": "${SEQUENCINGSERVER_PORT}",
    }
}
EOS
)
TOOLSERVER_JSON=$(cat << EOS
{
    "ToolServer": {
        "addr": "${TOOLSERVER_IP}",
        "port": "${TOOLSERVER_PORT}",
    }
}
EOS
)
DELIVERYDATA_JSON=$(cat << EOS
{
    "DeliveryData": {
        "S3BucketName": "${DELIVERYDATA_BUCKETNAME}",
        "S3BucketRegion": "${DELIVERYDATA_BUCKETREGION}",
        "CfDomainName": "${DELIVERYDATA_CFDOMAINNAME}",
    }
}
EOS
)
SYSTEMINFO_JSON=$(cat << EOS
{
    "git": {
        "committer_date": "${GIT_COMMITTER_DATE}",
    },

    "inky": {
        "url": "https://stg-api.bnea.io",
        "apikey_name": "X-BNEA-Api-Id",
        "apikey_value": "eJdBnRCPQn3DqxjvUCvE5h8c5Q9AxK79",
        "apisecretkey_name": "X-BNEA-Api-Secret",
        "apisecretkey_value": "kEJj7rDk5n72bLm9W2P3TEHfZx2yyqDB"
    },

    "battleserver": {
        "password": "lq8lG5SwQmZGCaCl2wYKV4AaWntTN6vo"
    }
}
EOS
)


AtExit() {
  [[ -n ${TMP_DBCONF_FILE-} ]] && rm -f "$TMP_DBCONF_FILE"
}
trap AtExit EXIT
trap 'rc=$?; trap - EXIT; AtExit; exit $?' INT PIPE TERM


TMP_DBCONF_FILE=$(mktemp "/tmp/${0##*/}.tmp.XXXXXX")

CURRENT_MIGRATION_ID=""
CurrentMigrationId(){

    local DB_NAME=$1

    CURRENT_MIGRATION_ID="error"

    EXIST_DATABASE=`mysql --defaults-extra-file=$TMP_DBCONF_FILE -e "SHOW DATABASES LIKE '$DB_NAME';" -h $DB_WRITE_ENDPOINT`
    if [ "$EXIST_DATABASE" = "" ]
    then
        CURRENT_MIGRATION_ID=0
    else
        EXIST_HISTORYTABLE=`mysql --defaults-extra-file=$TMP_DBCONF_FILE -e "SHOW TABLES LIKE '__EFMigrationsHistory';" -h $DB_WRITE_ENDPOINT $DB_NAME`
        if [ "$EXIST_HISTORYTABLE" = "" ]
        then
            CURRENT_MIGRATION_ID=0
        else
            CURRENT_MIGRATION_ID=`mysql --defaults-extra-file=$TMP_DBCONF_FILE -e "SELECT MigrationId FROM __EFMigrationsHistory ORDER BY  MigrationId DESC LIMIT 1;" -h $DB_WRITE_ENDPOINT -N -s $DB_NAME`
        fi
    fi
}

ShowCurrentMigrationId(){
    
    local DB_NAME=$1
    CurrentMigrationId $DB_NAME
    echo "$DB_NAME : $CURRENT_MIGRATION_ID"
}

GenerateMigratinSql(){

    local DB_NAME=$1
    local DB_CONTEXT=$2
    local START_PROJ=$3

    CurrentMigrationId $DB_NAME

    echo "----"
    echo "Enter target migration id for <$DB_NAME>"
    echo "(If the input is empty, it will be up to date !)"
    read local TARGET_MIGRATION_ID
    if [ "$TARGET_MIGRATION_ID" = "" ]
    then
        echo "Generate From [$CURRENT_MIGRATION_ID] To Latest."
    else
        echo "Generate From [$CURRENT_MIGRATION_ID] To [$TARGET_MIGRATION_ID]."
    fi

    local SQL_FILE=$DB_NAME.sql
    local SQL_PATH=$DST_ROOT_PATH/$SQL_FILE

    cd $SRC_ROOT_PATH/outgame/$START_PROJ

    dotnet ef -s $START_PROJ.csproj migrations script -c $DB_CONTEXT --output $SQL_PATH.tmp $CURRENT_MIGRATION_ID $TARGET_MIGRATION_ID # -v

    sed -e '1s/^\xef\xbb\xbf//' $SQL_PATH.tmp | sed -e "1iUSE ${DB_NAME};\n" | sed -e "1iCREATE DATABASE IF NOT EXISTS ${DB_NAME};\n" > $SQL_PATH

    rm $SQL_PATH.tmp

    aws s3 cp  $SQL_PATH s3://$BUILDPACKAGES_BUCKETNAME/$PACK_NAME/$SQL_FILE
}



echo $RDB_CONNECTIONS_JSON > $SRC_ROOT_PATH/outgame/RdbConnections.json
echo $KVS_CONNECTIONS_JSON > $SRC_ROOT_PATH/outgame/KvsConnections.json

echo Do you want to build packages? [yes or no]
read IS_BUILD
if [ "$IS_BUILD" = "yes" -o "$IS_BUILD" = "y" ]
then
    echo "--------------------------"
    echo "<<<< evoapi packaging >>>>"
    mkdir -p $DST_ROOT_PATH/evoapi/evoapi
    
    echo "Copy settings"
    cp $SRC_ROOT_PATH/outgame/appsettings.DevEnv.json                   $DST_ROOT_PATH/evoapi/
    cp $SRC_ROOT_PATH/outgame/appsettings.json                          $DST_ROOT_PATH/evoapi/
    cp $SRC_ROOT_PATH/environment/codedeploy/appspec/evoapi.appspec.yml $DST_ROOT_PATH/evoapi/appspec.yml
    cp $SRC_ROOT_PATH/environment/codedeploy/service/evoapi.service     $DST_ROOT_PATH/evoapi/
    cp -r $SRC_ROOT_PATH/environment/codedeploy/scripts                 $DST_ROOT_PATH/evoapi/
    echo $RDB_CONNECTIONS_JSON >                                        $DST_ROOT_PATH/evoapi/RdbConnections.json
    echo $KVS_CONNECTIONS_JSON >                                        $DST_ROOT_PATH/evoapi/KvsConnections.json
    echo $MATCHINGSERVER_JSON >                                         $DST_ROOT_PATH/evoapi/MatchingServer.json
    echo $SEQUENCINGSERVER_JSON >                                       $DST_ROOT_PATH/evoapi/SequencingServer.json
    cp $SRC_ROOT_PATH/outgame/AuthenticationServer.json                 $DST_ROOT_PATH/evoapi/
    echo $DELIVERYDATA_JSON >                                           $DST_ROOT_PATH/evoapi/DeliveryData.json
    echo $SYSTEMINFO_JSON >                                             $DST_ROOT_PATH/evoapi/systeminfo.json

    
    echo "Build"
    cd $SRC_ROOT_PATH/outgame/evoapi
    dotnet publish -c Debug -o $DST_ROOT_PATH/evoapi/evoapi
    
    echo "Archive"
    tar cvzf $DST_ROOT_PATH/evoapi.tar.gz -C $DST_ROOT_PATH/evoapi .
    
    echo "Upload to s3"
    aws s3 cp  $DST_ROOT_PATH/evoapi.tar.gz s3://$BUILDPACKAGES_BUCKETNAME/$PACK_NAME/evoapi.tar.gz



    echo "-------------------------------"
    echo "<<<< evomatching packaging >>>>"
    mkdir -p $DST_ROOT_PATH/evomatching/evomatching

    echo "Copy settings"
    cp $SRC_ROOT_PATH/outgame/appsettings.DevEnv.json                           $DST_ROOT_PATH/evomatching/
    cp $SRC_ROOT_PATH/outgame/appsettings.json                                  $DST_ROOT_PATH/evomatching/
    cp $SRC_ROOT_PATH/environment/codedeploy/appspec/evomatching.appspec.yml    $DST_ROOT_PATH/evomatching/appspec.yml
    cp $SRC_ROOT_PATH/environment/codedeploy/service/evomatching.service        $DST_ROOT_PATH/evomatching/
    cp -r $SRC_ROOT_PATH/environment/codedeploy/scripts                         $DST_ROOT_PATH/evomatching/
    echo $RDB_CONNECTIONS_JSON >                                                $DST_ROOT_PATH/evomatching/RdbConnections.json
    echo $KVS_CONNECTIONS_JSON >                                                $DST_ROOT_PATH/evomatching/KvsConnections.json
    echo $MATCHINGSERVER_JSON >                                                 $DST_ROOT_PATH/evomatching/MatchingServer.json
    echo $SEQUENCINGSERVER_JSON >                                               $DST_ROOT_PATH/evomatching/SequencingServer.json
    cp $SRC_ROOT_PATH/outgame/AuthenticationServer.json                         $DST_ROOT_PATH/evomatching/
    echo $DELIVERYDATA_JSON >                                                   $DST_ROOT_PATH/evomatching/DeliveryData.json

    
    echo "Build"
    cd $SRC_ROOT_PATH/outgame/evomatching
    dotnet publish -c Debug -o $DST_ROOT_PATH/evomatching/evomatching
    
    echo "Archive"
    tar cvzf $DST_ROOT_PATH/evomatching.tar.gz -C $DST_ROOT_PATH/evomatching .
    
    echo "Upload to s3"
    aws s3 cp  $DST_ROOT_PATH/evomatching.tar.gz s3://$BUILDPACKAGES_BUCKETNAME/$PACK_NAME/evomatching.tar.gz


    echo "---------------------------------"
    echo "<<<< evosequencing packaging >>>>"
    mkdir -p $DST_ROOT_PATH/evosequencing/evosequencing
    
    echo "Copy settings"
    cp $SRC_ROOT_PATH/outgame/appsettings.DevEnv.json                           $DST_ROOT_PATH/evosequencing/
    cp $SRC_ROOT_PATH/outgame/appsettings.json                                  $DST_ROOT_PATH/evosequencing/
    cp $SRC_ROOT_PATH/environment/codedeploy/appspec/evosequencing.appspec.yml  $DST_ROOT_PATH/evosequencing/appspec.yml
    cp $SRC_ROOT_PATH/environment/codedeploy/service/evosequencing.service      $DST_ROOT_PATH/evosequencing/
    cp -r $SRC_ROOT_PATH/environment/codedeploy/scripts                         $DST_ROOT_PATH/evosequencing/
    echo $RDB_CONNECTIONS_JSON >                                                $DST_ROOT_PATH/evosequencing/RdbConnections.json
    echo $KVS_CONNECTIONS_JSON >                                                $DST_ROOT_PATH/evosequencing/KvsConnections.json
    echo $MATCHINGSERVER_JSON >                                                 $DST_ROOT_PATH/evosequencing/MatchingServer.json
    echo $SEQUENCINGSERVER_JSON >                                               $DST_ROOT_PATH/evosequencing/SequencingServer.json
    cp $SRC_ROOT_PATH/outgame/AuthenticationServer.json                         $DST_ROOT_PATH/evosequencing/
    echo $DELIVERYDATA_JSON >                                                   $DST_ROOT_PATH/evosequencing/DeliveryData.json

    
    echo "Build"
    cd $SRC_ROOT_PATH/outgame/evosequencing
    dotnet publish -c Debug -o $DST_ROOT_PATH/evosequencing/evosequencing
    
    echo "Archive"
    tar cvzf $DST_ROOT_PATH/evosequencing.tar.gz -C $DST_ROOT_PATH/evosequencing .
    
    echo "Upload to s3"
    aws s3 cp  $DST_ROOT_PATH/evosequencing.tar.gz s3://$BUILDPACKAGES_BUCKETNAME/$PACK_NAME/evosequencing.tar.gz


    echo "---------------------------"
    echo "<<<< evotool packaging >>>>"
    mkdir -p $DST_ROOT_PATH/evotool/evotool

    echo "Copy settings"
    cp $SRC_ROOT_PATH/outgame/appsettings.DevEnv.json                       $DST_ROOT_PATH/evotool/
    cp $SRC_ROOT_PATH/outgame/appsettings.json                              $DST_ROOT_PATH/evotool/
    cp $SRC_ROOT_PATH/environment/codedeploy/appspec/evotool.appspec.yml    $DST_ROOT_PATH/evotool/appspec.yml
    cp $SRC_ROOT_PATH/environment/codedeploy/service/evotool.service        $DST_ROOT_PATH/evotool/
    cp -r $SRC_ROOT_PATH/environment/codedeploy/scripts                     $DST_ROOT_PATH/evotool/
    echo $RDB_CONNECTIONS_JSON >                                            $DST_ROOT_PATH/evotool/RdbConnections.json
    echo $KVS_CONNECTIONS_JSON >                                            $DST_ROOT_PATH/evotool/KvsConnections.json
    echo $MATCHINGSERVER_JSON >                                             $DST_ROOT_PATH/evotool/MatchingServer.json
    echo $SEQUENCINGSERVER_JSON >                                           $DST_ROOT_PATH/evotool/SequencingServer.json
    cp $SRC_ROOT_PATH/outgame/AuthenticationServer.json                     $DST_ROOT_PATH/evotool/
    echo $DELIVERYDATA_JSON >                                               $DST_ROOT_PATH/evotool/DeliveryData.json


    echo "Build"
    cd $SRC_ROOT_PATH/outgame/evotool
    npm ci --no-bin-links # --no-bin-links
    dotnet publish -c Debug -o $DST_ROOT_PATH/evotool/evotool

    echo "Archive"
    tar cvzf $DST_ROOT_PATH/evotool.tar.gz -C $DST_ROOT_PATH/evotool .

    echo "Upload to s3"
    aws s3 cp  $DST_ROOT_PATH/evotool.tar.gz s3://$BUILDPACKAGES_BUCKETNAME/$PACK_NAME/evotool.tar.gz


    echo "---------------------------"
    echo "<<<< evogmtool packaging >>>>"
    mkdir -p $DST_ROOT_PATH/evogmtool/evogmtool

    echo "Copy settings"
    cp $SRC_ROOT_PATH/outgame/appsettings.DevEnv.json                       $DST_ROOT_PATH/evogmtool/
    cp $SRC_ROOT_PATH/outgame/appsettings.json                              $DST_ROOT_PATH/evogmtool/
    cp $SRC_ROOT_PATH/environment/codedeploy/appspec/evogmtool.appspec.yml  $DST_ROOT_PATH/evogmtool/appspec.yml
    cp $SRC_ROOT_PATH/environment/codedeploy/service/evogmtool.service      $DST_ROOT_PATH/evogmtool/
    cp -r $SRC_ROOT_PATH/environment/codedeploy/scripts                     $DST_ROOT_PATH/evogmtool/
    echo $RDB_CONNECTIONS_JSON >                                            $DST_ROOT_PATH/evogmtool/RdbConnections.json
    echo $KVS_CONNECTIONS_JSON >                                            $DST_ROOT_PATH/evogmtool/KvsConnections.json
    echo $MATCHINGSERVER_JSON >                                             $DST_ROOT_PATH/evogmtool/MatchingServer.json
    echo $SEQUENCINGSERVER_JSON >                                           $DST_ROOT_PATH/evogmtool/SequencingServer.json
    echo $TOOLSERVER_JSON >                                                 $DST_ROOT_PATH/evogmtool/ToolServer.json
    cp $SRC_ROOT_PATH/outgame/AuthenticationServer.json                     $DST_ROOT_PATH/evogmtool/
    echo $DELIVERYDATA_JSON >                                               $DST_ROOT_PATH/evogmtool/DeliveryData.json


    echo "Build"
    cd $SRC_ROOT_PATH/outgame/evogmtool
    dotnet publish -c Debug -o $DST_ROOT_PATH/evogmtool/evogmtool

    echo "Archive"
    tar cvzf $DST_ROOT_PATH/evogmtool.tar.gz -C $DST_ROOT_PATH/evogmtool .

    echo "Upload to s3"
    aws s3 cp  $DST_ROOT_PATH/evogmtool.tar.gz s3://$BUILDPACKAGES_BUCKETNAME/$PACK_NAME/evogmtool.tar.gz
fi

echo "------------------------------"
echo "<<<< Generate Migration SQL >>>>"
read -sp "Enter database password:" DB_PASS
echo 

echo "[Client]" > $TMP_DBCONF_FILE
echo "user = evouser" >>  $TMP_DBCONF_FILE
echo "password = $DB_PASS" >> $TMP_DBCONF_FILE

echo ---- Current Migration Id ----
ShowCurrentMigrationId evopersonal001
ShowCurrentMigrationId evopersonal002
ShowCurrentMigrationId evopersonal003
ShowCurrentMigrationId evopersonal004
ShowCurrentMigrationId evopersonal005
ShowCurrentMigrationId evopersonal006
ShowCurrentMigrationId evopersonal007
ShowCurrentMigrationId evopersonal008
ShowCurrentMigrationId evocommon1
ShowCurrentMigrationId evocommon2
ShowCurrentMigrationId evocommon3
ShowCurrentMigrationId evogmtool

echo Do you want to generate migrationSql? [yes or no]
read IS_GENERATE
if [ "$IS_GENERATE" = "yes" -o "$IS_GENERATE" = "y" ]
then
    
    GenerateMigratinSql evocommon1 Common1DBContext evoapi
    GenerateMigratinSql evocommon2 Common2DBContext evoapi
    GenerateMigratinSql evocommon3 Common3DBContext evoapi

    GenerateMigratinSql evopersonal001 PersonalShard001 evoapi
    GenerateMigratinSql evopersonal002 PersonalShard001 evoapi
    GenerateMigratinSql evopersonal003 PersonalShard001 evoapi
    GenerateMigratinSql evopersonal004 PersonalShard001 evoapi
    GenerateMigratinSql evopersonal005 PersonalShard001 evoapi
    GenerateMigratinSql evopersonal006 PersonalShard001 evoapi
    GenerateMigratinSql evopersonal007 PersonalShard001 evoapi
    GenerateMigratinSql evopersonal008 PersonalShard001 evoapi

    GenerateMigratinSql evogmtool GmToolDbContext evogmtool
fi


echo "------------------------------"
echo $PACK_NAME build complete!
