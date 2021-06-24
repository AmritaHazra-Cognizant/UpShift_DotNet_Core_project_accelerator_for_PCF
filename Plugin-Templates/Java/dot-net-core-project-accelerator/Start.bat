set PORT=%~1

echo "Starting app at port = " %PORT%

java -jar -Dserver.port=%PORT% dot-net-core-project-accelerator-0.0.1-SNAPSHOT.jar
