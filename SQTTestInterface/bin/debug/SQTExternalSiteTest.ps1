param(
[parameter(mandatory=$true)]
[string]$TestFromComputer,
[parameter(mandatory=$true)]
[Array]$SiteToConnectTo
)
[reflection.assembly]::LoadWithPartialName('System.Windows.Forms')
[reflection.assembly]::LoadWithPartialName('System.Drawing')
$logFile = ".\\ExternalSiteTestLog.txt"
Add-Type -AssemblyName System.Drawing
Add-Type -AssemblyName System.Windows.Forms
Write-Host ("Testing From: "+$TestFromComputer)
Write-Host ("Connecting To: "+$SiteToConnectTo)
Add-Content $logFile ("Testing From: "+$TestFromComputer) -Force
Add-Content $logFile ("Connecting To: "+$SiteToConnectTo) -Force
Read-Host -Prompt "Hit Enter To Continue-->>"