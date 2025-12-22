#!/bin/bash

sudo rm  /etc/wsl-distribution.conf
sudo cp ./branding/wsl-distribution.conf /etc/wsl-distribution.conf

sudo rm /usr/lib/wsl/debian_logo.ico
sudo cp ./branding/debianet.ico /usr/lib/wsl/debianet.ico

sudo rm /etc/os-release
sudo cp ./branding/os-release.conf /etc/os-release

sudo rm /usr/lib/wsl/oobe.sh
sudo cp ./branding/oobe.sh /usr/lib/wsl/oobe.sh

sudo cp ./branding/terminal-debianet.json /usr/lib/wsl/terminal-debianet.json

sudo chmod 755 /etc/wsl-distribution.conf
sudo chmod 755 /usr/lib/wsl/oobe.sh
sudo chmod 755 /usr/lib/wsl/debianet.ico
sudo chmod 755 /usr/lib/wsl/terminal-debianet.json
sudo chmod 755 /etc/os-release
