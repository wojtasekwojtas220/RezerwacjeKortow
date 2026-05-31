# RezerwacjeKortów.pl

Projekt systemu rezerwacji kortów tenisowych wykonany w ASP.NET Core MVC.

## Wymagania
- .NET 8.0 SDK (lub nowszy)

## Jak uruchomić projekt
1. Sklonuj repozytorium:
   `git clone [LINK_DO_TWOJEGO_REPO]`
2. Wejdź do folderu projektu:
   `cd [NAZWA_FOLDERU]`
3. Przywróć pakiety i zaktualizuj bazę danych:
   `dotnet restore`
   `dotnet ef database update`
4. Uruchom aplikację:
   `dotnet run`
5. Otwórz przeglądarkę pod adresem: `http://localhost:5000`

## Opis technologii
- Backend: C#, ASP.NET Core 8.0 MVC
- Baza danych: SQLite (Entity Framework Core)
- Frontend: Bootstrap 5, JavaScript (AJAX)
