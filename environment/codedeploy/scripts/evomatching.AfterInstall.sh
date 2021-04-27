#!/bin/sh

set -euo pipefail

cp /home/ec2-user/packages/evomatching.service /etc/systemd/system/
systemctl daemon-reload
systemctl enable evomatching.service
systemctl restart evomatching
