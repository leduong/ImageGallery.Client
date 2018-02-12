FROM selenium/standalone-chrome

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

RUN apt-get install dotnet-sdk-2.1.3
RUN dotnet --version


USER seluser
WORKDIR /
