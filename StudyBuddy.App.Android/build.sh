#!/bin/sh

msbuild StudyBuddy.App.Android.csproj /verbosity:normal /t:Rebuild /t:PackageForAndroid /t:SignAndroidPackage /p:Configuration=Release
cd bin/Release

jarsigner -verbose -sigalg SHA1withRSA -digestalg SHA1 -keystore ~/gameucation.keystore -signedjar de.gameucation.signed.aab de.gameucation.aab gameucation

cd ../..
