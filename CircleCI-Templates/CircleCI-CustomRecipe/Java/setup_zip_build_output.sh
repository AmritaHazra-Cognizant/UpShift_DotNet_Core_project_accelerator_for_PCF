#!/bin/bash

set -e +x

pluginZipName='<Application Name>-0.0.1-SNAPSHOT.zip'

echo "Create package-output folder"
mkdir -p package-output
echo "Create temp_folder folder"
mkdir temp_folder

echo "---------------------Copying <Application Name> to package-output started---------------------"
find target -type f -name *$pluginZipName -exec cp "{}" temp_folder/$pluginZipName \;
echo "---------------------Copying <Application Name> to package-output completed---------------------"
echo
echo "Contents of temp_folder"
ls -lrt temp_folder/
echo
echo "---------------------Unzipping the zip file started---------------------"
echo
unzip temp_folder/$pluginZipName -d package-output
rm -rf temp_folder
echo "Contents of package-output/"
ls -lrt package-output/
echo "---------------------Unzipping the zip file completed---------------------"
echo

pushd package-output
    RED='\033[0;31m' # Red Color
    GREEN='\033[0;32m' # Green Color
    YELLOW='\033[0;33m' # Yellow Color
    BOLD='\033[1m' # No Color, but bold text
    NC='\033[0m' # No Color

  echo
	echo -e "\n${YELLOW}Contents of PluginManifest file, before modifications ...${NC}\n"
	cat PluginManifest.yml

	echo
	echo -e "\n${YELLOW}Checking the PluginManifest file contents ...${NC}\n"
	echo -e "\n${YELLOW}Removing the pluginSource from PluginManifest file ...${NC}\n"
	sed -i '/pluginSource/d' PluginManifest.yml
	echo -e "\n${YELLOW}Removing the pluginId from PluginManifest file ...${NC}\n"
	sed -i '/pluginId/d' PluginManifest.yml
	echo -e "\n${YELLOW}Removing the hash from PluginManifest file ...${NC}\n"
	sed -i '/hash/d' PluginManifest.yml

    echo "\n${YELLOW}Extracting the plugin name in manifest file ...${NC}\n"
    pluginName=$(awk '/name\s*:/' PluginManifest.yml | cut -d ':' -f2 | xargs)
    echo "\n${GREEN}plugin name - $pluginName${NC}\n"
    echo "\n${YELLOW}Computing the hash value ...${NC}\n"
    stringToHash="$pluginName$pluginSource$pluginId"
        saltAndPassword="$SALT$stringToHash"
    hash=$(printf "%s" "$saltAndPassword" | openssl dgst -sha256 -hex | awk '{print $2}')
    echo "\n${GREEN}hash - $hash${NC}\n"

    echo
	echo -e "\n${YELLOW}Modifying the PluginManifest file ...${NC}\n"
	echo "" >> PluginManifest.yml
	echo "pluginSource: $pluginSource" >> PluginManifest.yml
	echo "pluginId: $pluginId" >> PluginManifest.yml
	echo "hash: $hash" >> PluginManifest.yml
	echo -e "\n${GREEN}Done modifying PluginManifest.${NC}\n"

	echo
	echo -e "\n${YELLOW}Contents of PluginManifest file, after modifications ...${NC}\n"
	cat PluginManifest.yml
popd

apt update
apt install -y zip

pushd package-output
    zip -r <Application Name>_"${CIRCLE_BUILD_NUM}".zip .
popd

echo "---------------------Package output contents---------------------"
ls -lrt package-output/
echo "---------------------------------------------------------------"

echo "Done packaging"

exit 0
