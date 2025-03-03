# System Symulacji Ośrodka Badań nad Chorobami

## Wstęp
Program symuluje system ośrodka badań nad chorobami, wykorzystująca **ASP.NET** jako backend API. Umożliwia zarządzanie użytkownikami, badaniami i ich wynikami w środowisku kontrolowanym. System obsługuje role użytkowników (Pacjent, Researcher, Administrator), a także umożliwia rejestrowanie oraz analizowanie wyników badań. Całość działa w oparciu o bazę danych **SQLite** i może być uruchomiona w kontenerze Docker.

## Wykorzystane technologie
- **.NET 8** – backend API
- **ASP.NET Core** – implementacja logiki
- **SQLite** – baza danych
- **Docker** – konteneryzacja aplikacji

## Model bazy danych
Model bazy danych obejmuje następujące encje:

- **User** – informacje o użytkownikach systemu
- **Research** – badania prowadzone przez ośrodek
- **Signed** – powiązania użytkowników z badaniami
- **Examination** – wyniki badań

Struktura bazy danych została zaimplementowana z użyciem **Entity Framework Core**.

## Instalacja
### 1. Wymagania
- **Docker**: Całość zostanie stworzona na kontenerze, zawierającym wszystkie potrzebne pakiety do działania.

### 2. Uruchomienie (Docker)
```sh
docker-compose up --build
```

## Struktura aplikacji
Aplikacja korzysta z trójwarstwowej architektury:

- **Kontrolery (`Controllers/`)** – obsługują żądania HTTP i delegują je do serwisów.
- **Serwisy (`Services/`)** – implementują logikę biznesową, zarządzają danymi.
- **Repozytoria (`Repositories/`)** – warstwa dostępu do bazy danych, operacje CRUD.

Założenie struktury to pozostawienie konkretnej logiki dla odpowiedniego modułu, bez większego wnikania w działanie niższego.

## Role użytkowników
Zaimplementowany system autoryzacji to **Identity** udostępniający system ról, przydatny przy weryfikacji dostępu:
- Pacjent (Patient): Uzytkownik powiązany z badaniami, o najniższych uprawnieniach
- Naukowiec (Researcher): Użytkownik zarządzający przepływem badań, posiadający wieksze uprawnienia
- Admin (Administrator): Użytkownik z największymi uprawnieniami, dbajacym o porządek i bezpieczeństwo, w tym tworzenie kont dla innych użytkowników, czy nadawanie uprawnień.

## Endpointy API
Każdy request jeżeli przejdzie przez warstwę kontrolera (część związana głównie z autoryzacją), zwróci response w postaci obiektu result:
```json
{
    "Success": true,
    "Message": "Result message",
    "Errors": null,
    "Data": {
        "id": 1,
        "Subject": "Research subject"
    }
}
```
Gdzie:
- Success: Czy zapytanie się powiodło
- Message: Informacja zwrotna
- Errors: Lista dodatkowych błędów
- Data: W przypadku pobierania danych, znajdą się tutaj

API zostało podzielone na cztery główne sekcje:

### 1. Identity
Zarządza użytkownikami: rejestracja, logowanie, role.

#### Endpointy:
| Nazwa                        | Endpoint                          | Wymagania                        | Opis |
|------------------------------|----------------------------------|---------------------------------|------|
| Rejestracja                  | `POST /api/identity/register`    | Wymagana rola `Administrator` | Tworzy nowe konto użytkownika. |
| Logowanie                    | `POST /api/identity/login`       | Brak autoryzacji | Loguje użytkownika i zwraca token JWT. |
| Wylogowanie                  | `POST /api/identity/logout`      | Wymagana autoryzacja | Wylogowuje użytkownika. |
| Zmiana hasła                 | `POST /api/identity/change-password` | Wymagana autoryzacja | Umożliwia zmianę hasła użytkownika. |
| Zmiana danych osobowych      | `PATCH /api/identity/change-personal-data` | Wymagana autoryzacja | Umożliwia zmianę imienia i nazwiska użytkownika. |
| Zmiana danych wrażliwych     | `PATCH /api/identity/change-sensitive-data` | Wymagana rola `Administrator` | Umożliwia zmianę adresu e-mail i numeru PESEL użytkownika. |
| Ustawienie roli              | `PUT /api/identity/set-role`     | Wymagana rola `Administrator` | Zmienia rolę użytkownika. |
| Pobranie listy użytkowników  | `GET /api/identity`              | Wymagana rola `Administrator` | Zwraca listę wszystkich użytkowników. |

