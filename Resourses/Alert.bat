@echo off
PowerShell -Command "Add-Type -AssemblyName PresentationFramework;[System.Windows.MessageBox]::Show('Please, use services.msc or sc.exe to control this service.')"