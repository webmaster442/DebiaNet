# DebiaNet

![logo](src/branding/debianet-256.png)

A Debian based WSL Distro for .NET & Docker development based on Debian Trixie WSL distro. The purpose of DebiaNet is to simplify cross-platform .NET development by providing a pre-configured environment. This distribution is tailored for developers who work with .NET, Docker, and related tools, ensuring a seamless and efficient setup on Windows Subsystem for Linux (WSL).

The primary focus of this distribution is to provide an optimized experience within the Windows Subsystem for Linux (WSL). As such, no physical installation media (e.g., ISO images) will be created. However, if you wish to convert an existing Debian installation to DebiaNet, you can do so by using the `install.sh` script located in the `src` folder. Simply run the script to apply the necessary configurations.

## Release policy

DebiaNet is currently a personal project with a planned update and release cycle of six months. This schedule may be adjusted in the future based on the growth and needs of the user base and the release cycle of the .NET SDK versions.

## Configuration

DebiaNet comes preinstalled & preconfigured with a user to be able to use docker. The default username is `user` and the default password is `pass`. For security reasons it's highly recommended to change this password with the `passwd` command after the first login. The reason why we provide a preconfigured user is that the `user` account is configured with the nessceccary permissions to run docker. 

## Changelog

The complete list of changes can be found here: [changelog](changelog.md)

## Installed Software

**As debian packages**

- [.NET SDK 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [bat](https://github.com/sharkdp/bat)
- [binutils](https://www.gnu.org/software/binutils/)
- [curl](https://curl.se/)
- [DevToys](https://devtoys.app/)
- [Dive](https://github.com/wagoodman/dive)
- [docker](https://www.docker.com/)
- [git](https://git-scm.com/) with [git-lfs](https://git-lfs.com/)
- [htop](https://htop.dev/)
- [just](https://github.com/casey/just)
- [openssl](https://www.openssl.org/)
- [ripgrep](https://github.com/BurntSushi/ripgrep)
- [strace](https://strace.io/)
- [tmux](https://github.com/tmux/tmux/wiki)
- [wget](https://www.gnu.org/software/wget/)
- [jq](https://jqlang.org/)
- [byobu](https://www.byobu.org/)

**Apps**

- [Visual studio remote shell](https://learn.microsoft.com/en-us/visualstudio/debugger/remote-debugging?view=visualstudio)
- [Infer#](https://github.com/microsoft/infersharp)

## Installation

1. Make sure you have HyperV and WSL installed & enabled. **Note: These commands require admin rights**

```powershell
dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
```

2. After Enabling HyperV and WSL reboot your computer.

3. After reboot update WSL and set the default version to 2.0 

```powershell
wsl --update
wsl --set-default-version 2
```

4. Download latest DebaNet relese from github & install it by double clicking on the downloaded .wsl file.

## WSL Build instructions

1. Install debian: `wsl --install Debian`
2. Install user: `user` with password: `pass`
3. Run commands:

```bash
cd src
./install.sh
./copy-files.sh
```