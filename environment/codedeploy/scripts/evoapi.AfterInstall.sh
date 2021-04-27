#!/bin/sh

set -euo pipefail

cp /home/ec2-user/packages/evoapi.service /etc/systemd/system/
systemctl daemon-reload
systemctl enable evoapi.service
systemctl restart evoapi
