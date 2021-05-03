#!/bin/bash

set -e -u -x

wget -q -O - https://dl-ssl.google.com/linux/linux_signing_key.pub | apt-key add -

sh -c 'echo "deb [arch=amd64] http://dl.google.com/linux/chrome/deb/ stable main" >> /etc/apt/sources.list.d/google.list'

apt-get update && apt-get install -y google-chrome-stable

cd <ApplicationName>/ClientApp

npm set progress=false && npm config set depth 0 && npm cache clean --force