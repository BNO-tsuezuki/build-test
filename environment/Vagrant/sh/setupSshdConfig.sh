#!/bin/sh

set -eu

echo "---- setup Sshd config ----"

DST_FILE=/etc/ssh/sshd_config
SRC_FILE=${DST_FILE}_def


if [ ! -e $SRC_FILE ]; then
  cp $DST_FILE $SRC_FILE
fi

sed -r "s/^PasswordAuthentication(\s+)no/PasswordAuthentication yes/" $SRC_FILE > $DST_FILE
systemctl restart sshd
