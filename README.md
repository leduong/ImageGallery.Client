# ImageGallery.Client

[![This image on DockerHub](https://img.shields.io/docker/pulls/stuartshay/imagegallery-client.svg)](https://hub.docker.com/r/stuartshay/imagegallery-client/)
[![codecov](https://codecov.io/gh/stuartshay/ImageGallery.Client/branch/master/graph/badge.svg)](https://codecov.io/gh/stuartshay/ImageGallery.Client)

Jenkins | Status
------------ | -------------
Base Image | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery/ImageGallery-Client-Base-2)](https://jenkins.navigatorglass.com/job/ImageGallery/job/ImageGallery-Client-Base-2/)
Application Image | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery/ImageGallery-Client-Build-2)](https://jenkins.navigatorglass.com/job/ImageGallery/job/ImageGallery-Client-Build-2/)
Intergation Testing | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery/ImageGallery-Client-Test-2)](https://jenkins.navigatorglass.com/job/ImageGallery/job/ImageGallery-Client-Test-2/)
Test/Selenium Base | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery/ImageGallery-Client-Selenium-Base-2)](https://jenkins.navigatorglass.com/job/ImageGallery/job/ImageGallery-Client-Selenium-Base-2/)
Selenium Grid | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery/ImageGallery-Client-Selenium-2)](https://jenkins.navigatorglass.com/job/ImageGallery/job/ImageGallery-Client-Selenium-2/)
Local | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery/ImageGallery-Client-Local-2)](https://jenkins.navigatorglass.com/job/ImageGallery/job/ImageGallery-Client-Local-2/)
AppVeyor |[![Build status](https://ci.appveyor.com/api/projects/status/iub0tbs42d9ut0g7?svg=true)](https://ci.appveyor.com/project/StuartShay/imagegallery-client)

### Demo

```bash
https://dev.informationcart.com

User with Full Access 
L: Claire
P: password

User with Read Access
L: Frank
P: password

```

### Prerequisites

```bash
Node v8.9.3
NET Core 2.0.5
VS Code 1.19.1 or VS 2017 15.6.0
```

### Install

```
cd ImageGallery.Client
dotnet restore

cd src\ImageGallery.Client

npm install

npm run compile-app

dotnet run

http://localhost:5000/home
```

### Identity Server
https://auth.informationcart.com/
      
### API
https://imagegallery-api.informationcart.com/swagger
