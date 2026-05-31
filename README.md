TooliRent — API quick guide
===========================

Detta är en kort och enkel guide till de viktigaste API-endpointsen i projektet. Syftet är att ge snabba exempel för utveckling och test.

Basinfo
-------
- Kör projektet (standard Kestrel/HTTPS på utvecklingsprofil):

```bash
dotnet run --project TooliRent/TooliRent.csproj
```

- Standard-URL: `https://localhost:5001` (kan variera)

Arkitektur och design
----------------------
- N-tier separation:
  - Presentation/API: `TooliRent/Controllers` (API controllers)
  - Application/Services: `TooliRent.Services/Services` (affärslogik)
  - Domain: `TooliRent.Core/Models` och `TooliRent.Core/Interfaces` (entiteter och repository-kontrakt)
  - Infrastructure: `TooliRent.Infrastructure` (DbContext, migrations, repositories)
- Designmönster: Service pattern för affärslogik och Repository pattern för dataåtkomst.
- DTO & mapping: AutoMapper används (`TooliRent.Services/Mapping`) för att separera interna modeller från externa DTO:er.
- Validering: FluentValidation används för inkommande DTO:er (se `TooliRent.Services/Validators`).
- Autentisering: JWT Bearer + rollbaserad auktorisation (`Member`, `Admin`). Refresh-token sparas per användare.
- Databas: EF Core (code-first), migrations i `TooliRent.Infrastructure/Migrations` och seed data i `TooliRent.Infrastructure/Data/TooliRentDbContext.cs`.

Build, körning och Swagger
-------------------------
- Bygg och kör:

```bash
dotnet build
dotnet run --project TooliRent/TooliRent.csproj
```

- Swagger/OpenAPI: I utvecklingsläge finns Swagger UI på `/swagger` (t.ex. `https://localhost:5001/swagger`).
- XML-dokumentation för Swagger inkluderas automatiskt om projektet genererar XML (har satts på i csproj).

Autentisering
--------------
- Endpoints:
  - `POST /api/auth/register` — skapa konto.
  - `POST /api/auth/login` — login, returnerar JWT + refresh-token.
  - `POST /api/auth/refresh` — byt ut refresh-token mot ny JWT + ny refresh-token.
  - `POST /api/auth/logout` — logga ut (rensa refresh-token), kräver befintlig JWT.

Exempel (login):

```bash
curl -s -X POST https://localhost:5001/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"member","password":"Member#123"}'
```

Svarsexempel (avkortat):

```json
{
  "token": "<jwt>",
  "expiresUtc": "2026-05-30T12:00:00Z",
  "username": "member",
  "role": "Member",
  "refreshToken": "<refresh-token>",
  "refreshExpiresUtc": "2026-06-06T12:00:00Z"
}
```

Verktyg (Tools)
---------------
- `GET /api/tools` — lista verktyg.
  - Query-param: `category` (namn), `active` (true/false), `available` (true/false — ledig just nu).
- `GET /api/tools/{id}` — hämta ett verktyg.
- `POST /api/tools` — skapa nytt (Admin).
- `PUT /api/tools/{id}` — uppdatera (Admin).
- `DELETE /api/tools/{id}` — ta bort (Admin).

Exempel (lista lediga verktyg i kategori Garden):

```bash
curl -s "https://localhost:5001/api/tools?category=Garden&available=true"
```

Bokningar (Bookings)
---------------------
- `POST /api/bookings` — skapa bokning för en eller flera verktyg. Kropp innehåller `ToolIds`, `StartDate`, `EndDate`.
- `GET /api/bookings/mine` — lista egna bokningar (kan filtrera på status).
- `DELETE /api/bookings/{id}` — avboka (ägare eller admin).
- `POST /api/bookings/{id}/approve` — admin godkänner.
- `POST /api/bookings/{id}/pickup` — markera upphämtning (ägare eller admin).
- `POST /api/bookings/{id}/return` — markera återlämning (ägare eller admin).
- `POST /api/bookings/overdue/sweep` — admin: markera försenade.

Exempel (skapa bokning):

```bash
curl -X POST https://localhost:5001/api/bookings \
  -H "Authorization: Bearer <jwt>" \
  -H "Content-Type: application/json" \
  -d '{"toolIds":[1,2],"startDate":"2026-06-01T00:00:00Z","endDate":"2026-06-03T00:00:00Z"}'
```

Kategorier (Categories)
------------------------
- `GET /api/categories` — lista alla kategorier.
- `GET /api/categories/{id}` — hämta en kategori.
- `POST /api/categories` — skapa kategori (Admin).
- `PUT /api/categories/{id}` — uppdatera kategori (Admin).
- `DELETE /api/categories/{id}` — ta bort kategori (Admin).

Användare (Users) — Admin
-------------------------
- `GET /api/users` — lista användare (Admin).
- `POST /api/users/{id}/activate` — aktivera användare (Admin).
- `POST /api/users/{id}/deactivate` — inaktivera användare (Admin).

Statistik (Admin)
-----------------
- `GET /api/statistics` — returnerar totals, aktiva bokningar, försenade bokningar och top-verktyg.

Swagger
-------
- I utvecklingsläge finns Swagger UI aktiverat (vanligtvis `https://localhost:5001/swagger`). Använd för att utforska endpoints interaktivt.

Övergripande anteckningar
------------------------
- Alla skyddade endpoints kräver `Authorization: Bearer <jwt>` header.
- Refresh-token sparas per användare i databasen; anropa `/api/auth/refresh` med refresh-token för att få nytt access-token och ny refresh-token.
- Rollbaserad auktorisation: `Admin` för administrativa endpoints, `Member` för vanliga användare.