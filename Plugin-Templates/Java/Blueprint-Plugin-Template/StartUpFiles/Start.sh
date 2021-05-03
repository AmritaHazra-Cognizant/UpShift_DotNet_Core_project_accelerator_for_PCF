#!/bin/bash

serverPort=$1

echo "Starting app at port = "$serverPort

java -jar -DSERVER.PORT=$serverPort blueprinttemplate-0.0.1-SNAPSHOT.jar
