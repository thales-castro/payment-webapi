#!/usr/bin/env bash

echo "------------------- Going to load Payment System docker images... -------------------"
docker load -i payment-system-images.tar.gz

echo "------------------- Going to put Payment System up... -------------------"
docker-compose up -d