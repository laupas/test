#!/bin/sh
echo "# Install dotnet sdk"
curl -sSL https://dot.net/v1/dotnet-install.sh > dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh -c 5.0

echo "#show installed version"
dotnet --version

echo "#build"
dotnet publish -c Release -o output