#### POST /api/identity/register
- **Typ:** JSON
- **Opis:** Tworzy nowe konto użytkownika.
- **Request Body:**
```json
{
  "email": "user@example.com",
  "username": "user123",
  "password": "SecurePass123!",
  "name": "Jan",
  "surname": "Kowalski",
  "pesel": "12345678901",
  "role": "Researcher"
}
```

#### POST /api/identity/login
- **Typ:** JSON
- **Opis:** Logowanie użytkownika.
- **Request Body:**
```json
{
  "usernameOrEmail": "user123",
  "password": "SecurePass123!"
}
```

#### POST /api/identity/logout
- **Typ:** Brak
- **Opis:** Wylogowanie użytkownika.

#### POST /api/identity/change-password
- **Typ:** JSON
- **Opis:** Zmiana hasła użytkownika.
- **Request Body:**
```json
{
  "userId": "1",
  "oldPassword": "OldPass123!",
  "newPassword": "NewPass456!"
}
```

#### PATCH /api/identity/change-personal-data
- **Typ:** JSON
- **Opis:** Zmiana imienia i nazwiska użytkownika.
- **Request Body:**
```json
{
  "userId": "1",
  "name": "NoweImię",
  "surname": "NoweNazwisko"
}
```

#### PATCH /api/identity/change-sensitive-data
- **Typ:** JSON
- **Opis:** Zmiana e-maila i PESEL-u (Administrator).
- **Request Body:**
```json
{
  "userId": "1",
  "email": "newemail@example.com",
  "pesel": "09876543210"
}
```

#### PUT /api/identity/set-role
- **Typ:** JSON
- **Opis:** Ustawienie roli użytkownika (Administrator).
- **Request Body:**
```json
{
  "userId": "1",
  "role": "Administrator"
}
```

#### GET /api/identity
- **Typ:** GET
- **Opis:** Pobranie listy wszystkich użytkowników (Administrator).

### 2. Research
Obsługuje operacje na badaniach: dodawanie, edycja, usuwanie.

#### Endpointy:
| Nazwa                        | Endpoint                          | Wymagania                        | Opis |
|------------------------------|----------------------------------|---------------------------------|------|
| Dodanie badania              | `POST /api/research`            | Wymagana rola `Researcher` lub `Administrator` | Tworzy nowe badanie. |
| Aktualizacja badania         | `PATCH /api/research/{id}`      | Wymagana rola `Researcher` lub `Administrator` | Aktualizuje istniejące badanie. |
| Usunięcie badania            | `DELETE /api/research/{id}`     | Wymagana rola `Researcher` lub `Administrator` | Usuwa badanie o podanym ID. |
| Pobranie wszystkich badań    | `GET /api/research`             | Wymagana rola `Researcher` lub `Administrator` | Zwraca listę wszystkich badań. |
| Pobranie badania po ID       | `GET /api/research/{id}`        | Wymagana autoryzacja | Zwraca szczegóły konkretnego badania. |
| Zakończenie badania          | `PATCH /api/research/{id}/finish` | Wymagana rola `Researcher` lub `Administrator` | Oznacza badanie jako zakończone i zapisuje podsumowanie. |
| Aktualizacja wyników badania | `PATCH /api/research/{id}/results` | Wymagana rola `Researcher` lub `Administrator` | Aktualizuje wyniki badania. |

#### POST /api/research
- **Typ:** JSON
- **Opis:** Tworzy nowe badanie.
- **Request Body:**
```json
{
  "subject": "Nowe badanie nad wirusem X",
  "summary": "Opis badania i jego cel."
}
```

