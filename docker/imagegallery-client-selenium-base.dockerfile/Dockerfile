FROM selenium/standalone-chrome

# Get Distro Properties
RUN cat /etc/*-release

RUN apt-get update && apt-get install -y \
        curl \
        unzip \ 
        tree \
        wget \
    && rm -rf /var/lib/apt/lists/*









WORKDIR /
