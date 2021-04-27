#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

echo "---- setup [DevEnv] environment ----"

if [ ! -e ~/.keys/DevEnv.pem ]; then
  echo "~/.keys/DevEnv.pemが必要です"
  exit 1
fi

ansible-playbook -i $SCRIPT_DIR/inventories/DevEnv.ini $SCRIPT_DIR/playbooks/DevEnv.yml --private-key=~/.keys/DevEnv.pem -v  -e 'ansible_python_interpreter=/usr/bin/python3'
