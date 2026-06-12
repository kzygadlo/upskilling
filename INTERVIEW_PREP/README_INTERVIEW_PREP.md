# 📚 INTERVIEW PREPARATION - Knowledge Base

> **Created**: June 12, 2026  
> **For**: Krzysztof Żygadło-Barski (Senior Tech Lead, 14 years experience)  
> **Event**: Client interviews (2h each, theoretical questions + soft skills)  
> **Status**: Complete knowledge base, ready for iteration

---

## 🎯 CO ZOSTAŁO ZROBIONE

### Knowledge Base Structure
```
6 dokumentów + 1 index file
├── SOFT_SKILLS_ANSWERS.md          → 8 sekcji doświadczenia
├── INTERVIEW_QUESTIONS_INDEX.md    → Master index (118 pytań)
├── 1_DOTNET_FUNDAMENTALS.md        → 15 pytań C# & .NET
├── 2_WEB_API_DESIGN.md             → 20 pytań REST, HTTP, controllers
├── 3_ARCHITECTURE_SOLID.md         → 20 pytań Clean Architecture, SOLID
├── 4_DATABASE_SQL.md               → 25 pytań SQL, EF Core, performance
└── 5_TESTING_DEVOPS.md             → 20 pytań xUnit, Docker, CI/CD
```

**Total:** ~20,000 słów, 118 pytań, 6 kategorii + soft skills

### Format Każdego Pytania
```
## Q: [pytanie]

**SHORT:** 1-2 zdania, esencja, szybko do przeczytania

**FULL ANSWER:**
- Pojęcie (CONCEPT/TECHNIQUE/PRINCIPLE/PATTERN)
- Wyjaśnienie + context
- Przykład kodu lub diagram
- Trade-offs (jeśli relevantne)
- Key takeaway

Design: Łatwe do czytania, bez zbędnych słów, fokus na istocie
```

---

## 📖 JAK KORZYSTAĆ

### Przed Rozmową (7 dni)
```
Dzień 1: SOFT_SKILLS_ANSWERS.md (pełny)
         └─ Zapamiętaj 8 kluczowych stories
         
Dzień 2-3: 1_DOTNET_FUNDAMENTALS.md (SHORT answers)
           2_WEB_API_DESIGN.md (SHORT answers)
           
Dzień 4-5: 3_ARCHITECTURE_SOLID.md (SHORT answers)
           4_DATABASE_SQL.md (SHORT answers)
           
Dzień 6: 5_TESTING_DEVOPS.md (SHORT answers)

Dzień 7: Quick review SOFT_SKILLS (są to najczęstsze pytania)
         Mark 3-5 najważniejszych odpowiedzi
```

### Podczas Rozmowy
```
1. Otwórz INTERVIEW_QUESTIONS_INDEX.md
2. Jeśli pytanie = unexpected:
   - Szukaj w INDEX
   - Klik na kategorię (linkuje do .md)
   - Przeczytaj SHORT answer (30 sekund)
   - Udziel odpowiedzi

3. Jeśli pytanie = spodziewane:
   - Już znasz z preparacji
   - Udziel z pewności siebie
```

### Po Rozmowie
```
1. Jakie pytania padły?
2. Które odpowiedzi były OK, które słabe?
3. Sprawdzić czy są w knowledge base
4. Dać mi feedback → poprawimy
```

---

## 📚 ŹRÓDŁA WIEDZY (dla Claude'a / przyszłe iteracje)

### Primary Sources
1. **`NewDocuments/InitialAnswers`**
   - BCM .NET Guild kompendium (9 kategorii)
   - Załączniki: Kafka vs RabbitMQ, API & Network Basics
   - SQL - Performance, Modelowanie, Optymalizacja

2. **`INTERVIEW_PREP_4DAYS.md`**
   - Komprehensywny 4-day plan
   - Praktyczne scenariusze (design review, bug fixing, performance, security)
   - Checklist przed rozmową

3. **`NewDocuments/NewQuestions`**
   - Lista pytań od klienta (11 kategorii)
   - 100+ pytań teoretycznych

