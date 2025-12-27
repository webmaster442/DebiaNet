#!/bin/bash
sudo dpkg-query -W -f='${Package}\t${Version}\n' > installed-packages.txt