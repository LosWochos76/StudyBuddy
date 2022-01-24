#!/bin/sh

msbuild StudyBuddy.App.Android.csproj /verbosity:normal /t:Rebuild /t:PackageForAndroid /t:SignAndroidPackage /p:Configuration=Release
