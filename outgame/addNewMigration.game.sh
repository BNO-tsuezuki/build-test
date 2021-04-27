#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

START_PROJECT=$SCRIPT_DIR/evoapi/evoapi.csproj

echo "---- add new migration ----"


cd $SCRIPT_DIR/evolib

export ASPNETCORE_ENVIRONMENT=Development
dotnet ef -s $START_PROJECT migrations add -c GameDBContext -o "Migrations/gamedb" $1
