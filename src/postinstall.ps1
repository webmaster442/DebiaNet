if (-not (Test-Path -Path $PROFILE)) {
    $profileDir = Split-Path -Path $PROFILE
    if (-not (Test-Path -Path $profileDir)) {
        New-Item -ItemType Directory -Path $profileDir -Force | Out-Null
    }
    New-Item -ItemType File -Path $PROFILE -Force | Out-Null
}
"setenv PATH `"$env:PATH:$HOME/.dotnet/tools`"" >> $PROFILE
"dotnet completions script pwsh | Out-String | Invoke-Expression" >> $PROFILE