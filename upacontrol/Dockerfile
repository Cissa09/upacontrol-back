#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:2.2 AS build
WORKDIR /src
COPY ["upacontrol/upacontrol.csproj", "upacontrol/"]
RUN dotnet restore "upacontrol/upacontrol.csproj"
COPY . .
WORKDIR "/src/upacontrol"
RUN dotnet build "upacontrol.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "upacontrol.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "upacontrol.dll"]