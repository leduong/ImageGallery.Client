FROM selenium/standalone-chrome

# Get Distro Properties
RUN cat /etc/*-release
RUN which selenium

USER root

RUN apt-get update && apt-get install -y \
        curl \
        unzip \ 
        tree \
        wget \
        libunwind8 \
        liblttng-ust0 \
        libcurl3 \
        libssl1.0.0 \
        libuuid1 \
        libkrb5-3 \
        zlib1g \
        libicu55 \
    && rm -rf /var/lib/apt/lists/*


RUN  sh -c 'echo "deb [arch=amd64] https://packages.microsoft.com/repos/microsoft-ubuntu-xenial-prod xenial main" > /etc/apt/sources.list.d/dotnetdev.list'
RUN  apt-get update

RUN apt-get install dotnet-sdk-2.1.3
RUN dotnet --version


USER seluser
WORKDIR /
