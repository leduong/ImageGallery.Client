FROM microsoft/aspnetcore-build:2.0
MAINTAINER Stuart Shay

ENV ASPNETCORE_URLS="http://*:44300"
ENV ASPNETCORE_ENVIRONMENT="Staging"

EXPOSE 44300

COPY src /app/src
COPY ImageGallery.Client.sln /app/ImageGallery.Client.sln
COPY NuGet.config /app/NuGet.config
WORKDIR /app

RUN dotnet restore

WORKDIR /app/src/ImageGallery.Client
RUN dotnet build

ENTRYPOINT ["dotnet", "run"]