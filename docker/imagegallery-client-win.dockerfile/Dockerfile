FROM microsoft/windowsservercore:10.0.14393.2068

ENV NODEJS_VERSION 8.9.4

SHELL ["powershell", "-Command", "$ErrorActionPreference = 'Stop'; $ProgressPreference = 'SilentlyContinue';"]

ENV chocolateyUseWindowsCompression false

RUN iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1')); \
    choco feature disable --name showDownloadProgress

RUN choco install -y nodejs -version $env:NODEJS_VERSION

## RUN dotnet --version
RUN node -v
RUN npm -v


CMD [ "cmd" ]

