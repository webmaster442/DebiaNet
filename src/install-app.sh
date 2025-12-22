#!/bin/bash

sudo rm /usr/bin/debianet
sudo rm -r /opt/debianet-app/DebiaNetApp 
	  
cd DebiaNetApp
sudo dotnet publish -r linux-x64 -c Release -o ./publish DebiaNetApp.csproj
sudo mkdir -p /opt/debianet-app
sudo cp -r ./publish/* /opt/debianet-app

sudp rm -f /usr/bin/debianet
sudo ln -s /opt/debianet-app/DebiaNetApp /usr/bin/debianet
sudo chmod +x /usr/bin/debianet
