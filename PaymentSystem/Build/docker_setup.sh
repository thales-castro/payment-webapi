#!/usr/bin/env bash

echo "------------------- Going to check if Docker is installed on server. -------------------"
if ! command -v docker &> /dev/null
then
    echo "------------------- Docker is not installed, going to install. -------------------"
    sudo apt-get update -y
    curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
    sudo add-apt-repository \
        "deb [arch=amd64] https://download.docker.com/linux/ubuntu \
        $(lsb_release -cs) \
        stable"
    sudo apt-get update -y
    sudo apt-get install docker-ce docker-ce-cli containerd.io -y
    sudo usermod -aG docker $USER
    echo "------------------- Docker successfully installed. -------------------"
else
    echo "------------------- Docker is already installed. -------------------"
fi


echo "------------------- Going to check if Docker Compose is installed on server. -------------------"
if ! command -v docker-compose &> /dev/null
then
    echo "------------------- Docker Compose is not installed, going to install. -------------------"
    sudo curl -L "https://github.com/docker/compose/releases/download/1.27.0/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
    sudo chmod +x /usr/local/bin/docker-compose
    echo "------------------- Docker Compose successfully installed and its version is: $(docker-compose --version). -------------------"
    echo "------------------- Obs.: If you want to use docker without sudo by CLI, this Bash need be restarted. -------------------"
else
    echo "------------------- Docker Compose is already installed. -------------------"
fi