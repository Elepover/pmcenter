#!/bin/bash
# pmcenter upgrader
#
# THIS SCRIPT COULD ONLY BE EXECUTED WHEN:
# - pmcenter is running and managed by systemd, service name "pmcenter".
# - you are executing this script in the directory where pmcenter.dll is in.
# - you have wget installed.
# - you have unzip installed.

echo "############### pmcenter upgrader ###############"
echo "# THIS SCRIPT COULD ONLY BE EXECUTED WHEN:      #"
echo "# - pmcenter is running and managed by systemd  #"
echo "#     with the service named \"pmcenter\"         #"
echo "# - you are executing this script in the        #"
echo "#     directory where pmcenter.dll is in.       #"
echo "# - you have wget installed.                    #"
echo "# - you have unzip installed.                   #"
echo "#                                               #"
echo "#          Copyright (C) 2018 Elepover          #"
echo "#################################################"
echo "==> Resuming in 5 seconds..."
sleep 5s

echo "==> Downloading binaries..."
wget https://ci.appveyor.com/api/projects/Elepover/pmcenter/artifacts/pmcenter.zip
echo "==> Extracting..."
unzip -o pmcenter.zip
echo "==> Removing unused files..."
rm pmcenter.zip
echo "==> Restarting service..."
systemctl restart pmcenter
echo "==> Complete!"
