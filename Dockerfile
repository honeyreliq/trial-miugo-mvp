FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env

RUN apt-get update -yq
RUN apt-get install curl gnupg -yq
RUN curl -sL https://deb.nodesource.com/setup_13.x | bash -
RUN apt-get install -y nodejs

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY ./IUGOCare.API/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . ./
RUN dotnet publish IUGOCare.sln -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 80
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "IUGOCare.API.dll"]
