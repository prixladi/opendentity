FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# restore
WORKDIR /app/src
COPY src/Shamyr.Opendentity.Service/Shamyr.Opendentity.Service.csproj Shamyr.Opendentity.Service/
COPY src/Shamyr.Opendentity.OpenId/Shamyr.Opendentity.OpenId.csproj Shamyr.Opendentity.OpenId/
COPY src/Shamyr.Opendentity.Database/Shamyr.Opendentity.Database.csproj Shamyr.Opendentity.Database/
COPY src/Shamyr.Opendentity.Emails/Shamyr.Opendentity.Emails.csproj Shamyr.Opendentity.Emails/

WORKDIR /app/lib
COPY lib/shamyr-core/src/Shamyr.Extensions/Shamyr.Extensions.csproj shamyr-core/src/Shamyr.Extensions/
COPY lib/shamyr-core/src/Shamyr/Shamyr.csproj shamyr-core/src/Shamyr/
COPY lib/shamyr-core/src/Shamyr.AspNetCore/Shamyr.AspNetCore.csproj shamyr-core/src/Shamyr.AspNetCore/

WORKDIR /app/src/Shamyr.Opendentity.Service
RUN dotnet restore Shamyr.Opendentity.Service.csproj

# build and publish
WORKDIR /app
COPY . .

WORKDIR /app/src/Shamyr.Opendentity.Service
RUN dotnet build Shamyr.Opendentity.Service.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Shamyr.Opendentity.Service.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shamyr.Opendentity.Service.dll"]