FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["MicroLoanApi/MicroLoanApi.csproj", "MicroLoanApi/"]
RUN dotnet restore "MicroLoanApi/MicroLoanApi.csproj"
COPY . .
WORKDIR "/src/MicroLoanApi"
RUN dotnet build "MicroLoanApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "MicroLoanApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "MicroLoanApi.dll"]