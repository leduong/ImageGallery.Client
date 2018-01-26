FROM microsoft/dotnet:2.0.3-sdk

RUN apt-get update && apt-get install -y \
        tree \
    && rm -rf /var/lib/apt/lists/* \
    && curl https://raw.githubusercontent.com/vishnubob/wait-for-it/master/wait-for-it.sh > /wait_for_it.sh \
    && chmod +x /wait_for_it.sh

ARG BUILD_NUMBER=0
ENV BUILD_NUMBER ${BUILD_NUMBER}

ARG TEST_FILTER=Category=Intergration
ENV TEST_FILTER ${TEST_FILTER}

ENV ASPNETCORE_ENVIRONMENT Staging

COPY . /app
COPY _stylecop _stylecop

WORKDIR /app
RUN dotnet restore

WORKDIR /app/test/ImageGallery.Client.Test

RUN dotnet setversion 2.0.0.${BUILD_NUMBER};
RUN dotnet build
