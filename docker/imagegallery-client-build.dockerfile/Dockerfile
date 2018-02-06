# Build Stage
FROM stuartshay/imagegallery-client:2.0-base-auth AS build-env

# Declare constants
ENV NVM_VERSION v0.33.6
ENV NODE_VERSION v8.9.1
ENV NVM_DIR /usr/local/nvm

ARG BUILD_NUMBER=0
ENV BUILD_NUMBER ${BUILD_NUMBER}

ARG LOGGLY_TOKEN=0
ENV LOGGLY_TOKEN ${BUILD_NUMBER}

# Install Packages 
RUN apt-get update && apt-get install -y \
    curl

RUN curl --silent -o- https://raw.githubusercontent.com/creationix/nvm/${NVM_VERSION}/install.sh | bash

# install node and npm
RUN source $NVM_DIR/nvm.sh \
    && nvm install $NODE_VERSION \
    && nvm alias default $NODE_VERSION \
&& nvm use default

ENV NODE_PATH $NVM_DIR/v$NODE_VERSION/lib/node_modules
ENV PATH $NVM_DIR/versions/node/v$NODE_VERSION/bin:$PATH

RUN node -v
RUN npm -v

RUN gulp -v
RUN npm bower -v

# Copy Source
COPY src /app/src
COPY ImageGallery.Client.sln /app/ImageGallery.Client.sln

WORKDIR /app/src/ImageGallery.Client

RUN dotnet setversion 2.0.0.${BUILD_NUMBER};
RUN dotnet publish -o /publish -c Release -f netcoreapp2.0 -r debian.8-x64

# Runtime Image Stage
FROM microsoft/aspnetcore:2.0

WORKDIR /publish
COPY --from=build-env /publish .

# Set environment variables
ENV ASPNETCORE_URLS http://*:44600
ENV ASPNETCORE_ENVIRONMENT Staging

EXPOSE 44600

ENTRYPOINT ["dotnet", "ImageGallery.Client.dll"]
