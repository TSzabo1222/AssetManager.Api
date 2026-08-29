# IT Asset & Resource Management System (Backend)

ASP.NET Core Web API + Entity Framework Core + SQL Server (LocalDB).

## Prerequisites

- .NET 8 SDK: https://dotnet.microsoft.com/download
- SQL Server LocalDB (comes automatically with Visual Studio Community, or install separately: "SQL Server Express LocalDB" package)
- (For the Angular part, later) Node.js LTS + `npm install -g @angular/cli`

## 1. Restore packages and build

Open this folder in a terminal, then:

```
dotnet restore
dotnet build
```

## 2. EF Core migration (create the database)

If the EF Core CLI tool isn't installed yet:

```
dotnet tool install --global dotnet-ef
```

Then create the initial migration and the database:

```
dotnet ef migrations add InitialCreate
dotnet ef database update
```

This creates the `AssetManagerDb` database on LocalDB, based on the connection string in `appsettings.json`.

## 3. Run the backend

```
dotnet run
```

The console will print which port it's running on (e.g. `https://localhost:7001`). Open `https://localhost:7001/swagger` — there you can interactively try out every endpoint via Swagger UI before calling it from Postman or Angular.

## 4. Testing in Postman

Create a Postman collection with the calls below, then export it and add it to the repo under a `/postman` folder:

- `POST /api/employees` — create an employee
- `POST /api/assets` — create an asset (Status will automatically be InStock)
- `POST /api/assets/{id}/assign` — body: `{ "employeeId": 1 }`
- `POST /api/assets/{id}/return`
- `GET /api/assets/{id}/history` — review the log entries

## 5. Connecting Angular

The `frontend-starter` folder contains a ready-made `asset.model.ts` and `asset.service.ts`. Once you've created the Angular project:

```
ng new asset-manager-ui --routing --style=scss
cd asset-manager-ui
ng add @angular/material
```

Copy the two files into `src/app/` (e.g. into a `services` folder), and import `HttpClientModule` into `app.module.ts` (or, for a standalone setup, into `app.config.ts`) so the service works.

Update the `API_URL` at the top of the service to match your backend's actual port (the one printed by `dotnet run`).

## Next steps (see the build plan)

1. Employees and Inventory modules on the Angular side (table + form)
2. Dashboard component with summary numbers
3. JWT-based authentication + role-based guards
4. xUnit tests for the `Assign`/`Return` logic
5. GitHub Actions workflow (build + test on every push)
