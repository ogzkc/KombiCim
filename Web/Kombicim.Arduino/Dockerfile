#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Kombicim.Arduino/Kombicim.Arduino.csproj", "Kombicim.Arduino/"]
COPY ["Kombicim.APIShared/Kombicim.APIShared.csproj", "Kombicim.APIShared/"]
COPY ["Kombicim.Data/Kombicim.Data.csproj", "Kombicim.Data/"]
RUN dotnet restore "Kombicim.Arduino/Kombicim.Arduino.csproj"
COPY . .
WORKDIR "/src/Kombicim.Arduino"
RUN dotnet build "Kombicim.Arduino.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Kombicim.Arduino.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Kombicim.Arduino.dll"]