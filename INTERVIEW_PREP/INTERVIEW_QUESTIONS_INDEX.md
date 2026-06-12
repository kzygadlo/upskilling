# 📚 INTERVIEW QUESTIONS INDEX - Complete Knowledge Base

> **Rozmowa**: 2h | **Format**: Teoretyczne pytania + soft skills | **Level**: Senior Tech Lead, 14 years experience

---

## 🎯 PYTANIA - PEŁNA LISTA

### **SEKCJA 0: SOFT SKILLS & DOŚWIADCZENIE** 
👉 [SOFT_SKILLS_ANSWERS.md](./SOFT_SKILLS_ANSWERS.md)

| # | Pytanie | Type |
|----|---------|------|
| 0.1 | Opisz swoje największe osiągnięcia w karierze | Experience |
| 0.2 | Z jakimi najtrudniejszymi problemami technicznymi się zmierzyłeś? | Problem-Solving |
| 0.3 | Jakie decyzje architektoniczne podjąłeś i dlaczego? | Architecture |
| 0.4 | Monolit czy mikroserwisy? Trade-offy? | System Design |
| 0.5 | Jak kierujesz zespołem? Jak motivujesz? | Leadership |
| 0.6 | Jak komunikujesz decyzje techniczne biznesowi? | Communication |
| 0.7 | Jaki był najtrudniejszy incident? Jak go resolve'łeś? | Problem-Solving |
| 0.8 | Jakie są Twoje golden rules w inżynierii? | Principles |
| 0.9 | Dlaczego wybrałeś daną technologię w swoich projektach? | Decision-Making |
| 0.10 | Jak analizujesz problemy wydajnościowe? | Troubleshooting |

---

### **SEKCJA 1: .NET C# FUNDAMENTALS**
👉 [1_DOTNET_FUNDAMENTALS.md](./1_DOTNET_FUNDAMENTALS.md)

#### Core Language Concepts
| # | Pytanie | Category |
|----|---------|----------|
| 1.1 | Czym są value types vs reference types? Jak to wpływa na pamięć? | Memory |
| 1.2 | Generics - czym są, po co się ich używa? | Language Feature |
| 1.3 | Nullable types - czym są i dlaczego są przydatne? | Language Feature |
| 1.4 | Czym są lambda expressions? Jak działają? | Language Feature |
| 1.5 | IEnumerable vs IQueryable - czym się różnią? | Collections |

#### Runtime & Memory Management
| # | Pytanie | Category |
|----|---------|----------|
| 1.6 | Stack vs Heap - czym są i co jest gdzie? | Memory |
| 1.7 | Garbage Collection - czym jest i jak działa? | Memory |
| 1.8 | Jaka jest rola CLR w .NET? | Platform |
| 1.9 | IDisposable - kiedy używać, jak używać? | Resource Management |
| 1.10 | Dispose() vs finalizer - czym się różnią? | Resource Management |

#### Async & Concurrency
| # | Pytanie | Category |
|----|---------|----------|
| 1.11 | async/await - czym to jest i jak działa? | Async |
| 1.12 | async Task vs async void - czym się różnią? | Async |
| 1.13 | Threads vs Tasks vs ThreadPool - czym się różnią? | Concurrency |
| 1.14 | Race conditions - czym są i jak ich unikać? | Concurrency |
| 1.15 | Deadlocks - czym są i jak je rozwiązywać? | Concurrency |

#### Collections & Data Structures
| # | Pytanie | Category |
|----|---------|----------|
| 1.16 | List vs Dictionary - różnice, kiedy co używać? | Collections |
| 1.17 | Hash table - jak działa wewnętrznie? | Data Structure |
| 1.18 | HashSet - czym jest i kiedy go używać? | Collections |
| 1.19 | Złożoność obliczeniowa - Big O notation basics | Performance |
| 1.20 | LINQ - czym jest i jaki ma wpływ na wydajność? | Query Language |

