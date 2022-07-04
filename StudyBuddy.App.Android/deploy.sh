#!/bin/sh

msbuild StudyBuddy.App.Android.csproj /verbosity:normal /t:Rebuild /t:PackageForAndroid /t:SignAndroidPackage /p:Configuration=Release

sftp root@gameucation.eu <<EOF
cd download
put ./bin/Release/de.gameucation-Signed.apk
exit
EOF