# Backend Setup

Once the repository is cloned and you're inside the **backend** directory, run the commands below:

## Restore dependencies

```bash
dotnet restore
```

## Set user secrets

```bash
dotnet user-secrets init
dotnet user-secrets set "Database_Url" "your database url goes here"
dotnet user-secrets set "Cors:AllowedOrigin" "http://localhost:4200" # This is the default server, you can change it however you like
```

## Install and trust the HTTPS certificate

```bash
dotnet dev-certs https --trust
```

## Run the application

```bash
dotnet run
```

# Docker Setup

Before you start, you need to provide the credentials for the backend. For security, these are managed using an environment file.

## Configure environment variables

Create a file named **.env.backend** in the **backend.api** directory of the project. If you change the name of this file make sure you **DO NOT COMMIT THIS FILE TO GIT.**

Contents of the file should be in this format:

```
Database_Url=
Cors__AllowedOrigin=http://localhost:4200 # Default frontend server
ASPNETCORE_URLS=
ASPNETCORE_Kestrel__Certificates__Default__Password=<your_password>
ASPNETCORE_Kestrel__Certificates__Default__Path=
```

## Create and trust a certificate

```bash
mkdir certs
dotnet dev-certs https --export-path ./certs/certificate.pfx --password <your_password>
```