#### Modern C# Features
| # | Pytanie | Category |
|----|---------|----------|
| 1.21 | Records - czym są i do czego się ich używa? | Language Feature |
| 1.22 | Pattern matching - czym jest? | Language Feature |
| 1.23 | Nullable reference types - czym są (C# 8+)? | Language Feature |

---

### **SEKCJA 2: WEB API DESIGN**
👉 [2_WEB_API_DESIGN.md](./2_WEB_API_DESIGN.md)

#### HTTP Fundamentals
| # | Pytanie | Category |
|----|---------|----------|
| 2.1 | HTTP basics - request/response structure | Protocol |
| 2.2 | HTTP methods - GET, POST, PUT, DELETE, PATCH | Protocol |
| 2.3 | HTTP status codes - 2xx, 4xx, 5xx meanings | Protocol |
| 2.4 | Kiedy używać 400 vs 422 vs 404? | API Design |

#### REST API Design
| # | Pytanie | Category |
|----|---------|----------|
| 2.5 | REST principles - czym są? | Architecture |
| 2.6 | Routing - jak projektować URL structure? | API Design |
| 2.7 | DTOs - Data Transfer Objects, po co się ich używa? | Architecture |
| 2.8 | Serialization/Deserialization - czym to jest? | Communication |
| 2.9 | Error handling w API - jak to robić? | Error Handling |

#### ASP.NET Core Specifics
| # | Pytanie | Category |
|----|---------|----------|
| 2.10 | Controllers - architektura, routing, atrybuty | Framework |
| 2.11 | Dependency Injection w ASP.NET Core | DI |
| 2.12 | Middleware pipeline - czym jest i jak działa? | Framework |
| 2.13 | Filters - czym są (Authorization, Action, Exception)? | Framework |

#### API Security & Validation
| # | Pytanie | Category |
|----|---------|----------|
| 2.14 | JWT tokens - czym są i jak działają? | Security |
| 2.15 | OAuth - czym jest i jak działa? | Security |
| 2.16 | Input validation - czym jest i why important? | Security |
| 2.17 | CORS - czym jest i why enforce? | Security |
| 2.18 | SQL Injection protection - czym jest i how prevent? | Security |

#### API Versioning & Documentation
| # | Pytanie | Category |
|----|---------|----------|
| 2.19 | API Versioning - dlaczego i jak implementować? | Maintenance |
| 2.20 | Swagger/OpenAPI - czym jest i po co? | Documentation |

---

### **SEKCJA 3: ARCHITECTURE & SOLID**
👉 [3_ARCHITECTURE_SOLID.md](./3_ARCHITECTURE_SOLID.md)

#### Clean Architecture
| # | Pytanie | Category |
|----|---------|----------|
| 3.1 | Clean Architecture - 4 layers, czym są? | Architecture |
| 3.2 | Dependency Rule - czym jest i dlaczego ważna? | Architecture |
| 3.3 | Separation of Concerns - czym to jest? | Principle |
| 3.4 | Layered Architecture - pros/cons | Architecture |

#### SOLID Principles
| # | Pytanie | Category |
|----|---------|----------|
| 3.5 | SOLID - co to jest? (overview) | Principle |
| 3.6 | Single Responsibility Principle | SOLID |
| 3.7 | Open/Closed Principle | SOLID |
| 3.8 | Liskov Substitution Principle | SOLID |
| 3.9 | Interface Segregation Principle | SOLID |
| 3.10 | Dependency Inversion Principle | SOLID |

#### Design Patterns
| # | Pytanie | Category |
|----|---------|----------|
| 3.11 | Repository Pattern - czym jest, dlaczego używać? | Pattern |
| 3.12 | Factory Pattern - czym jest? | Pattern |
| 3.13 | Strategy Pattern - czym jest? | Pattern |
| 3.14 | Singleton Pattern - czym jest? | Pattern |
| 3.15 | Observer Pattern - czym jest? | Pattern |
| 3.16 | Adapter Pattern - czym jest? | Pattern |

#### Application Design
| # | Pytanie | Category |
|----|---------|----------|
| 3.17 | Services layer - czym jest i why use? | Architecture |
| 3.18 | DTOs vs Domain Models - czym się różnią? | Architecture |
| 3.19 | Validation - where should it happen? | Architecture |
| 3.20 | Error handling strategy - global approach | Architecture |

---

### **SEKCJA 4: DATABASE & SQL**
👉 [4_DATABASE_SQL.md](./4_DATABASE_SQL.md)

#### SQL Basics
| # | Pytanie | Category |
|----|---------|----------|
| 4.1 | SELECT, WHERE, GROUP BY basics | SQL |
| 4.2 | INNER JOIN vs LEFT JOIN - czym się różnią? | SQL |
| 4.3 | Aggregate functions - SUM, COUNT, AVG | SQL |
| 4.4 | Subqueries - czym są i kiedy używać? | SQL |
| 4.5 | CTEs (Common Table Expressions) - czym są? | SQL |
| 4.6 | Window functions - RANK(), ROW_NUMBER(), etc. | SQL |

#### Database Design
| # | Pytanie | Category |
|----|---------|----------|
| 4.7 | Primary Key - czym jest? | Database Design |
| 4.8 | Foreign Keys - czym są i why important? | Database Design |
| 4.9 | Relationships - one-to-many, many-to-many | Database Design |
| 4.10 | Normalization - czym jest i why important? | Database Design |
| 4.11 | Denormalization - trade-offs | Database Design |
| 4.12 | Transactions - czym są i ACID properties | Database Design |

#### Performance & Optimization
| # | Pytanie | Category |
|----|---------|----------|
| 4.13 | Indexes - czym są i jak działają? | Performance |
| 4.14 | Clustered vs Non-clustered indexes | Performance |
| 4.15 | Execution Plan - czym jest i how read? | Performance |
| 4.16 | Query Profiler - czym jest i why use? | Performance |
| 4.17 | Statistics - czym są i why important? | Performance |
| 4.18 | N+1 problem - czym jest i how fix? | Performance |
| 4.19 | Slow queries - how debug and optimize? | Troubleshooting |

#### EF Core & Data Access
| # | Pytanie | Category |
|----|---------|----------|
| 4.20 | DbContext - czym jest? | ORM |
| 4.21 | Migrations - czym są i how use? | ORM |
| 4.22 | Change Tracking - czym jest? | ORM |
| 4.23 | Lazy Loading, Eager Loading - czym się różnią? | ORM |
| 4.24 | Projections - czym są i why important? | ORM |
| 4.25 | Dapper vs EF Core - trade-offs | ORM |

---

### **SEKCJA 5: TESTING & DEVOPS**
👉 [5_TESTING_DEVOPS.md](./5_TESTING_DEVOPS.md)

#### Testing Fundamentals
| # | Pytanie | Category |
|----|---------|----------|
| 5.1 | Unit tests - czym są? | Testing |
| 5.2 | Integration tests - czym są? | Testing |
| 5.3 | E2E tests - czym są? | Testing |
| 5.4 | Test Pyramid - czym jest? | Testing Strategy |
| 5.5 | AAA pattern (Arrange, Act, Assert) | Testing |

#### Testing Tools & Techniques
| # | Pytanie | Category |
|----|---------|----------|
| 5.6 | xUnit - czym jest i how use? | Framework |
| 5.7 | Moq - mocking library, why use? | Mocking |
| 5.8 | Mocking - czym jest i kiedy używać? | Testing Technique |
| 5.9 | Test coverage - czym jest? | Metrics |
| 5.10 | TDD - Test Driven Development, czym jest? | Methodology |

#### DevOps & Cloud
| # | Pytanie | Category |
|----|---------|----------|
| 5.11 | Docker - czym jest i why use? | Containerization |
| 5.12 | Dockerfile - czym jest i how write? | Containerization |
| 5.13 | Docker Compose - czym jest? | Containerization |
| 5.14 | Kubernetes basics - czym jest (AKS)? | Orchestration |
| 5.15 | CI/CD - czym jest? | Automation |
| 5.16 | GitHub Actions / Azure Pipelines - how use? | Automation |
| 5.17 | Azure - App Service, SQL DB, Key Vault | Cloud |
| 5.18 | Infrastructure as Code (IaC) - basics | DevOps |

#### Monitoring & Logging
| # | Pytanie | Category |
|----|---------|----------|
| 5.19 | Logging best practices | Observability |
| 5.20 | Monitoring - metrics, alerts, dashboards | Observability |

---

## 📊 PODSUMOWANIE - LICZBA PYTAŃ

| Sekcja | Ilość pytań |
|--------|------------|
| 0. Soft Skills | 10 |
| 1. .NET C# Fundamentals | 23 |
| 2. Web API Design | 20 |
| 3. Architecture & SOLID | 20 |
| 4. Database & SQL | 25 |
| 5. Testing & DevOps | 20 |
| **RAZEM** | **118 pytań** |

---

## 🚀 KORZYSTANIE Z BAZY

### Dla danej kategorii:
1. Otwórz odpowiedni `.md` plik (np. `1_DOTNET_FUNDAMENTALS.md`)
2. Przeczytaj pytanie
3. Przeczytaj SHORT answer (quick refresh)
4. Przeczytaj FULL answer (głębia)
5. Robi notatki jeśli coś niejasne

### Format każdego pytania w plikach:
```
## Q: Pytanie?

**SHORT:** 1-2 zdania, esencja

**FULL ANSWER:**
- Pojęcie (CONCEPT/TECHNIQUE/PRINCIPLE)
- Wyjaśnienie
- Przykład lub diagram
- Key takeaways
```

---

## ⏱️ ROZMOWA - SCENARIUSZ

```
Rozmowa trwa 2h:

0-5 min: Small talk, intro
5-40 min: Soft skills (0.1-0.10) 
40-90 min: Technical questions (1-5)
90-120 min: More technical + design scenario

Pytania mogą być mix'em - interviewer może przeskoczyć między kategoriami.
```

---

## 📝 TWOJA STRATEGIA

1. **Dzień 1-2**: Przeczytaj SOFT_SKILLS_ANSWERS (zapamiętaj key points)
2. **Dzień 3-5**: Przeczytaj DOTNET + WEB_API (fundamentals)
3. **Dzień 6-7**: ARCHITECTURE + DATABASE (bardziej advanced)
4. **Dzień 8**: TESTING + DEVOPS (ostatnie rzeczy)
5. **Przed rozmową**: Quick review SOFT_SKILLS + najbardziej sprzedające odpowiedzi

---

**Następne kroki:** Otwórz odpowiednie `.md` pliki i czytaj! Jeśli coś niejasne → pisze CI feedback w danym pliku. 🚀
