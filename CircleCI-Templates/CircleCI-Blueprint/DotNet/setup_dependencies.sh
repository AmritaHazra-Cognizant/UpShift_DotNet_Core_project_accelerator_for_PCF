#!/bin/bash

set -e +x
export NG_CLI_ANALYTICS=ci

echo "---------------------build-essential install starting---------------------"
apt-get update
# Required to ensure we dont get "make not found" errors
apt-get -y install build-essential
echo "---------------------build-essential install ending---------------------"

echo "---------------------nodejs install starting---------------------"
apt-get update
# Required to ensure we dont get "npm command not found" errors
curl -sL https://deb.nodesource.com/setup_14.x | bash
apt-get -y install nodejs
node --version
npm --version
echo "---------------------nodejs install completed---------------------"
pushd <ApplicationName>/ClientApp
    # Required to ensure we dont get lib-sass issues
    npm install -g --unsafe-perm node-sass
    # Required to ensure the ng commands work
    npm install -g @angular/cli@latest
popd
exit 0