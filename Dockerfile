FROM microsoft/dotnet:2.0.0-preview1-sdk

WORKDIR /dotnetapp

# copy project.json and restore as distinct layers
COPY . .
RUN dotnet restore --configfile ./NuGet.Config

# copy and build everything else
#COPY . .
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "out/SampleAspNetCore-Server.dll"]
