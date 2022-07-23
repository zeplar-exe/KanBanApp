FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["KanBanApp/KanBanApp.csproj", "KanBanApp/"]
RUN dotnet restore "KanBanApp/KanBanApp.csproj"
COPY . .
WORKDIR "/src/KanBanApp"
RUN dotnet build "KanBanApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "KanBanApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "KanBanApp.dll"]
