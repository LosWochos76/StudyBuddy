#!/bin/sh
ng build --configuration production
firebase deploy

zip dist.zip dist/*
sftp root@gameucation.eu <<EOF
put dist.zip
exit
EOF

ssh root@gameucation.eu <<EOF
unzip dist.zip
cd backend
rm *
mv ../dist/* .
cd ..
rm dist.zip
rmdir dist
exit
EOF
 
rm dist.zip
