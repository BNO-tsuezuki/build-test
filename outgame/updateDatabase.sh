#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

START_PROJECT=$SCRIPT_DIR/evoapi/evoapi.csproj

UpdatePersonalDatabase(){
    echo '--'
    echo PersonalShard `expr $1 + 1`
    export UPDATE_DATABASE_PERSONAL_SHARD_INDEX=$1

    if [ $# -ne 2 ];
    then
        dotnet ef -s $START_PROJECT database update -c PersonalShard001
    else
        dotnet ef -s $START_PROJECT database update -c PersonalShard001 $2
    fi
}

UpdateCommonDatabase(){
    echo '--------'
    echo "Do you want to update $1 database? [yes or no]"
    read IS_UPDATE
    if [ "$IS_UPDATE" = "yes" -o "$IS_UPDATE" = "y" ]
    then
        echo Enter target migration id.
        echo "(If the input is empty, it will be up to date !)"
        read MIGRATIONID

        echo '--'
        echo $1
        dotnet ef -s $START_PROJECT database update -c $1 $MIGRATIONID
    fi
}

echo "---- update database ----"

cd $SCRIPT_DIR/evolib

export ASPNETCORE_ENVIRONMENT=Development
export EVO_DBMIGRATION_FLAG=ON


echo Do you want to update PERSONAL database? [yes or no]
read IS_UPDATE_PERSONAL
if [ "$IS_UPDATE_PERSONAL" = "yes" -o "$IS_UPDATE_PERSONAL" = "y" ]
then
    echo Enter target migration id.
    echo "(If the input is empty, it will be up to date !)"
    read PERSONAL_MIGRATIONID

    UpdatePersonalDatabase 0 $PERSONAL_MIGRATIONID
    UpdatePersonalDatabase 1 $PERSONAL_MIGRATIONID
    UpdatePersonalDatabase 2 $PERSONAL_MIGRATIONID
    UpdatePersonalDatabase 3 $PERSONAL_MIGRATIONID
    UpdatePersonalDatabase 4 $PERSONAL_MIGRATIONID
    UpdatePersonalDatabase 5 $PERSONAL_MIGRATIONID
    UpdatePersonalDatabase 6 $PERSONAL_MIGRATIONID
    UpdatePersonalDatabase 7 $PERSONAL_MIGRATIONID
fi

UpdateCommonDatabase Common1DBContext
UpdateCommonDatabase Common2DBContext
UpdateCommonDatabase Common3DBContext
