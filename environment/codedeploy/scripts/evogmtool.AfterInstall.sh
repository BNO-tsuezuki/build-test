#!/bin/sh

set -euo pipefail

cp /home/ec2-user/packages/evogmtool.service /etc/systemd/system/
systemctl daemon-reload
systemctl enable evogmtool.service
systemctl restart evogmtool
