#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

sh $SCRIPT_DIR/commonDeployDevEnv.sh "18.182.87.121" MIGRATION_BETA

