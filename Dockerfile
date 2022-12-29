FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EmployeeManagement.WebApi/EmployeeManagement.WebApi.csproj", "EmployeeManagement.WebApi/"]
RUN dotnet restore "EmployeeManagement.WebApi/EmployeeManagement.WebApi.csproj"
COPY . .
WORKDIR "/src/EmployeeManagement.WebApi"
RUN dotnet build "EmployeeManagement.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeManagement.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
EXPOSE 80
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManagement.WebApi.dll"]