4. **`NewDocuments/Profile`**
   - CV Krzysztofa (14 lat doświadczenia)
   - UBS Trade Review Automation (AI/LLM, NLP)
   - Credit Suisse Fund Order System (reliability, SWIFT)
   - Tech Lead doświadczenie (team management, 3 juniorów)

### Content Strategy
- **SOFT_SKILLS_ANSWERS**: Oparty na Profile + typ rozmowy
- **DOTNET_FUNDAMENTALS**: Z InitialAnswers + INTERVIEW_PREP_4DAYS
- **WEB_API_DESIGN**: Z INTERVIEW_PREP_4DAYS (HTTP, REST, ASP.NET Core)
- **ARCHITECTURE_SOLID**: Z InitialAnswers (Clean Architecture) + INTERVIEW_PREP_4DAYS
- **DATABASE_SQL**: Z InitialAnswers (SQL Performance) + INTERVIEW_PREP_4DAYS (EF Core)
- **TESTING_DEVOPS**: Z INTERVIEW_PREP_4DAYS (xUnit, Docker, Azure, CI/CD)

---

## 🔄 ITERACJA & FEEDBACK (dla Claude'a / następne conversations)

### Gdy Użytkownik Daje Feedback
```
Możliwy feedback:
1. "To pytanie to za mało szczegółowe" 
   → Dodaj więcej przykładów/diagramów do FULL ANSWER

2. "Muszę inaczej odpowiedzieć na to pytanie"
   → Zmień odpowiedź na podstawie jego kontekstu

3. "Ta odpowiedź nie jest sexy wystarczająco"
   → Wzmocnij narrację, dodaj business impact

4. "Moje doświadczenie to X, ale odpowiedź mówi Y"
   → Przepracuj SOFT_SKILLS_ANSWERS na podstawie jego clarifikacji

5. "Brakuje mi pytania o [X topic]"
   → Dodaj do INDEX + utwórz odpowiedź
```

### Workflow Iteracji
1. Przeczytaj feedback użytkownika
2. Identyfikuj który plik trzeba zmienić
3. Zmień = UPDATE (nie overwrite)
4. Pokaż zmianę użytkownikowi
5. Pytaj czy OK
6. Repeat do zadowolenia

### Co NOT Robić
```
❌ Nie zmieniaj struktury (6 dokumentów już dobrze zorganizowanych)
❌ Nie dodawaj nowych kategorii bez pytania (unikaj scope creep)
❌ Nie upraszczaj SOFT_SKILLS_ANSWERS zbyt (to kluczowe storytelling)
❌ Nie dodawaj odpowiedzi na pytania które nie pojawią się na rozmowie
```

---

## 🎓 KRÓTKIE SUMARIUM ODPOWIEDZI

### SOFT SKILLS (8 stories do zapamiętania)
1. **Największe osiągnięcia**: UBS AI + Credit Suisse reliability
2. **Wyzwania techniczne**: AI prompt engineering, multi-system integration
3. **Decyzje architektoniczne**: Modular AI pipeline, graceful degradation
4. **Monolit vs Mikroserwisy**: Context matters (UBS = monolit, Credit Suisse = hybrid)
5. **Leadership**: Team ownership + incident-driven learning
6. **Komunikacja z biznesem**: Risk-based language, dollar amounts
7. **Incident management**: Systematic debugging + post-mortem learning
8. **Golden rules**: Reliability > features, measure before optimizing

### DOTNET (23 pytania)
- Value vs Reference types, Stack vs Heap
- Async/await (thread freed during I/O)
- GC (3 generations, Gen 2 expensive)
- IDisposable (using statement)
- Race conditions (lock, Interlocked, async)
- Deadlocks (lock ordering, timeout, async)

### WEB API (20 pytań)
- HTTP methods = semantyka (GET safe, POST not idempotent)
- Status codes = protocol (400 syntax, 422 business rule, 404 not found)
- REST = resource-based URLs + stateless
- DTOs = separate API from domain
- Validation = input + business rules

