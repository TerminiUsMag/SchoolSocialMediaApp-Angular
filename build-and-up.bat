@echo off
cd /d %~dp0
:: Step 1: Build Docker Compose services
echo Building Docker Compose services...
docker-compose -f .\docker-compose.yaml build

:: Step 2: Bring up Docker Compose services
echo Bringing up Docker Compose services...
docker-compose -f .\docker-compose.yaml up -d

echo Docker Compose services are up and running.
