FROM microsoft/aspnetcore-build as build

WORKDIR /code

COPY  . .

RUN dotnet restore

RUN dotnet publish --output /out/ --configuration Release

FROM microsoft/aspnetcore 

COPY --from=build /out /app/

WORKDIR /app

ENTRYPOINT [ "dotnet", "ProductCatalogApi.dll"]