#### PATCH /api/research/{id}
- **Typ:** JSON
- **Opis:** Aktualizuje istniejące badanie.
- **Request Body:**
```json
{
  "subject": "Zaktualizowany temat",
  "summary": "Nowy opis badania."
}
```

#### DELETE /api/research/{id}
- **Typ:** Brak
- **Opis:** Usuwa badanie o podanym ID.

#### GET /api/research
- **Typ:** GET
- **Opis:** Pobiera listę wszystkich badań.

#### GET /api/research/{id}
- **Typ:** GET
- **Opis:** Pobiera szczegóły konkretnego badania.

#### PATCH /api/research/{id}/finish
- **Typ:** JSON
- **Opis:** Oznacza badanie jako zakończone i zapisuje podsumowanie.
- **Request Body:**
```json
{
  "resultSummary": "Badanie zakończone - wnioski..."
}
```

#### PATCH /api/research/{id}/results
- **Typ:** JSON
- **Opis:** Aktualizuje wyniki badania.
- **Request Body:**
```json
{
  "resultSummary": "Nowe wyniki badań..."
}
```

### 3. Signed
Rejestracja użytkowników na badania.

#### Endpointy:
| Nazwa                                      | Endpoint                                  | Wymagania                        | Opis |
|--------------------------------------------|------------------------------------------|---------------------------------|------|
| Zapisanie jako badacz                      | `POST /api/signed/researcher`            | Wymagana rola `Researcher` lub `Administrator` | Przypisuje użytkownika do badania jako badacza. |
| Zapisanie jako pacjent                     | `POST /api/signed/patient`               | Wymagana rola `Researcher` lub `Administrator` | Przypisuje użytkownika do badania jako pacjenta. |
| Usunięcie zapisu                           | `DELETE /api/signed/{signedId}`          | Wymagana rola `Researcher` lub `Administrator` | Usuwa przypisanie użytkownika do badania. |
| Pobranie przypisania po ID                 | `GET /api/signed/{signedId}`             | Wymagana rola `Researcher` lub `Administrator` | Pobiera szczegóły przypisania użytkownika do badania. |
| Wyszukiwanie przypisań po użytkowniku/badaniu/roli | `GET /api/signed/search` | Wymagana autoryzacja, dodatkowe ograniczenia dla zwykłych użytkowników | Wyszukuje przypisania na podstawie użytkownika, badania i roli. |
| Pobranie listy przypisań                   | `GET /api/signed/list`                   | Wymagana rola `Researcher` lub `Administrator` | Pobiera listę przypisanych użytkowników. |
| Pobranie własnych przypisań                | `GET /api/signed/my-list`                | Wymagana autoryzacja | Pobiera listę badań, do których użytkownik jest przypisany. |

#### POST /api/signed/researcher
- **Typ:** JSON
- **Opis:** Przypisuje użytkownika do badania jako badacza.
- **Request Body:**
```json
{
  "userId": "1",
  "researchId": "10"
}
```

#### POST /api/signed/patient
- **Typ:** JSON
- **Opis:** Przypisuje użytkownika do badania jako pacjenta.
- **Request Body:**
```json
{
  "userId": "2",
  "researchId": "10"
}
```

#### DELETE /api/signed/{signedId}
- **Typ:** Brak
- **Opis:** Usuwa przypisanie użytkownika do badania.

#### GET /api/signed/{signedId}
- **Typ:** GET
- **Opis:** Pobiera szczegóły przypisania użytkownika do badania.

#### GET /api/signed/search
- **Typ:** JSON
- **Opis:** Wyszukuje przypisania na podstawie użytkownika, badania i roli.
- **Request Body:**
```json
{
  "userId": "1",
  "researchId": "10",
  "role": "Researcher"
}
```

#### GET /api/signed/list
- **Typ:** GET
- **Opis:** Pobiera listę przypisanych użytkowników.

