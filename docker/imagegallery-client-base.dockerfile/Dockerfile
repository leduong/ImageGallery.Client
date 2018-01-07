# Build Stage
FROM microsoft/aspnetcore-build:2.0

# Declare constants
ENV NVM_VERSION v0.33.6
ENV NODE_VERSION v8.9.1

RUN apt-get update && apt-get install -y \
    tree \
 && rm -rf /var/lib/apt/lists/*

# Install NVM
#RUN curl -o- https://raw.githubusercontent.com/creationix/nvm/${NVM_VERSION}/install.sh | bash
RUN curl -o- https://raw.githubusercontent.com/creationix/nvm/v0.33.6/install.sh | bash

#RUN source ~/.nvm/nvm.sh; \
#    nvm install $NODE_VERSION; \
#    nvm use --delete-prefix $NODE_VERSION;

RUN source ~/.nvm/nvm.sh; \
    nvm install v8.9.1; \
    nvm use --delete-prefix v8.9.1;

COPY .  /app

WORKDIR /app

RUN dotnet restore