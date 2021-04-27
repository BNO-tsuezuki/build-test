#!/bin/sh

set -eu

ENV=$1
DB_USAGE=$2
MIGRATION_ID=$3

COPY_DIR=~/tmp/evo_src
START_PROJECT=$COPY_DIR/evoapi/evoapi.csproj

cd $COPY_DIR/evolib

export ASPNETCORE_ENVIRONMENT=$ENV
export DB_USAGE=$DB_USAGE
dotnet ef database update $MIGRATION_ID -s $START_PROJECT -c GameDBContext
