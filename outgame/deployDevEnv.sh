#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

sh $SCRIPT_DIR/commonDeployDevEnv.sh "13.113.119.202" MIGRATION

