set PORT=%~1

echo "Starting app at port = " %PORT%

java -jar -Dserver.port=%PORT% blueprinttemplate-0.0.1-SNAPSHOT.jar
