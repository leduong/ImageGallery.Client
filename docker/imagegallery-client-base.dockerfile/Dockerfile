# Build Stage
FROM microsoft/aspnetcore-build:2.0

RUN apt-get update && apt-get install -y \
    tree \
 && rm -rf /var/lib/apt/lists/*

COPY .  /app

WORKDIR /app

RUN dotnet restore