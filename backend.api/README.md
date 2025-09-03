# Backend Setup

Once the repository is cloned and you're inside the **backend** directory, run the commands below:

```bash
dotnet restore
dotnet user-secrets init
dotnet user-secrets set "Database_Url" "your database url goes here"
dotnet user-secrets set "Cors:AllowedOrigin" "http://localhost:4200" # This is the default server, you can change it however you like
dotnet dev-certs https --trust
dotnet run
```

# Docker Setup

First create a file to store the environment variables, in my case I called it **.env.backend**, after creating it set the values for the variables defined below:

```
Database_Url=
Cors__AllowedOrigin=http://localhost:4200 # Default frontend server
ASPNETCORE_URLS=
ASPNETCORE_Kestrel__Certificates__Default__Password=<your_password>
ASPNETCORE_Kestrel__Certificates__Default__Path=
```

After this create a certificate:

```
mkdir certs
dotnet dev-certs https --export-path ./certs/certificate.pfx --password <your_password>
```

If done correctly, this should work.
