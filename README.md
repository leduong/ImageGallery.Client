# ImageGallery.Client

[![This image on DockerHub](https://img.shields.io/docker/pulls/stuartshay/imagegallery-client.svg)](https://hub.docker.com/r/stuartshay/imagegallery-client/)
[![Greenkeeper badge](https://badges.greenkeeper.io/stuartshay/ImageGallery.Client.svg)](https://greenkeeper.io/)

[![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=ImageGalleryClient&metric=alert_status)](http://sonar.navigatorglass.com:9000/dashboard?id=ImageGalleryClient)
[![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=ImageGalleryClient&metric=reliability_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=ImageGalleryClient)
[![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=ImageGalleryClient&metric=security_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=ImageGalleryClient)
[![SonarCloud](http://sonar.navigatorglass.com:9000/api/project_badges/measure?project=ImageGalleryClient&metric=sqale_rating)](http://sonar.navigatorglass.com:9000/dashboard?id=ImageGalleryClient)


 Jenkins | Status  
------------ | -------------
Base Image (Auth) | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery-Auth/ImageGallery-Auth-base)](https://jenkins.navigatorglass.com/job/ImageGallery-Auth/job/ImageGallery-Auth-base/)
Application Image (Auth) | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery-Auth/ImageGallery-Auth-build)](https://jenkins.navigatorglass.com/job/ImageGallery-Auth/job/ImageGallery-Auth-build/)
Local Image (Auth) | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery-Auth/ImageGallery-Auth-local)](https://jenkins.navigatorglass.com/job/ImageGallery-Auth/job/ImageGallery-Auth-local/)
SonarQube (Auth) | [![Build Status](https://jenkins.navigatorglass.com/buildStatus/icon?job=ImageGallery-Auth/ImageGallery-Auth-sonarqube)](https://jenkins.navigatorglass.com/job/ImageGallery-Auth/job/ImageGallery-Auth-sonarqube/)


### Demo
```
https://dev.informationcart.com
L: Claire P: password
```

### Prerequisites

```
Node v9.3.0
NET Core 2.1
VS Code 1.19.1 or VS 2017 15.8.0
```

### Install & Run

```
cd ImageGallery.Client
dotnet restore

cd src\ImageGallery.Client

npm install

npm run compile-app

dotnet run

http://localhost:8000/
```
