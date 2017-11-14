# escape=`
FROM microsoft/aspnet:4.6.2
SHELL ["powershell","-Command","$ErrorActionPreference = 'Stop'; $ProgressReference = 'SilentlyContinue';"];

WORKDIR /install

ADD https://download.microsoft.com/download/C/9/E/C9E8180D-4E51-40A6-A9BF-776990D8BCA9/rewrite_amd64.msi rewrite_amd64.msi

RUN Write-Host 'Installing URL Rewrite';`
    Start-Process msiexec.exe -ArgumentList '/i', 'rewrite_amd64.msi', '/quiet', '/norestart' -NoNewWindow -Wait

RUN Remove-Website 'Default Web Site';

RUN New-Item -Path 'C:\Websites\Drawback' -Type Directory -Force;

RUN New-Website -Name 'Drawback' -PhysicalPath 'C:\Websites\Drawback';

EXPOSE 8090

COPY ["Drawback","/websites/Drawback"]

RUN $path ='C:\Websites\Drawback';`
	$acl= Get-Acl $path;`
	$newOwner =[System.Security.Principal.NTAccount]('BUILTIN\IIS_IUSRS');`
	$acl.SetOwner($newOwner);`
	dir -r $path | Set-Acl -aclobject $acl