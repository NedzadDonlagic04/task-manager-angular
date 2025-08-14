# Backend Setup

Once the repository is cloned and you're inside the **backend** directory, run the commands below:

```bash
dotnet restore
dotnet user-secrets init
dotnet user-secrets add "DATABASE_URL" "your database url goes here"
dotnet user-secrets add "Cors:AllowedOrigin" "http://localhost:4200" # This is the default server, you can change it however you like
dotnet run
