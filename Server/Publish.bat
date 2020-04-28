dotnet publish -c Release -r win-x64 --self-contained false
dotnet publish -c Release -r linux-x64 --self-contained false

dotnet publish -c Debug -r win-x64 --self-contained false
dotnet publish -c Debug -r linux-x64 --self-contained false
