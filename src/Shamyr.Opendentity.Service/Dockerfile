#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Shamyr.Opendentity.Service/Shamyr.Opendentity.Service.csproj", "Shamyr.Opendentity.Service/"]
RUN dotnet restore "Shamyr.Opendentity.Service/Shamyr.Opendentity.Service.csproj"
COPY . .
WORKDIR "/src/Shamyr.Opendentity.Service"
RUN dotnet build "Shamyr.Opendentity.Service.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Shamyr.Opendentity.Service.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shamyr.Opendentity.Service.dll"]