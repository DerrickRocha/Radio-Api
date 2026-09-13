#!/usr/bin/env bash
set -euo pipefail

cd "$(dirname "$0")"
git pull
dotnet publish -c Release -o /tmp/radio-publish
sudo rsync -a --delete /tmp/radio-publish/ /opt/radio-api/
sudo chown -R radioapi:radioapi /opt/radio-api
sudo systemctl restart dotnet-radio-api
sudo systemctl status dotnet-radio-api --no-pager