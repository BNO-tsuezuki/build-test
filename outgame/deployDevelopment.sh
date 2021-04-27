#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

COPY_DIR=~/tmp/evo_src

START_PROJECT=$COPY_DIR/evoapi/evoapi.csproj

echo "---- deploy [Development] environment ----"

sh $SCRIPT_DIR/rsyncSrc.sh $COPY_DIR

echo "-------------------------"
echo "<<<< database update >>>>"
echo "-------------------------"

cd $COPY_DIR/evolib

export ASPNETCORE_ENVIRONMENT=Development
export DB_USAGE=MIGRATION
echo "< gamedb >"
dotnet ef -s $START_PROJECT database update -c GameDBContext
echo "< accountdb >"
dotnet ef -s $START_PROJECT database update -c AccountDBContext



# echo "--------------------"
# echo "<<<< build tool >>>>"
# echo "--------------------"
# cd $COPY_DIR/evotool


# echo "--------------------"
# echo "<<<<   upload   >>>>"
# echo "--------------------"
