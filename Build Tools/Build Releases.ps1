$outputPath = Read-Host "Please enter the path to output builds to: "

cd ..\MinishCapRandomizerUI
dotnet publish .\MinishCapRandomizerUI.csproj -c Release -o $outputPath\UI\win-x64\ -r win-x64
dotnet publish .\MinishCapRandomizerUI.csproj -c Release -o $outputPath\UI\win-x86\ -r win-x86
dotnet publish .\MinishCapRandomizerUI.csproj -c Release -o $outputPath\UI\win-arm64\ -r win-arm64
cd ..\MinishCapRandomizerCLI
dotnet publish .\MinishCapRandomizerCLI.csproj -c Release -o $outputPath\CLI\win-x64\ -r win-x64
dotnet publish .\MinishCapRandomizerCLI.csproj -c Release -o $outputPath\CLI\win-x86\ -r win-x86
dotnet publish .\MinishCapRandomizerCLI.csproj -c Release -o $outputPath\CLI\win-arm64\ -r win-arm64
dotnet publish .\MinishCapRandomizerCLI.csproj -c Release -o $outputPath\CLI\win-arm\ -r win-arm
dotnet publish .\MinishCapRandomizerCLI.csproj -c Release -o $outputPath\CLI\linux-x64\ -r linux-x64
dotnet publish .\MinishCapRandomizerCLI.csproj -c Release -o $outputPath\CLI\linux-arm64\ -r linux-arm64
dotnet publish .\MinishCapRandomizerCLI.csproj -c Release -o $outputPath\CLI\linux-arm\ -r linux-arm
cd '..\Build Tools\' 
Write-Host "Done Building!" -foregroundcolor Green
Pause