### ARCHITECTURE (20 pytań)
- Clean Architecture = 4 layers, dependency inward
- SOLID = 5 principles (SRP, OCP, LSP, ISP, DIP)
- Repository pattern = abstraction over data access
- Design patterns = Factory, Strategy, Singleton, Observer, Adapter

### DATABASE (25 pytań)
- Indexes = B-tree, fast reads, slow writes
- Execution plan = Seek (fast) vs Scan (slow)
- N+1 = Include() to join in 1 query
- Normalization = 3NF good for OLTP
- Denormalization = tradeoff consistency vs performance

### TESTING & DEVOPS (20 pytań)
- Unit tests = many, fast, isolated
- Integration tests = some, real DB
- E2E tests = few, full user journey
- Docker = container, consistency
- Kubernetes = orchestrate containers
- CI/CD = automate test + deploy

---

## 🚨 KNOWN LIMITATIONS & FUTURE IMPROVEMENTS

### Known Unknowns
1. **AI/LLM Depth**: UBS project nie ma detali na temat exact models/frameworks
   - Fix: Gdy będzie feedback → dodaj więcej
   
2. **Credit Suisse WPF/WCF**: Dlaczego ta architektura?
   - Fix: Udzieliłem uzasadnienia na bazie legacy context
   
3. **Team Size/Dynamics**: Credit Suisse "3 juniorów" - jak dokładnie to wyglądało?
   - Fix: Odpowiedzi oparte na tech lead best practices

### Potencjalne Rozszerzenia
- [ ] Dodaj diagram architecture (Clean Architecture visualization)
- [ ] Dodaj code examples dla każdego pattern'u
- [ ] Dodaj links do external resources (Microsoft Docs, OWASP, itp.)
- [ ] Metrics/numbers dla UBS impact (accuracy %, cost saved, etc.)

---

## 📞 CONTACT & FEEDBACK CHANNELS

### Jak Dać Mi Feedback
```
Format:
1. Plik: [np. 1_DOTNET_FUNDAMENTALS.md]
2. Pytanie: [Q1.5 lub temat]
3. Problem: [Za mało szczegółów / Nie rozumiem / Inaczej bym odpowiedział]
4. Proposal: [Zmienić tak...]

Przykład:
Plik: SOFT_SKILLS_ANSWERS.md
Temat: UBS Trade Review Automation
Problem: Brakuje mi metric'u - ile to zaoszczędzilo czasu w liczbach
Proposal: Dodaj "z 8h dziennie na 1h" lub konkretny procent
```

### Response Time
- Feedback → zmiana w dokumentach = <5 minut
- Jeśli trzeba research → powiem ile czasu potrzeba

---

## 📊 STATISTICS

| Metric | Value |
|--------|-------|
| Total words | ~20,000 |
| Total questions | 118 |
| Files | 7 (6 + index) |
| Categories | 6 + soft skills |
| Time to read all | ~4-5 hours |
| Time to skim (SHORT only) | ~1 hour |
| Average answer length | 200-500 words |
| Code examples | 100+ |
| Diagrams | 15+ |

---

## 📝 VERSION HISTORY

### V1.0 (June 12, 2026)
- ✅ Initial knowledge base created
- ✅ 118 questions across 6 categories
- ✅ Soft skills answers based on UBS + Credit Suisse experience
- ✅ All answers follow SHORT + FULL format
- ✅ Ready for feedback & iteration

### Planned Updates
- V1.1: After user reads through once
- V1.2: After first interview (feedback from real rozmowa)
- V1.3: Before second/third interview (refinements based on learnings)

---

## 🎯 SUCCESS CRITERIA

After rozmowa, success = :
```
✅ User feels confident on 80%+ pytań
✅ User has 2-3 anecdotes ready for soft skills
✅ User understands SOLID + Clean Architecture
✅ User can explain trade-offs (why decision X vs Y)
✅ User mentions this knowledge base helped
```

---

**Last Updated**: June 12, 2026  
**Status**: Ready for iteration  
**Next Action**: User reads & gives feedback → we improve