#### GET /api/signed/my-list
- **Typ:** JSON
- **Opis:** Pobiera listę badań, do których użytkownik jest przypisany.
- **Request Body:**
```json
{
  "active": true,
  "researchId": "10",
  "role": "Patient"
}
```

### 4. Examination
Obsługuje wyniki badań.

#### Endpointy:
| Nazwa                                      | Endpoint                                  | Wymagania                        | Opis |
|--------------------------------------------|------------------------------------------|---------------------------------|------|
| Dodanie badania                            | `POST /api/examination/add`              | Wymagana rola `Researcher` lub `Administrator` | Dodaje nowe badanie dla pacjenta. |
| Akceptacja badania                         | `PATCH /api/examination/accept/{examinationId}` | Wymagana autoryzacja | Akceptuje badanie przez użytkownika. |
| Anulowanie badania                         | `PATCH /api/examination/cancel/{examinationId}` | Wymagana autoryzacja | Anuluje zaplanowane badanie. |
| Ukończenie badania                         | `PATCH /api/examination/complete`        | Wymagana autoryzacja | Oznacza badanie jako ukończone i zapisuje raport. |
| Przełożenie badania                        | `PATCH /api/examination/reschedule`      | Wymagana autoryzacja | Zmienia termin badania. |
| Pobranie listy badań                       | `GET /api/examination/list`              | Wymagana rola `Researcher` lub `Administrator` | Pobiera listę badań na podstawie filtrów. |
| Pobranie szczegółowej listy badań          | `GET /api/examination/detailed-list`     | Wymagana rola `Researcher` lub `Administrator` | Pobiera listę badań na podstawie szczegółowych filtrów. |
| Pobranie badań pacjenta                    | `GET /api/examination/patient-examinations` | Wymagana autoryzacja | Pobiera badania przypisane do zalogowanego pacjenta. |
| Pobranie badań związanych z danym badaniem | `GET /api/examination/by-research/{researchId}` | Wymagana rola `Researcher` lub `Administrator` | Pobiera badania związane z konkretnym badaniem. |
| Pobranie badania po ID                     | `GET /api/examination/{examinationId}`   | Wymagana rola `Researcher` lub `Administrator` | Pobiera szczegóły konkretnego badania. |

#### POST /api/examination/add
- **Typ:** JSON
- **Opis:** Dodaje nowe badanie dla pacjenta.
- **Request Body:**
```json
{
  "patientUId": "2",
  "researchId": "10",
  "examDate": "2025-03-15T10:00:00Z"
}
```

#### PATCH /api/examination/accept/{examinationId}
- **Typ:** Brak
- **Opis:** Akceptuje badanie przez użytkownika.

#### PATCH /api/examination/cancel/{examinationId}
- **Typ:** Brak
- **Opis:** Anuluje zaplanowane badanie.

#### PATCH /api/examination/complete
- **Typ:** JSON
- **Opis:** Oznacza badanie jako ukończone i zapisuje raport.
- **Request Body:**
```json
{
  "examinationId": "5",
  "report": "Pacjent wykazuje poprawę."
}
```

#### PATCH /api/examination/reschedule
- **Typ:** JSON
- **Opis:** Zmienia termin badania.
- **Request Body:**
```json
{
  "examinationId": "5",
  "newExamDate": "2025-03-20T14:00:00Z"
}
```

#### GET /api/examination/list
- **Typ:** GET
- **Opis:** Pobiera listę badań na podstawie filtrów.

#### GET /api/examination/detailed-list
- **Typ:** GET
- **Opis:** Pobiera szczegółową listę badań na podstawie filtrów.

#### GET /api/examination/patient-examinations
- **Typ:** GET
- **Opis:** Pobiera badania przypisane do zalogowanego pacjenta.

#### GET /api/examination/by-research/{researchId}
- **Typ:** GET
- **Opis:** Pobiera badania związane z konkretnym badaniem.

#### GET /api/examination/{examinationId}
- **Typ:** GET
- **Opis:** Pobiera szczegóły konkretnego badania.
