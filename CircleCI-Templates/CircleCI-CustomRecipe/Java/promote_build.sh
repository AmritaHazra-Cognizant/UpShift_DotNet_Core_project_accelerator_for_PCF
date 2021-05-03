#!/bin/bash

set -e +x

echo
echo "--------Install JFrog cli---------"
curl -fL https://getcli.jfrog.io | sh
echo "--------Install JFrog cli installation is finished---------"

echo
echo "-------- Starting to execute jfrog commands ---------"

echo
echo "-------- Configuring jfrog artifactory ---------"
./jfrog rt c --url ${ARTIFACTORY_URL}artifactory --user $ARTIFACTORY_USER --apikey $ARTIFACTORY_TOKEN --interactive=false

echo
echo "-------- Searching a file in jfrog artifactory generic-dev-local/UpShift/<Application Name> ---------"
./jfrog rt s --props build.id=$CIRCLE_WORKFLOW_ID generic-dev-local/UpShift/<Application Name>/ | jq .[-1].path -r > artifact_path.txt
ARTIFACT_PATH=$(cat artifact_path.txt)
echo "ARTIFACT_PATH -> $ARTIFACT_PATH"

echo
echo "Copying this artifact to generic-local/UpShift/<Application Name>_latest.zip"
./jfrog rt cp $ARTIFACT_PATH generic-local/<Application Name>_latest.zip

echo
echo "Copying this artifact to generic-local/"
./jfrog rt cp $ARTIFACT_PATH generic-local/
