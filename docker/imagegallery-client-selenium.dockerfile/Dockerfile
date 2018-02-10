FROM microsoft/dotnet:2.0.3-sdk

RUN apt-get update && apt-get install -y \
        build-essential \
        chrpath \ 
        libssl-dev  \ 
        libxft-dev \ 
        libfreetype6-dev  \ 
        libfreetype6 \ 
        libfontconfig1-dev \ 
        libfontconfig1 \ 
        curl \
        tree \
        wget \
    && rm -rf /var/lib/apt/lists/* \
    && curl https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh > /wait_for_it.sh \
    && chmod +x /wait_for_it.sh

# Declare constants
ENV PHANTOMJS_VERSION 2.1.1

ARG BUILD_NUMBER=0
ENV BUILD_NUMBER ${BUILD_NUMBER}

RUN wget https://bitbucket.org/ariya/phantomjs/downloads/phantomjs-${PHANTOMJS_VERSION}-linux-x86_64.tar.bz2
RUN tar xvjf phantomjs-${PHANTOMJS_VERSION}-linux-x86_64.tar.bz2 -C /usr/local/share/
RUN ln -s /usr/local/share/phantomjs-${PHANTOMJS_VERSION}-linux-x86_64/bin/phantomjs /usr/local/bin/

RUN phantomjs --version
RUN which phantomjs

ARG TEST_FILTER=Category=Intergration
ENV TEST_FILTER ${TEST_FILTER}

ENV ASPNETCORE_ENVIRONMENT Testing

COPY  /test/ImageGallery.Client.Test.UI  /app/test/ImageGallery.Client.Test.UI
COPY _stylecop /app/_stylecop
COPY _stylecop /_stylecop
COPY NuGet.config /app/NuGet.config
COPY ImageGallery.Client.sln /app/ImageGallery.Client.sln


WORKDIR /app

RUN dotnet restore --ignore-failed-sources

WORKDIR /app/test/ImageGallery.Client.Test.UI
RUN dotnet build

WORKDIR /app/test/