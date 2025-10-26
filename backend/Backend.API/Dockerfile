FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ./backend.api.csproj ./
RUN dotnet restore
COPY ./ ./
RUN dotnet publish -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS app
WORKDIR /app
COPY --from=build /app .
COPY ./certs/certificate.pfx /https/certificate.pfx
RUN chown app:app /https/certificate.pfx && chmod 600 /https/certificate.pfx
USER app
EXPOSE 80
EXPOSE 443
ENTRYPOINT [ "dotnet", "./backend.api.dll" ]
