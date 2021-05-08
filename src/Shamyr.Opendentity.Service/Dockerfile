#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Shamyr.Opendentity.Service/Shamyr.Opendentity.Service.csproj", "src/Shamyr.Opendentity.Service/"]
COPY ["Shamyr.Opendentity.Database/Shamyr.Opendentity.Database.csproj", "Shamyr.Opendentity.Database/"]
COPY ["lib/Shamyr.AspNetCore/Shamyr.AspNetCore.csproj", "lib/Shamyr.AspNetCore/"]
COPY ["lib/Shamyr/Shamyr.csproj", "lib/Shamyr/"]
COPY ["lib/Shamyr.Extensions/Shamyr.Extensions.csproj", "lib/Shamyr.Extensions/"]
RUN dotnet restore "src/Shamyr.Opendentity.Service/Shamyr.Opendentity.Service.csproj"
COPY . .
WORKDIR "/src/src/Shamyr.Opendentity.Service"
RUN dotnet build "Shamyr.Opendentity.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shamyr.Opendentity.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shamyr.Opendentity.Service.dll"]