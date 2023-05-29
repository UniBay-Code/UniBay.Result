FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["example/UniBay.Result.ExampleApi/UniBay.Result.ExampleApi.csproj", "UniBay.Result.ExampleApi/"]
RUN dotnet restore "example/UniBay.Result.ExampleApi/UniBay.Result.ExampleApi.csproj"
COPY . .
WORKDIR "/src/UniBay.Result.ExampleApi"
RUN dotnet build "UniBay.Result.ExampleApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UniBay.Result.ExampleApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UniBay.Result.ExampleApi.dll"]
