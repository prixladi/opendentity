FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
ENV Logging__Console__FormatterName=Simple

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

# restore
WORKDIR /app/src
COPY src/Opendentity.Service/Opendentity.Service.csproj Opendentity.Service/
COPY src/Opendentity.OpenId/Opendentity.OpenId.csproj Opendentity.OpenId/
COPY src/Opendentity.Database/Opendentity.Database.csproj Opendentity.Database/
COPY src/Opendentity.Emails/Opendentity.Emails.csproj Opendentity.Emails/
COPY src/Opendentity.Domain/Opendentity.Domain.csproj Opendentity.Domain/

WORKDIR /app/lib
COPY lib/shamyr-core/src/Shamyr.Extensions/Shamyr.Extensions.csproj shamyr-core/src/Shamyr.Extensions/
COPY lib/shamyr-core/src/Shamyr/Shamyr.csproj shamyr-core/src/Shamyr/
COPY lib/shamyr-core/src/Shamyr.AspNetCore/Shamyr.AspNetCore.csproj shamyr-core/src/Shamyr.AspNetCore/

WORKDIR /app/src/Opendentity.Service
RUN dotnet restore Opendentity.Service.csproj

# build and publish
WORKDIR /app
COPY . .

WORKDIR /app/src/Opendentity.Service
RUN dotnet build Opendentity.Service.csproj -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Opendentity.Service.csproj -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Opendentity.Service.dll"]