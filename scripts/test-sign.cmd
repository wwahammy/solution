@echo off
:test-sign.cmd
cd %~dp0

start /wait coapp-simplesigner --certificate-path=%~dp0\..\CoAppTest.pfx --password=password ..\..\output\any\test\bin\*.exe ..\..\output\any\test\bin\*.dll