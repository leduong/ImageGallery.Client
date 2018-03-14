# ImageGallery.Client

[![This image on DockerHub](https://img.shields.io/docker/pulls/stuartshay/imagegallery-client.svg)](https://hub.docker.com/r/stuartshay/imagegallery-client/)
[![codecov](https://codecov.io/gh/stuartshay/ImageGallery.Client/branch/master/graph/badge.svg)](https://codecov.io/gh/stuartshay/ImageGallery.Client)


 Jenkins | Status  
------------ | -------------
Auth Base Image | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery-Auth/ImageGallery-Auth-base)](https://jenkins.navigatorglass.com/job/ImageGallery-Auth/job/ImageGallery-Auth-base/)
Auth Application Image | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery-Auth/ImageGallery-Auth-build)](https://jenkins.navigatorglass.com/job/ImageGallery-Auth/job/ImageGallery-Auth-build/)

### Demo
```
https://dev.informationcart.com/

User with Full Access 
L: Claire
P: password

User with Read Access 
L: Frank
P: password

```

### Prerequisites

```
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


```


### Identity Server
https://auth.informationcart.com/
      
### API
https://imagegallery-api.informationcart.com/swagger
