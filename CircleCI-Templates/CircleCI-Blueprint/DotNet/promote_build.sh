#!/bin/bash

set -e +x

echo "--------Install JFrog cli---------"
curl -fL https://getcli.jfrog.io | sh
echo "--------Install JFrog cli installation is finished---------"


./jfrog rt c --url ${ARTIFACTORY_URL}artifactory --user $ARTIFACTORY_USER --apikey $ARTIFACTORY_TOKEN --interactive=false
./jfrog rt s --props build.id=$CIRCLE_WORKFLOW_ID generic-dev-local/UpShift/<ApplicationName>/ | jq .[-1].path -r > artifact_path.txt
ARTIFACT_PATH=$(cat artifact_path.txt)
./jfrog rt cp $ARTIFACT_PATH generic-local/<ApplicationName>_latest.zip
./jfrog rt cp $ARTIFACT_PATH generic-local/