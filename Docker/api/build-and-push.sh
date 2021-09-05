#!/bin/sh

docker build . -f docker/api/Dockerfile -t stuckenholz/studybuddyservices:latest
docker login
docker push stuckenholz/studybuddyservices:latest
