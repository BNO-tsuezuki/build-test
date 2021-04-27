#!/bin/sh

set -eu

echo "---- setup Ansible ----"
 
sudo apt-add-repository -y ppa:ansible/ansible-2.4
sudo  apt-get update
sudo apt-get install -y ansible
ansible --version
echo "StrictHostKeyChecking=no" >> ~/.ssh/config
echo "UserKnownHostsFile=/dev/null" >> ~/.ssh/config
