#!/bin/sh

set -euo pipefail

cp /home/ec2-user/packages/evotool.service /etc/systemd/system/
systemctl daemon-reload
systemctl enable evotool.service
systemctl restart evotool
