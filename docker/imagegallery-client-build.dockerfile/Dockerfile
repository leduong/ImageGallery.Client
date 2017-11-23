# Build Stage
FROM stuartshay/imagegallery-client:2.0-base AS build-env

ARG BUILD_NUMBER=0
ENV BUILD_NUMBER ${BUILD_NUMBER}

COPY src /app/src
COPY ImageGallery.Client.sln /app/ImageGallery.Client.sln

WORKDIR /app/src/ImageGallery.Client

RUN node --version
RUN npm --version

RUN dotnet setversion 2.0.0.${BUILD_NUMBER};
RUN dotnet publish -o /publish -c Release -f netcoreapp2.0 -r debian.8-x64

# Runtime Image Stage
FROM microsoft/aspnetcore:2.0

WORKDIR /publish
COPY --from=build-env /publish .

# Set environment variables
ENV ASPNETCORE_URLS="http://*:44300"
ENV ASPNETCORE_ENVIRONMENT="Staging"

EXPOSE 44300

ENTRYPOINT ["dotnet", "ImageGallery.Client.dll"]