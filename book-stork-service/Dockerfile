FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /repo

COPY BookStorkService.sln ./
COPY src/BookStork.Api/BookStork.Api.csproj                         src/BookStork.Api/
COPY src/BookStork.Application/BookStork.Application.csproj         src/BookStork.Application/
COPY src/BookStork.Domain/BookStork.Domain.csproj                   src/BookStork.Domain/
COPY src/BookStork.Infrastructure/BookStork.Infrastructure.csproj   src/BookStork.Infrastructure/

RUN dotnet restore

COPY src/ src/

RUN dotnet publish src/BookStork.Api/BookStork.Api.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BookStork.Api.dll"]
