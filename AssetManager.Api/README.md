# IT Eszköz- és Erőforrás-kezelő Rendszer (Backend)

ASP.NET Core Web API + Entity Framework Core + SQL Server (LocalDB).

## Amit előre telepíteni kell

- .NET 8 SDK: https://dotnet.microsoft.com/download
- SQL Server LocalDB (Visual Studio Community telepítővel automatikusan jön,
  vagy külön: "SQL Server Express LocalDB" csomag)
- (Az Angular részhez később) Node.js LTS + `npm install -g @angular/cli`

## 1. Csomagok visszaállítása és build

Nyisd meg ezt a mappát terminálban, majd:

```
dotnet restore
dotnet build
```

## 2. EF Core migráció (adatbázis létrehozása)

Ha még nincs telepítve az EF Core CLI eszköz:

```
dotnet tool install --global dotnet-ef
```

Ezután hozd létre az első migrációt és az adatbázist:

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

Ez létrehozza a `AssetManagerDb` adatbázist a LocalDB-n, az `appsettings.json`-ban
megadott connection string alapján.

## 3. Backend futtatása

```
dotnet run
```

A konzol kiírja, melyik porton fut (pl. `https://localhost:7001`).
Nyisd meg a `https://localhost:7001/swagger` címet - ott interaktívan
kipróbálhatod az összes végpontot Swagger UI-on keresztül, mielőtt
Postmanban vagy Angularból hívnád.

## 4. Tesztelés Postmanban

Hozz létre egy Postman collectiont az alábbi hívásokkal, majd exportáld
és tedd a repóba egy `/postman` mappába:

- `POST /api/employees` - alkalmazott létrehozása
- `POST /api/assets` - eszköz létrehozása (Status automatikusan InStock lesz)
- `POST /api/assets/{id}/assign` - body: `{ "employeeId": 1 }`
- `POST /api/assets/{id}/return`
- `GET /api/assets/{id}/history` - végignézed a napló bejegyzéseket

## 5. Angular kapcsolódás

A `frontend-starter` mappában van egy kész `asset.model.ts` és
`asset.service.ts`. Ha létrehoztad az Angular projektet:

```
ng new asset-manager-ui --routing --style=scss
cd asset-manager-ui
ng add @angular/material
```

Másold be a két fájlt a `src/app/` alá (pl. egy `services` mappába),
és importáld be az `HttpClientModule`-t az `app.module.ts`-be (vagy
standalone esetén az `app.config.ts`-be), hogy a service működjön.

Állítsd be a service tetején lévő `API_URL`-t a saját backended
tényleges portjára (amit a `dotnet run` kiírt).

## Következő lépések (lásd a build-tervet)

1. Employees és Inventory modulok Angular oldalon (táblázat + form)
2. Dashboard komponens összesítő számokkal
3. JWT alapú authentikáció + role-based guardok
4. xUnit tesztek a `Assign`/`Return` logikára
5. GitHub Actions workflow (build + teszt minden push-nál)
