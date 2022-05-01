#!/bin/bash
set -e

trap cleanup ERR TERM INT

echo "Building docker image"
docker build -f src/CarbonIntensity.App/Dockerfile . -t app

echo "Running app"
docker run app

echo "done"