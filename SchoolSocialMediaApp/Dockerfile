#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SchoolSocialMediaApp/SchoolSocialMediaApp.csproj", "SchoolSocialMediaApp/"]
COPY ["SchoolSocialMediaApp.Common/SchoolSocialMediaApp.Common.csproj", "SchoolSocialMediaApp.Common/"]
COPY ["SchoolSocialMediaApp.Core/SchoolSocialMediaApp.Core.csproj", "SchoolSocialMediaApp.Core/"]
COPY ["SchoolSocialMediaApp.Infrastructure/SchoolSocialMediaApp.Infrastructure.csproj", "SchoolSocialMediaApp.Infrastructure/"]
COPY ["SchoolSocialMediaApp.ViewModels/SchoolSocialMediaApp.ViewModels.csproj", "SchoolSocialMediaApp.ViewModels/"]
RUN dotnet restore "SchoolSocialMediaApp/SchoolSocialMediaApp.csproj"
COPY . .
WORKDIR "/src/SchoolSocialMediaApp"
RUN dotnet build "SchoolSocialMediaApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SchoolSocialMediaApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SchoolSocialMediaApp.dll"]