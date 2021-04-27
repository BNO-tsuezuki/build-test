#!/bin/sh

set -eu

PACK_SRC_PATH=~/tmp/$PACK_NAME/AuthenticationServer
PACK_DST_PATH=~/packages


mkdir -p $PACK_DST_PATH

rsync --recursive --delete -v $PACK_SRC_PATH/ $PACK_DST_PATH/

sudo cp $SRC_ROOT_PATH/environment/cloudformation/bno-evo/AuthenticationServer/AuthenticationServer.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl enable AuthenticationServer.service
sudo systemctl restart AuthenticationServer
