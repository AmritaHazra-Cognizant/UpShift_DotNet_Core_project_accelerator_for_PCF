#!/bin/bash

set -e +x
export NG_CLI_ANALYTICS=ci

java -version

echo "---------------------nodejs install starting---------------------"
apt-get update
# Required to ensure we dont get "npm command not found" errors
curl -sL https://deb.nodesource.com/setup_15.x | bash
apt-get -y install nodejs
node --version
npm --version
echo "---------------------nodejs install completed---------------------"

pushd ui
    # Required to ensure the ng commands work
    npm install -g @angular/cli@latest
popd
exit 0