#!/bin/sh

docker build --platform linux/amd64 . -f docker/api/Dockerfile -t stuckenholz/studybuddyservices:latest
docker login
docker push stuckenholz/studybuddyservices:latest
