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

RUN dotnet --version


# Declare constants
#ENV PHANTOMJS_VERSION 2.1.1

ARG BUILD_NUMBER=0
ENV BUILD_NUMBER ${BUILD_NUMBER}

#RUN wget https://bitbucket.org/ariya/phantomjs/downloads/phantomjs-${PHANTOMJS_VERSION}-linux-x86_64.tar.bz2
#RUN tar xvjf phantomjs-${PHANTOMJS_VERSION}-linux-x86_64.tar.bz2 -C /usr/local/share/
#RUN ln -s /usr/local/share/phantomjs-${PHANTOMJS_VERSION}-linux-x86_64/bin/phantomjs /usr/local/bin/
#RUN phantomjs --version
#RUN which phantomjs


#RUN apt-get install -y unzip openjdk-8-jre-headless xvfb libxi6 libgconf-2-4

# Install Chrome for Selenium
#RUN curl https://dl.google.com/linux/direct/google-chrome-stable_current_amd64.deb -o /chrome.deb
#RUN dpkg -i /chrome.deb || apt-get install -yf
#RUN rm /chrome.deb

# Install Firefox for Selenium
##RUN apt-get update
##RUN apt-get --assume-yes install software-properties-common
##RUN add-apt-repository ppa:mozillateam/firefox-next
##RUN apt-get update
##RUN apt-get install firefox

# Install geckodriver for Selenium
##RUN cd  /usr/local/bin/
##RUN wget https://github.com/mozilla/geckodriver/releases/download/v0.19.1/geckodriver-v0.19.1-linux64.tar.gz
##RUN tar -xvzf geckodriver*
##RUN chmod +x geckodriver
##RUN rm geckodriver-v0.19.1-linux64.tar.gz




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
