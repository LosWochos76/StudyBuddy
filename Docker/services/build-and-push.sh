#!/bin/sh

docker build . -f docker/services/Dockerfile -t stuckenholz/studybuddyservices:latest
docker login
docker push stuckenholz/studybuddyservices:latest
