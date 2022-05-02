#!/bin/bash
set -e

trap cleanup ERR TERM INT

echo "Building docker image"
docker build -f src/CarbonIntensity.App/Dockerfile -t app:latest .

echo "Running app"
docker run -p 5000:80 -p 5001:443 app:latest -d

echo "done"