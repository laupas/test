#!/bin/sh
echo "# Install dotnet sdk"
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 5.0 -InstallDir ./dotnet5

echo "#ls"
ls -al

echo "#show installed version"
./dotnet5/dotnet --version

echo "#build"
./dotnet5/dotnet publish -c Release -o output

echo "test"
dotnet --version
