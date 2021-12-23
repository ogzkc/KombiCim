#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Kombicim.Mobile/Kombicim.Mobile.csproj", "Kombicim.Mobile/"]
COPY ["Kombicim.APIShared/Kombicim.APIShared.csproj", "Kombicim.APIShared/"]
COPY ["Kombicim.Data/Kombicim.Data.csproj", "Kombicim.Data/"]
RUN dotnet restore "Kombicim.Mobile/Kombicim.Mobile.csproj"
COPY . .
WORKDIR "/src/Kombicim.Mobile"
RUN dotnet build "Kombicim.Mobile.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kombicim.Mobile.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kombicim.Mobile.dll"]