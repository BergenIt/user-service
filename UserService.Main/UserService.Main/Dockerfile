FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
RUN apt-get update && apt-get install -y libgdiplus
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["UserService.Main/UserService.Main/UserService.Main.csproj", "UserService.Main/UserService.Main/"]
COPY ["UserService.Data/UserService.Data/UserService.Data.csproj", "UserService.Data/UserService.Data/"]
COPY ["UserService.Core/UserService.Core/UserService.Core.csproj", "UserService.Core/UserService.Core/"]
COPY ["DatabaseExtension/DatabaseExtension.csproj", "DatabaseExtension/"]
RUN dotnet restore "UserService.Main/UserService.Main/UserService.Main.csproj"
COPY . .
WORKDIR "/src/UserService.Main/UserService.Main"
RUN dotnet build "UserService.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UserService.Main.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UserService.Main.dll"]
