#!/usr/bin/env bash

echo "------------------- Going to load Tracker System docker images... -------------------"
docker load -i payment-system-images.tar.gz

echo "------------------- Going to put Tracker System up... -------------------"
docker-compose up -d