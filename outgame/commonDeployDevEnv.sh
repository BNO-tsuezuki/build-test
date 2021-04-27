#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)
TARGET_ADDR=$1
DB_USAGE=$2

COPY_DIR=~/tmp/evo_src

PUBLISH_PACK_DIR=~/tmp/evo_packs

START_PROJECT=$COPY_DIR/evoapi/evoapi.csproj


echo "---- deploy [DevEnv] environment ----"


sh $SCRIPT_DIR/rsyncSrc.sh $COPY_DIR


echo "-------------------------"
echo "<<<< database update >>>>"
echo "-------------------------"

cd $COPY_DIR/evolib

export ASPNETCORE_ENVIRONMENT=DevEnv
export DB_USAGE=$DB_USAGE
echo "< gamedb >"
dotnet ef -s $START_PROJECT database update -c GameDBContext
echo "< accountdb >"
dotnet ef -s $START_PROJECT database update -c AccountDBContext

echo "-----------------------"
echo "<<<< copy settings >>>>"
echo "-----------------------"
mkdir -p $PUBLISH_PACK_DIR
cp $COPY_DIR/appsettings.DevEnv.json $PUBLISH_PACK_DIR/
cp $COPY_DIR/appsettings.json $PUBLISH_PACK_DIR/


echo "-------------------"
echo "<<<< build api >>>>"
echo "-------------------"

cd $COPY_DIR/evoapi

# dotnet publish -c Debug -f netcoreapp2.0 -r ubuntu.16.04-x64 -o $PUBLISH_PACK_DIR/evoapi --no-dependencies
dotnet publish -c Debug -o $PUBLISH_PACK_DIR/evoapi


echo "------------------------"
echo "<<<< build matching >>>>"
echo "------------------------"

cd $COPY_DIR/evomatching

# dotnet publish -c Debug -f netcoreapp2.0 -r ubuntu.16.04-x64 -o $PUBLISH_PACK_DIR/evoapi --no-dependencies
dotnet publish -c Debug -o $PUBLISH_PACK_DIR/evomatching


echo "--------------------------"
echo "<<<< build sequencing >>>>"
echo "--------------------------"

cd $COPY_DIR/evosequencing

# dotnet publish -c Debug -f netcoreapp2.0 -r ubuntu.16.04-x64 -o $PUBLISH_PACK_DIR/evoapi --no-dependencies
dotnet publish -c Debug -o $PUBLISH_PACK_DIR/evosequencing


echo "--------------------"
echo "<<<< build tool >>>>"
echo "--------------------"

cd $COPY_DIR/evotool

npm update --no-bin-links # --no-bin-links ダウンロード量節約のためのシンボリックリンク作成を禁止 これがないとホストがWindowsのVM環境ではこける

# dotnet publish -c Debug -f netcoreapp2.0 -r ubuntu.16.04-x64 -o $PUBLISH_PACK_DIR/evotool --no-dependencies
dotnet publish -c Debug -o $PUBLISH_PACK_DIR/evotool


echo "--------------------"
echo "<<<<   upload   >>>>"
echo "--------------------"
ssh ubuntu@$TARGET_ADDR "sudo systemctl stop evotool"
ssh ubuntu@$TARGET_ADDR "sudo systemctl stop evosequencing"
ssh ubuntu@$TARGET_ADDR "sudo systemctl stop evomatching"
ssh ubuntu@$TARGET_ADDR "sudo systemctl stop evoapi"
rsync -e "ssh " --recursive --delete --progress $PUBLISH_PACK_DIR/ ubuntu@$TARGET_ADDR:~/evo_packs/
ssh ubuntu@$TARGET_ADDR "sudo systemctl start evoapi"
ssh ubuntu@$TARGET_ADDR "sudo systemctl start evomatching"
ssh ubuntu@$TARGET_ADDR "sudo systemctl start evosequencing"
ssh ubuntu@$TARGET_ADDR "sudo systemctl start evotool"
