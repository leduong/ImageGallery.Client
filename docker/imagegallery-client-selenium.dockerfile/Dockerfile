FROM stuartshay/imagegallery-client:2.0.0-test-base

USER root

RUN apt-get update && apt-get install -y \
        curl \
        firefox \
        tree \
        wget \
    && rm -rf /var/lib/apt/lists/* \
    && curl https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh > /wait_for_it.sh \
    && chmod +x /wait_for_it.sh 


# Install geckodriver:
RUN export BASE_URL=https://github.com/mozilla/geckodriver/releases/download \
  && export VERSION=$(curl -sL \
    https://api.github.com/repos/mozilla/geckodriver/releases/latest | \
    grep tag_name | cut -d '"' -f 4) \
  && curl -sL \
  $BASE_URL/$VERSION/geckodriver-$VERSION-linux64.tar.gz | tar -xz \
&& mv geckodriver /usr/local/bin/geckodriver \
&& chmod +x /usr/local/bin/geckodriver

RUN which geckodriver
RUN export PATH=$PATH:/usr/local/bin/geckodriver

ENV DOTNET_CLI_TELEMETRY_OPTOUT 1
RUN dotnet --version

ARG BUILD_NUMBER=0
ENV BUILD_NUMBER ${BUILD_NUMBER}

ARG TEST_FILTER=Category=Intergration
ENV TEST_FILTER ${TEST_FILTER}

ENV ASPNETCORE_ENVIRONMENT Testing

COPY  /test/ImageGallery.Client.Test.UI  /app/test/ImageGallery.Client.Test.UI
COPY _stylecop /app/_stylecop
COPY _stylecop /_stylecop
COPY NuGet.config /app/NuGet.config
COPY Common.props /app/Common.props
COPY ImageGallery.Client.sln /app/ImageGallery.Client.sln


WORKDIR /app

RUN dotnet restore --ignore-failed-sources

WORKDIR /app/test/ImageGallery.Client.Test.UI
RUN dotnet build

WORKDIR /app/test/
