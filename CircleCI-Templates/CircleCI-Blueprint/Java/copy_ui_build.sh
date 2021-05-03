#!/bin/bash

#!/bin/bash

set -e +x
mkdir -p src/main/resources/static

echo "Contents of ui/dist/docker-image-finder/"
ls -lrt ui/dist/docker-image-finder/
echo "Contents of src/main/resources/static/"
ls -lrt src/main/resources/static/
echo "Copying to src/main/resources/static/"
cp -r ui/dist/docker-image-finder/* src/main/resources/static/

echo "---------------------src/main/resources/static/ contents---------------------"
ls -lrt src/main/resources/static/
echo "---------------------------------------------------------------"

exit 0