#!/bin/sh

set -eu

MIGRATION_ID=$1

SCRIPT_DIR=$(cd $(dirname $0); pwd)

sh $SCRIPT_DIR/commonRevertMigration.sh DevEnv MIGRATION $MIGRATION_ID
