# Warehouse Management System (WMS) dla Firmy Budowlanej - Dokumentacja Techniczna

Pełna dokumentacja w pliku WMS-Dokumentacja-projektu.pdf

1.Autorzy Projektu Grupa K30
• Adam Jóźwiak
• Michał Jaworski


2.Krótki opis projektu
Warehouse Management System (WMS) dla Firmy Budowlanej to aplikacja
umożliwiająca efektywne zarządzanie magazynami materiałów budowlanych. Aplikacja
zrealizowana w ramach tego projektu będzie mogła służyć małym i średnim
przedsiębiorcom do zarządzania ich magazynami, śledzenia stanów produktów i
zapisywania wydań oraz przyjęć na magazyn. Aplikacja będzie zrealizowana w formie
aplikacji webowej, przy użyciu .NET 8 oraz języka C# . System pozwali na zarządzanie
magazynami, przyjmowanie do nich produktów oraz wydawanie tych produktów do
podwykonawców. Aplikacja pozwoli adminowi na wprowadzanie i śledzenie wszystkich
wydarzeń związanych z zarządzanymi przez niego magazynami.

3. Instrukcje pierwszego uruchomienia projektu
  1. Pobierz repozytorium:
    a. Sklonuj repozytorium z systemu kontroli wersji (np. Git) za pomocą polecenia:
      git clone https://github.com/AdusAdzik/WarehouseManagment.git
  2. Skonfiguruj bazę danych:
    a. Jeśli chcesz korzystać z własnej, niestandardowej bazy otwórz plik
          appsettings.json i zaktualizuj sekcję
    b. Wykonaj migrację update-database
  3. Przygotowanie migracji:
    a. Otwórz terminal w katalogu projektu i wykonaj polecenie:
    b. To polecenie utworzy i skonfiguruje schemat bazy danych zgodnie z definicjami w migracjach.
  4. Uruchom projekt
    a. Dostęp do aplikacji:
    b. Zarejestruj nowe konto użytkownika w aplikacji, klikając register w prawym górnym rogu.
    c. Po zalogowaniu uzyskasz dostęp do pełnej funkcjonalności systemu.
4. Opis struktury projektu
Struktura projektu jest zorganizowana zgodnie z konwencjami ASP.NET MVC. Główne foldery
projektu to:
• Controllers: Zawiera klasy kontrolerów odpowiadających za logikę biznesową aplikacji.
• Models: Przechowuje klasy modeli danych, w tym encje oraz klasy pomocnicze (np.
ViewModel).
• Views: Zawiera pliki widoków (.cshtml) do renderowania interfejsu użytkownika.
• wwwroot: Katalog publiczny, w którym znajdują się zasoby statyczne, takie jak pliki CSS,
JS i obrazy.
• Migrations: Przechowuje pliki migracji dla Entity Framework Core, które definiują zmiany
w schemacie bazy danych.
• Services: Przechowuje dodatkowe pliki pomocnicze, w naszym przypadku helper do
zarządzania sesją koszyka

5. Specyfikacja wykorzystanych technologii
Projekt został zbudowany z użyciem najnowszej stabilnej wersji platformy .NET 8.
Kluczowe technologie i narzędzia użyte w projekcie to:
• .NET 8: Framework do tworzenia aplikacji webowych w architekturze MVC (ModelView-Controller).
• Entity Framework Core: ORM (Object-Relational Mapping) do obsługi bazy danych.
• SQL Server: Relacyjna baza danych przechowująca dane aplikacji.
• ASP.NET Identity: Mechanizm uwierzytelniania i autoryzacji użytkowników.
• Bootstrap: Framework CSS do tworzenia responsywnych interfejsów użytkownika.
• SweetAlert2: Biblioteka JavaScript do dynamicznych powiadomień i formularzy
modalnych.
• jQuery: Biblioteka JavaScript do manipulacji DOM oraz obsługi zapytań AJAX używana przy zarządzaniu koszykiem produktów.

STRUKTURA PROJEKTU
| appsettings.Development.json
| appsettings.json
| Program.cs
| README.md
| WarehouseManagment.csproj
| WarehouseManagment.csproj.user
| WarehouseManagment.sln
|
+---Areas
| \---Identity
| \---Pages
| _ViewStart.cshtml
|
+---bin
| \---Debug
|
+---Controllers
| HomeController.cs
| ProductsController.cs
| SubcontractorsController.cs
| WarehouseInventoriesController.cs
| WarehousesController.cs
|
+---Data
| | ApplicationDbContext.cs
| |
| \---Migrations
| [ALL MIGRATION FILES]
| ApplicationDbContextModelSnapshot.cs
|
+---Models
| CartItem.cs
| DashboardViewModel.cs
| ErrorViewModel.cs
| Product.cs
| ProductLog.cs
| Subcontractor.cs
| SubcontractorLog.cs
| Warehouse.cs
| WarehouseEvent.cs
| WarehouseInventory.cs
| WarehouseLog.cs
|
+---obj
| |
| \---Debug
|
+---Properties
| launchSettings.json
| serviceDependencies.json
| serviceDependencies.local.json
| serviceDependencies.local.json.user
|
+---Services
| SessionExtensions.cs
|
+---Views
| | _ViewImports.cshtml
| | _ViewStart.cshtml
| |
| +---Home
| | Index.cshtml
| | Privacy.cshtml
| |
| +---Products
| | Create.cshtml
| | Delete.cshtml
| | Details.cshtml
| | Edit.cshtml
| | Index.cshtml
| |
| +---Shared
| | Error.cshtml
| | _Layout.cshtml
| | _Layout.cshtml.css
| | _LoginPartial.cshtml
| | _LogListPartial.cshtml
| | _ValidationScriptsPartial.cshtml
| |
| +---Subcontractors
| | Create.cshtml
| | Delete.cshtml
| | Details.cshtml
| | Edit.cshtml
| | Index.cshtml
| |
| +---WarehouseInventories
| | CartSummary.cshtml
| | ProductsForWarehouse.cshtml
| |
| \---Warehouses
| Create.cshtml
| Delete.cshtml
| Details.cshtml
| Edit.cshtml
| Index.cshtml
| Inventory.cshtml
|
\---wwwroot
 | favicon.ico
 |
 +---css
 | sidebar.css
 | site.css
 |
 +---js
 | site.js
 |
 \---lib
 +---bootstrap
 +---jquery
 +---jquery-validation
 +---jquery-validation-unobtrusive
 ---sweetalert
