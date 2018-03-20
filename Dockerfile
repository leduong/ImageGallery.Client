# Build Stage
FROM microsoft/aspnetcore-build:2.0

# Declare constants
ENV NVM_VERSION v0.33.6
ENV NODE_VERSION v8.9.1
ENV NVM_DIR /usr/local/nvm

RUN rm /bin/sh && ln -s /bin/bash /bin/sh

RUN apt-get update && apt-get install -y \
    curl \
    tree \
 && rm -rf /var/lib/apt/lists/*

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

COPY .  /app
WORKDIR /app

RUN dotnet restore --ignore-failed-sources

WORKDIR /app/src/ImageGallery.Client

RUN npm install --unsafe-perm
RUN node node_modules/webpack/bin/webpack.js --config webpack.config.js --env.prod
RUN npm run compile-app

RUN dotnet build

# Set environment variables
ENV ASPNETCORE_URLS http://*:44600
ENV ASPNETCORE_ENVIRONMENT Docker

EXPOSE 44600

ENTRYPOINT ["dotnet", "run"]
