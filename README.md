# ImageGallery.Client

[![This image on DockerHub](https://img.shields.io/docker/pulls/stuartshay/imagegallery-client.svg)](https://hub.docker.com/r/stuartshay/imagegallery-client/)
 [![dependencies Status](https://david-dm.org/stuartshay/ImageGallery.Client/status.svg)](https://david-dm.org/stuartshay/ImageGallery.Client) [![devDependencies Status](https://david-dm.org/stuartshay/ImageGallery.Client/dev-status.svg)](https://david-dm.org/stuartshay/ImageGallery.Client?type=dev) 


 Jenkins | Status  
------------ | -------------
Base Image | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery/ImageGallery-Client-Base-2)](https://jenkins.navigatorglass.com/job/ImageGallery/job/ImageGallery-Client-Base-2/)
Application Image | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery/ImageGallery-Client-Build-2)](https://jenkins.navigatorglass.com/job/ImageGallery/job/ImageGallery-Client-Build-2/)


### Prerequisites

```
Node v6.10.1
NET Core 2.0.2
VS Code 1.18.0 or VS 2017 15.4.2
```

### Install

```
cd ImageGallery.Client
dotnet restore

cd src\ImageGallery.Client

npm install

dotnet run

http://localhost:5000/home
```

### Docker
```
docker build -t imagegallery-core-base

docker run -p 8080:44300 imagegallery-core-base
```

### Demo
```
https://imagegallery-client.informationcart.com/

User with Full Access 
L: Claire
P: password

User with Read Access 
L: Frank
P: password

```

### Identity Server
https://auth.informationcart.com/
      
### API
https://imagegallery-api.informationcart.com/swagger


### Samples
https://github.com/MarkPieszak/aspnetcore-angular2-universal
