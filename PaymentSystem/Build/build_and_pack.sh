#!/usr/bin/env bash

############################################ Functions #############################################

showHelp() {
    echo "
    Please, run # $(basename "${0}") -o <OUTPUT PATH>

    -h, -help,          --help                  Display help

    -o, -output,        --output                Output path to generate the build"
}

################################# Args - Vars set in script call #################################

options=$(getopt -l "help,output:,clone:" -o "ho:" -a -- "$@")

eval set -- "$options"

while true; do
    case $1 in
    -h | --help)
        showHelp
        exit 0
        ;;
    -o | --output)
        shift
        output_path=$1
        ;;
    --)
        shift
        break
        ;;
    esac
    shift
done

if [[ -z ${output_path} ]]; then
    showHelp
    exit 1
fi

##################################################################################################

############################################## Main ##############################################

echo "------------------- Checking if the output path exists and cleaning... -------------------"

if [[ ! -d "$output_path" ]]; then
    echo "------------------- Output path does not exists, creating... -------------------"
    mkdir "$output_path"
else
    echo "------------------- Output path already exists, cleaning... -------------------"
    rm -rf "$output_path"/*
fi

cd ..

echo "------------------- Reseting repository & Updating with stable branch... ------------------- "
# git fetch
# git checkout -- .
# git checkout develop # or the release branch
# git pull

if ! command -v zip &> /dev/null
then
    echo "------------------- Going to install zip package... ------------------- "
    sudo apt update -y
    sudo apt install zip -y
fi

cd Build

echo "------------------- Going to copy necessary script files -------------------"
mkdir -p "$output_path"/tb
# copiar script de instalação de docker e docker compose
cp -f deploy_and_run.sh "$output_path"/tb

cd ../Docker

echo "------------------- Pulling databases docker images... -------------------"
docker pull mongo:latest
docker pull mongo-express

echo "------------------- Building services docker images... -------------------"
docker-compose build

echo "------------------- Saving images to compressed file... -------------------"
docker save paymentsystem/webapi mongo-express mongo -o "$output_path"/tb/payment-system-images.tar.gz

echo "------------------- Copying docker-compose of application... -------------------"
cp -f .env "$output_path"/tb
cp -f mongo-init.js "$output_path"/tb
cp -f docker-compose.yml "$output_path"/tb

echo "------------------- Compressing final file with all backend application content... -------------------"
cd "$output_path"/tb && zip -r "$output_path"/payment_base.zip . && cd - || exit 1

echo "------------------- Cleaning output path... -------------------"
rm -rf "$output_path"/tb