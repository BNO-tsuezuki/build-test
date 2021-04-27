#!/bin/sh

set -euo pipefail

cp /home/ec2-user/packages/evosequencing.service /etc/systemd/system/
systemctl daemon-reload
systemctl enable evosequencing.service
systemctl restart evosequencing
