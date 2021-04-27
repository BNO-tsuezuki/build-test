#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)
COPY_DIR=$1

mkdir -p $COPY_DIR

echo "-------------------"
echo "<<<< rsync src >>>>"
echo "-------------------"

rsync --recursive --delete -v \
--exclude='.vs' \
--exclude='.vscode' \
--exclude='*.usr' \
--exclude='bin' \
--exclude='obj' \
--exclude='node_modules' \
--exclude='.gitignore' \
--exclude='*.sh' \
$SCRIPT_DIR/ $COPY_DIR/


# rsync --recursive --delete -v \
# --exclude='bin' \
# --exclude='obj' \
# --exclude='node_modules' \
# --exclude='*.user' \
# $SCRIPT_DIR/evolib $COPY_DIR/

# rsync --recursive --delete -v \
# --exclude='bin' \
# --exclude='obj' \
# --exclude='node_modules' \
# --exclude='*.user' \
# $SCRIPT_DIR/evoapi $COPY_DIR/

# rsync --recursive --delete -v \
# --exclude='bin' \
# --exclude='obj' \
# --exclude='node_modules' \
# --exclude='*.user' \
# $SCRIPT_DIR/evomatching $COPY_DIR/

# rsync --recursive --delete -v \
# --exclude='bin' \
# --exclude='obj' \
# --exclude='node_modules' \
# --exclude='*.user' \
# $SCRIPT_DIR/evotool $COPY_DIR/
