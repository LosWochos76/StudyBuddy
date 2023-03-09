#!/bin/sh
ng build --configuration production

zip dist.zip dist/*
sftp root@www.gameucation.de <<EOF
put dist.zip
exit
EOF

ssh root@www.gameucation.de <<EOF
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
