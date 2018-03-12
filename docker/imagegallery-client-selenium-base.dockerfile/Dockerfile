FROM selenium/standalone-firefox:latest


## CHNAGE BASE IMAGE TAG TO MATCH GECKO_DRIVER

ENV NET_CORE_SDK_VERSION 2.1.4
ENV GECKO_DRIVER 0.19.1

# Get Distro Properties
RUN cat /etc/*-release

USER root

RUN apt-get update && apt-get install -y \
        apt-transport-https \
        curl \
        unzip \ 
        tree \
        wget \
        gettext \
        libunwind8 \
        liblttng-ust0 \
        libcurl3 \
        libssl1.0.0 \
        libuuid1 \
        libkrb5-3 \
        zlib1g \
        libicu55 \
    && rm -rf /var/lib/apt/lists/*


RUN curl https://packages.microsoft.com/keys/microsoft.asc | gpg --dearmor > microsoft.gpg
RUN mv microsoft.gpg /etc/apt/trusted.gpg.d/microsoft.gpg

RUN sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-xenial-prod xenial main" > /etc/apt/sources.list.d/dotnetdev.list'
RUN apt-get update

RUN apt-get install dotnet-sdk-$NET_CORE_SDK_VERSION -y
RUN apt-get install dotnet-runtime-2.1.0-preview1-26216-03 -y
RUN dotnet --version

RUN ln -s /opt/geckodriver-$GECKO_DRIVER  /usr/local/bin/geckodriver

USER seluser
WORKDIR /
