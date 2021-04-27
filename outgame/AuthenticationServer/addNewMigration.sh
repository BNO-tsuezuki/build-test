#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

START_PROJECT=$SCRIPT_DIR/AuthenticationServer.csproj

echo "---- add new migration ----"


cd $SCRIPT_DIR

export ASPNETCORE_ENVIRONMENT=Development
dotnet ef -s $START_PROJECT migrations add -c AuthDBContext -o "Migrations" $1
