#!/bin/bash

sudo rm  /etc/wsl-distribution.conf
sudo rm /usr/lib/wsl/debian_logo.ico
sudo rm /usr/lib/wsl/oobe.sh
sudo mv /etc/os-release /etc/os_release.bak

chmod 755 -R ./Root
cd /Root
cp -R -f ./etc /etc
cp -R -f ./usr /usr