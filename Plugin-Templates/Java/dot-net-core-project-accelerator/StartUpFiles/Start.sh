#!/bin/bash

serverPort=$1

echo "Starting app at port = "$serverPort

java -jar -DSERVER.PORT=$serverPort dot-net-core-project-accelerator-0.0.1-SNAPSHOT.jar
