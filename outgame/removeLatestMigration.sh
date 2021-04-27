#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

START_PROJECT=$SCRIPT_DIR/evoapi/evoapi.csproj

echo "---- remove latest migration ----"


cd $SCRIPT_DIR/evolib

export EVO_DBMIGRATION_FLAG=ON

export ASPNETCORE_ENVIRONMENT=Development
dotnet ef -s $START_PROJECT migrations remove -c $1
