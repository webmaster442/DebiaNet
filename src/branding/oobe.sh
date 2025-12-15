#!/bin/bash

set -ue

DEFAULT_GROUPS='adm,cdrom,sudo,dip,plugdev'
DEFAULT_UID='1000'

if getent passwd "$DEFAULT_UID" >/dev/null; then
    echo 'Welcome to DebiaNet. The default user account is user and the default password is pass.'
    echo 'For security reasons, it is recommended to change the password after your first login.'
    echo 'You can also create additional user accounts using the "adduser" command.'
	echo 'type debianet to execute debianet utility'
    exit 0
fi

echo 'Please create a default UNIX user account. The username does not need to match your Windows username.'
echo 'For more information visit: https://aka.ms/wslusers'

while true; do

    # Prompt from the username
    read -p 'Enter new UNIX username: ' username

    # Create the user
    if /usr/sbin/adduser --uid "$DEFAULT_UID" --quiet --gecos '' "$username"; then

        if /usr/sbin/usermod "$username" -aG "$DEFAULT_GROUPS"; then
            break
        else
            /usr/sbin/deluser "$username"
        fi
    fi
done
