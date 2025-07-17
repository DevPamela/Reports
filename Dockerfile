# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia os arquivos da solução e projetos
COPY ["Reports.sln", "."]
COPY ["Reports.Application/Reports.Application.csproj", "Reports.Application/"]
COPY ["Reports.Domain/Reports.Domain.csproj", "Reports.Domain/"]
COPY ["Reports.Infrastructure/Reports.Infrastructure.csproj", "Reports.Infrastructure/"]
COPY ["Reports.Service/Reports.Service.csproj", "Reports.Service/"]
COPY ["tests/Reports.Service.Tests/Reports.Service.Tests.csproj", "tests/Reports.Service.Tests/"]
COPY ["tests/Reports.Application.Tests/Reports.Application.Tests.csproj", "tests/Reports.Application.Tests/"]
COPY ["tests/Reports.Tests.TestHelpers/Reports.Tests.TestHelpers.csproj", "tests/Reports.Tests.TestHelpers/"]

# Restaura pacotes
RUN dotnet restore "Reports.sln"

# Copia todo o código-fonte
COPY . .

# Build
WORKDIR "/src/Reports.Application"
RUN dotnet build "Reports.Application.csproj" -c Release -o /app/build

# Publish
FROM build AS publish
RUN dotnet publish "Reports.Application.csproj" -c Release -o /app/publish --no-restore

# Imagem final
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Porta exposta
EXPOSE 80

# Executa a aplicação
ENTRYPOINT ["dotnet", "Reports.Application.dll"]
