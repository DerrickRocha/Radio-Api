#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")"
git pull
dotnet publish -c Release -o /tmp/radio-publish
sudo rsync -a --delete /tmp/radio-publish/ /opt/radio-api/
sudo chown -R radioapi:radioapi /opt/radio-api
sudo systemctl restart radio-api.service
sudo systemctl status radio-api.service --no-pager