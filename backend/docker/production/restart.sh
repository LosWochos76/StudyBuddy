#!/bin/sh

docker pull stuckenholz/studybuddyadmin
docker pull stuckenholz/studybuddyservices
docker compose -f ./docker/production/docker-compose.yml restart