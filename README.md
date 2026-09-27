# Društvena mreža za poticanje socijalizacije ljudi
 Društvena mreža u vidu mobilne aplikacije s ciljem povezivanja ljudi na temelju geografske lokacije i zajedničkih interesa koja se sastoji od mobilne aplikacije kao klijenta i API kao poslužitelj.

 Aplikacija omogućuje korisnicima izradu osobnog profila, definiranje kakve vrste aktivnosti ih zanimaju, pronalazak i organizaciju aktivnosti, povezivanje s drugim korisnicima, slanje zahtjeva za prijateljstvo te          komunikaciju putem dopisivanja.

## Funkcionalnosti
 - Registracija i prijava korisnika
 - Autentifikacija i autorizacija korisnika
 - Verifikacija e-maila
 - Izrada i uređivanje korisničkog profila
 - Dodavanje i upravljanje interesima
 - Kreiranje i prihvaćanje aktivnosti
 - Kategorizacija aktivnosti
 - Pronalaženje i pregled aktivnosti
 - Povezivanje korisnika na temelju zajedničkih aktivnosti i lokacije
 - Slanje i prihvaćanje zahtjeva za prijateljstvo
 - Chat sobe i komunikacija između korisnika
 - Slanje poruka u stvarnom vremenu
 - Sustav sviđanja
 - Slanje e-mail obavijesti
 - Funkcionalnost zaboravljene lozinke
 - prikaz lokacije ostalih korisnika na karti
 - Slanje obavjesti
 - Prijava korisnika
 - Blokiranje korisničkog računa
 - Brisanje objava kako administrator
 - Micanje blokade korisničkog računa kao administrator

## Korištene tehnologija
### Backend
- C#
- ASP.NET Web Api
- Entity Framework
- ASP.NET Core Identity
- SQL Server
- Firebase

### Frontend
- .NET MAUI Blazor Hybrid
- HTML
- CSS
- JavaScript

## Arhitektura
Aplikacija prati principe Clean Architecture te je organizirana u slojeve koji omogućuju jasnu podjelu odgovornosti između pojedinih dijelova sustava

- **Domain** – sadrži domenske entitete
- **Application** – sadrži DTO objekte, sučelja, aplikacijske servise i logiku pojedinih slučajeva uporabe
- **Infrastructure** – zadužen je za pristup bazi podataka, Entity Framework Core, implementaciju servisa, generiranje autentifikacijski tokena, slanje e-mail poruka i komunikaciju s vanjskim sustavima
- **API** – sadrži REST API kontrolere i obradu HTTP zahtjeva
- **Client** – Blazor klijentska aplikacija i korisničko sučelje

## Baza podataka
Za pohranu podataka koristi se Microsoft SQL Server, dok se za pristup bazi i upravljanje podacima koristi Entity Framework Core.

Entity Framework Core koristi se za:
 - Generiranje tablica u bazi podataka
 - Definiranje odnosa između entiteta
 - Izradu i upravljanje migracijama
 - CRUD operacije
 - Izvršavanje upita nad bazom podataka

## Autentifikacija i sigurnost
Za autentifikaciju i upravljanje korisnicima koristi se ASP.NET Core Identity, dok se za autentifikaciju API zahtjeva koristi JWT token.
Nakon isteka JWT token kreira se novi pomoću tokena osvježavanja a isti token osvježavanja ne može biti korišten više puta.

## Svrha projekta
Projekt je razvijen kao praktičan projekt iz područja razvoja programske podrške te predstavlja implementaciju društvene mreže kao koncept ideje o povezivanju korisnika putem zajedničkih interesa i aktivnosti i samim time nije u potpunosti završen.

Tijekom razvoja primijenjeni su različiti principi razvoja softvera, uključujući slojevitu arhitekturu, rad s relacijskom bazom podataka, autentifikaciju i autorizaciju korisnika, razvoj API-ja, asinkrono programiranje, prijenos podataka u realnom vremenu te odvajanje odgovornosti između pojedinih dijelova sustava.

## Prikaz aplikacije
U nastavku su dodane slike glavnih dijelova korisničkog sučelja

### Ekran za registraciju
<img width="300" alt="Screenshot_2026-08-06-13-55-21-768_com companyname mobileclient" src="https://github.com/user-attachments/assets/3490b6e5-f61c-424a-8d4c-ca50f1b649f0" />

### Ekran za prijavu
<img width="300" alt="Screenshot_2026-08-06-13-55-40-357_com companyname mobileclient" src="https://github.com/user-attachments/assets/c6454276-5a0d-48c1-9db3-012d777fc3b4" />

### Karta koja prikazuje druge korisnike
<img width="300" alt="Karta korisnika" src="https://github.com/user-attachments/assets/e250a0b4-42ab-4c0f-b604-6523bae6acb8" />
<img width="300" alt="Karta korisnika" src="https://github.com/user-attachments/assets/70aa771a-c4eb-40d6-bb0b-79d89f0baf8a" />

### Dopisivanje
<img width="300" alt="Chat" src="https://github.com/user-attachments/assets/00fdfd41-bdfe-476c-955f-308d57f62251" />
<img width="300" alt="Chat" src="https://github.com/user-attachments/assets/09e4deb4-5e61-4215-b878-67cbe526b5fc" />

### Aktivnosti
<img width="300" alt="Aktivnosti" src="https://github.com/user-attachments/assets/3d91a412-8a73-4b21-a8dd-48a8d5399b94" />
<img width="300" alt="Aktivnosti" src="https://github.com/user-attachments/assets/7f31c0b7-ffc4-4651-b720-480dbe7420be" />

