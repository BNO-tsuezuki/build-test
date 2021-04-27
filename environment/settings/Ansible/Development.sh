#!/bin/sh

set -eu

SCRIPT_DIR=$(cd $(dirname $0); pwd)

echo "---- setup [Development] environment ----"

ansible-playbook -i $SCRIPT_DIR/inventories/Development.ini $SCRIPT_DIR/playbooks/Development.yml -v  -e 'ansible_python_interpreter=/usr/bin/python3'
