FROM microsoft/dotnet:2.0.3-sdk

RUN apt-get update && apt-get install -y \
        build-essential \
        curl \
        tree \
    && rm -rf /var/lib/apt/lists/* \
    && curl https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh > /wait_for_it.sh \
    && chmod +x /wait_for_it.sh

# Declare constants
ENV NVM_VERSION v0.33.6
ENV NODE_VERSION v8.9.1
ENV NVM_DIR /usr/local/nvm

ARG BUILD_NUMBER=0
ENV BUILD_NUMBER ${BUILD_NUMBER}

# Install Packages 
RUN curl -sL https://deb.nodesource.com/setup_8.x | bash
RUN apt-get install nodejs

RUN curl --silent -o- https://raw.githubusercontent.com/creationix/nvm/${NVM_VERSION}/install.sh | bash

# install node and npm
##RUN source $NVM_DIR/nvm.sh \
##    && nvm install $NODE_VERSION \
##    && nvm alias default $NODE_VERSION \
##&& nvm use default

ENV NODE_PATH $NVM_DIR/v$NODE_VERSION/lib/node_modules
ENV PATH $NVM_DIR/versions/node/v$NODE_VERSION/bin:$PATH

RUN node -v
RUN npm -v

RUN npm install -g bower gulp \ 
&& echo '{ "allow_root": true }' > /root/.bowerrc

RUN gulp -v
RUN npm bower -v

ARG TEST_FILTER=Category=Intergration
ENV TEST_FILTER ${TEST_FILTER}

ENV ASPNETCORE_ENVIRONMENT Staging

COPY . /app
COPY _stylecop _stylecop
COPY ImageGallery.Client.sln /app/ImageGallery.Client.sln

WORKDIR /app
RUN dotnet restore

WORKDIR /app/src/ImageGallery.Client
RUN npm install --unsafe-perm
RUN node node_modules/webpack/bin/webpack.js --config webpack.config.js --env.prod


WORKDIR /app

RUN dotnet restore
RUN dotnet build

WORKDIR /app/test/



#WORKDIR /app/test/ImageGallery.Client.Test
#RUN dotnet setversion 2.0.0.${BUILD_NUMBER};
#RUN dotnet build
