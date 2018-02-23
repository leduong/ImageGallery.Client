# escape=`
FROM microsoft/dotnet:2.0.5-sdk-2.1.4-nanoserver-sac2016

# set up environment
ENV NODE_VERSION 8.9.4
ENV ASPNETCORE_PKG_VERSION 2.0.5

# Install node, bower, and git
RUN Invoke-WebRequest -UseBasicParsing https://nodejs.org/dist/v${env:NODE_VERSION}/node-v${env:NODE_VERSION}-win-x64.zip -outfile node.zip; `
    Expand-Archive node.zip -DestinationPath ${env:TEMP}/nodejs-tmp; `
    Move-Item ${env:TEMP}/nodejs-tmp/node-v${env:NODE_VERSION}-win-x64 ${env:ProgramFiles}/nodejs; `
    Remove-Item -Force node.zip; `
    $env:PATH += \";${env:ProgramFiles}/nodejs\"; `
    & npm install -g gulp bower; `
    Invoke-WebRequest -UseBasicParsing https://github.com/git-for-windows/git/releases/download/v2.16.2.windows.1/MinGit-2.16.2-busybox-64-bit.zip -outfile git.zip; `
    Expand-Archive git.zip -DestinationPath $Env:ProgramFiles/git; `
    Remove-Item -Force git.zip

RUN setx /M PATH $($Env:PATH + ';' + $Env:ProgramFiles + '/nodejs' + ';' + $Env:ProgramFiles + '/git/cmd')

RUN git --version --build-options
RUN dotnet --version
RUN node -v
RUN npm -v


# Copy SRC 
COPY src /app/src
COPY ImageGallery.Client.sln /app/ImageGallery.Client.sln
COPY NuGet.config /app/NuGet.config
WORKDIR /app

RUN dotnet restore

WORKDIR /app/src/ImageGallery.Client
RUN npm install
RUN npm run compile-app
RUN dotnet build


CMD [ "cmd" ]


