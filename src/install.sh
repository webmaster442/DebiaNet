#!/bin/bash

#set pipefail
set -euo pipefail

#remove conflicting packages with docker
sudo apt remove $(dpkg --get-selections docker.io docker-compose docker-doc podman-docker containerd runc | cut -f1)

# update package sources
sudo apt update

# dist upgrade
sudo apt dist-upgrade -y

# install apt packages
sudo apt install -y wget ca-certificates curl gpg libxml2 mc htop openssl git git-lfs lazygit tmux bat openssh-server unzip fastfetch

# prerpare docker install
sudo curl -fsSL https://download.docker.com/linux/debian/gpg -o /etc/apt/keyrings/docker.asc
sudo chmod a+r /etc/apt/keyrings/docker.asc
sudo tee /etc/apt/sources.list.d/docker.sources <<EOF
Types: deb
URIs: https://download.docker.com/linux/debian
Suites: $(. /etc/os-release && echo "$VERSION_CODENAME")
Components: stable
Signed-By: /etc/apt/keyrings/docker.asc
EOF

# install .net package sources
wget https://packages.microsoft.com/config/debian/13/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# update package sources
sudo apt update

# install .net
sudo apt install -y dotnet-sdk-10.0 dotnet-sdk-9.0 dotnet-sdk-8.0

# install docker
sudo apt install -y docker-ce docker-ce-cli containerd.io docker-buildx-plugin docker-compose-plugin

# configure docker user
sudo usermod -aG docker $USER

# update dotnet workloads
sudo dotnet workload update

# install devtoys.cli
wget https://github.com/DevToys-app/DevToys/releases/download/v2.0.8.0/devtoys.cli_linux_x64.deb -O devtoys.cli.deb
sudo dpkg -i devtoys.cli.deb
rm devtoys.cli.deb

# install VS Remote debugger
curl -sSL https://aka.ms/getvsdbgsh | /bin/sh /dev/stdin -v latest -l ~/vsdbg

# install ripgrep
wget https://github.com/BurntSushi/ripgrep/releases/download/15.1.0/ripgrep_15.1.0-1_amd64.deb -O ripgrep.deb
sudo dpkg -i ripgrep.deb
rm ripgrep.deb

# install .NET Global tools
dotnet tool install --global PowerShell

sudo dotnet workload update

dotnet tool install --global dotnet-ef
dotnet tool install --global csharprepl
dotnet tool install --global dotnet-coverage
dotnet tool install --global Microsoft.VisualStudio.SlnGen.Tool
dotnet tool install --global upgrade-assistant
dotnet tool install --global ilspycmd
dotnet tool install --global DotnetPackaging.Tool


# set path for dotnet tools
echo 'export PATH="$PATH:$HOME/.dotnet/tools"' >> ~/.bashrc
echo 'eval "$(dotnet completions script bash)"' >> ~/.bashrc
export PATH="$PATH:$HOME/.dotnet/tools"
pwsh postinstall.ps1

# cleanup
sudo apt autoremove -y
sudo apt clean
sudo apt autoclean

dotnet nuget locals all --clear

clear
echo "Installation complete! Please restart your terminal or log out and back in for all changes to take effect."