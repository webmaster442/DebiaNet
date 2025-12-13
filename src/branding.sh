#!/bin/bash

rm /etc/wsl-distribution.conf
cp ./branding/wsl-distribution.conf /etc/wsl-distribution.conf

rm /usr/lib/wsl/debian_logo.ico
cp ./branding/debianet.ico /usr/lib/wsl/debianet.ico

rm /etc/os-release
cp ./branding/os-release.conf /etc/os-release

rm /usr/lib/wsl/oobe.sh
cp ./branding/oobe.sh /usr/lib/wsl/oobe.sh

cp ./branding/terminal-debianet.json /usr/lib/wsl/terminal-debianet.json

chmod 755 /etc/wsl-distribution.conf
chmod 755 /usr/lib/wsl/oobe.sh
chmod 755 /usr/lib/wsl/debianet.ico
chmod 755 /usr/lib/wsl/terminal-debianet.json
chmod 755 /etc/os-release
