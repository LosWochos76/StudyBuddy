#!/bin/sh

sudo docker pull stuckenholz/studybuddyadmin
sudo docker pull stuckenholz/studybuddyservices
sudo docker-compose -f /home/stuckenholz/docker-compose.yml restart
