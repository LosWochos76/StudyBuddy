#!/bin/sh

docker build . -f docker/admin/Dockerfile -t stuckenholz/studybuddyadmin:latest
docker login
docker push stuckenholz/studybuddyadmin:latest
