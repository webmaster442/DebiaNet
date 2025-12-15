# DebiaNet

![logo](src/branding/debianet192.png)

A Debian based WSL Distro for .NET & Docker development based on Debian Trixie WSL distro. The purpose of DebiaNet is to simplify cross-platform .NET development by providing a pre-configured environment. This distribution is tailored for developers who work with .NET, Docker, and related tools, ensuring a seamless and efficient setup on Windows Subsystem for Linux (WSL).

The primary focus of this distribution is to provide an optimized experience within the Windows Subsystem for Linux (WSL). As such, no physical installation media (e.g., ISO images) will be created. However, if you wish to convert an existing Debian installation to DebiaNet, you can do so by using the `install.sh` script located in the `src` folder. Simply run the script to apply the necessary configurations.

## Release policy

DebiaNet is currently a personal project with a planned update and release cycle of six months. This schedule may be adjusted in the future based on the growth and needs of the user base.

## Configuration

DebiaNet comes preinstalled & preconfigured with a user to be able to use docker. The default username is `user` and the default password is `pass`. For security reasons it's highly recommended to change this password with the `passwd` command after the first login. The reason why we provide a preconfigured user is that the `user` account is configured with the nessceccary permissions to run docker. 

## Installed Software

**As debian packages**

- [htop](https://htop.dev/)
- [openssl](https://www.openssl.org/)
- [git](https://git-scm.com/)
- [wget](https://www.gnu.org/software/wget/)
- [curl](https://curl.se/)
- [docker](https://www.docker.com/)
- [.NET SDK 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0)
- [.NET SDK 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)
- [DevToys](https://devtoys.app/)
- [lazygit](https://github.com/jesseduffield/lazygit)
- [tmux](https://github.com/tmux/tmux/wiki)
- [bat](https://github.com/sharkdp/bat)
- [ripgrep](https://github.com/BurntSushi/ripgrep)
- [Visual studio remote shell](https://learn.microsoft.com/en-us/visualstudio/debugger/remote-debugging?view=visualstudio)
- [FastFetch](https://github.com/fastfetch-cli/fastfetch)
- [Dive](https://github.com/wagoodman/dive)

**.NET tools from microsoft**

- [powershell](https://learn.microsoft.com/en-us/powershell/scripting/install/install-powershell-on-linux)
- [dotnet-ef](https://learn.microsoft.com/en-us/ef/core/cli/dotnet)
- [csharprepl](https://github.com/waf/CSharpRepl)
- [dotnet-coverage](https://learn.microsoft.com/en-us/dotnet/core/additional-tools/dotnet-coverage)
- [dotnet-counters](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-counters)
- [dotnet-gcdump](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-gcdump)
- [dotnet-monitor](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-monitor)
- [dotnet-trace](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-trace)
- [dotnet-stack](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-stack)
- [dotnet-symbol](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/dotnet-symbol)
- [sngen](https://github.com/microsoft/slngen)
- [upgrade-assistant](https://learn.microsoft.com/en-us/dotnet/core/porting/upgrade-assistant-overview)

**3rd party .NET tools**

- [ilspycmd](github.com/icsharpcode/ILSpy)
- [DotnetPackaging.Tool](https://github.com/quamotion/dotnet-packaging)
- [roslynator](https://github.com/dotnet/roslynator?tab=readme-ov-file#command-line-tool)

## Installation

1. Make sure you have HyperV and WSL installed & enabled

```powershell
dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
```

2. After Enabling HyperV and WSL reboot your computer.

3. After reboot update WSL and set the default version to 2.0 

```powershell
wsl --set-default-version 2
wsl --update
```

4. Download latest DebaNet relese from github & install it by double clicking on the downloaded .wsl file.

## WSL Build instructions

1. Install debian: `wsl --install Debian`
2. Install user: `user` with password: `pass`
3. Run install.sh: `./src/install.sh`
4. Run branding.sh: `sudo ./src/branding.sh`
5. exit: `exit`
6. do a shutdown: `wsl --shutdown`
7. export: `wsl --export Debian --format tar.xz debiannet.wsl`
8. unregister debian: `wsl --unregister Debian`
9. reinstall debianet.wsl