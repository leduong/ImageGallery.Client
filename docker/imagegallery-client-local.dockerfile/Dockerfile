# Build Stage
FROM microsoft/aspnetcore-build:2.0 AS build-env

# Declare constants
ENV NVM_VERSION v0.33.8
ENV NODE_VERSION v8.9.1
ENV NVM_DIR /usr/local/nvm

RUN rm /bin/sh && ln -s /bin/bash /bin/sh

RUN apt-get update && apt-get install -y \
    curl \
    tree \
 && rm -rf /var/lib/apt/lists/* \
 && curl -o- https://raw.githubusercontent.com/creationix/nvm/$NVM_VERSION/install.sh | bash

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

WORKDIR /app/src/ImageGallery.Client
RUN npm install

RUN dotnet publish -o /publish -c Release -f netcoreapp2.0 -r debian.8-x64


# Runtime Image Stage
FROM microsoft/aspnetcore:2

RUN apt-get update && apt-get install -y \
        curl \
        dnsutils \ 
        iputils-ping \ 
		telnet \
        tree \
    && rm -rf /var/lib/apt/lists/* \
    && curl https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh > /wait_for_it.sh \
&& chmod +x /wait_for_it.sh

RUN curl --silent -o- https://raw.githubusercontent.com/creationix/nvm/${NVM_VERSION}/install.sh | bash

WORKDIR /app
COPY --from=build-env /app .
COPY --from=build-env /publish .


CMD ["/bin/bash"]