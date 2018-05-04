#### Base Stage
FROM microsoft/dotnet:2.1-sdk AS base

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

ENV NODE_PATH $NVM_DIR/$NODE_VERSION/lib/node_modules
ENV PATH $NVM_DIR/versions/node/$NODE_VERSION/bin:$PATH

RUN node -v
RUN npm -v

RUN npm install -g bower gulp \ 
&& echo '{ "allow_root": true }' > /root/.bowerrc

RUN gulp -v
RUN npm bower -v

#### Package Stage
FROM base AS package

COPY .  /app
WORKDIR /app

RUN dotnet restore /p:RestoreUseSkipNonexistentTargets="false"

WORKDIR /app/src/ImageGallery.Client
RUN npm install


####  Publish 
FROM package AS publish
COPY --from=base /app /app 

WORKDIR /app/src/ImageGallery.Client
RUN dotnet publish -o /publish -c Release -f netcoreapp2.1 -r debian.9-x64

#### RELEASE 
FROM microsoft/dotnet:2.1-aspnetcore-runtime AS release


RUN apt-get update && apt-get install -y \
        curl \
        dnsutils \ 
        iputils-ping \ 
		telnet \
        tree \
    && rm -rf /var/lib/apt/lists/* \
    && curl https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh > /wait_for_it.sh \
    && chmod +x /wait_for_it.sh

WORKDIR /publish
COPY --from=publish /publish .

COPY --from=publish /app/scripts /publish
RUN chmod +x **/*.sh


# Set environment variables
ENV ASPNETCORE_URLS http://*:44600
ENV ASPNETCORE_ENVIRONMENT Docker

EXPOSE 44600

CMD ["/bin/bash"]
#ENTRYPOINT ["/entrypoint.sh"]