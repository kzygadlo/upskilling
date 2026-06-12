# 🎯 SOFT SKILLS & DOŚWIADCZENIE - Interview Answers

> **Context**: Senior Tech Lead, 14 lat doświadczenia w systemach finansowych. Rozmowa na ~2h, pytania teoretyczne + doświadczenie.

---

## 1️⃣ NAJWIĘKSZE OSIĄGNIĘCIA & PROJEKTY

### P: "Opisz swoje największe osiągnięcia w karierze"

**SHORT VERSION:**
Dwa kluczowe projekty: (1) AI-driven trade review automation w UBS, które zmniejszyło manual review effort z niemożliwych 2k dokumentów do możliwego do zarządzania workflow'u, i (2) Fund order execution system w Credit Suisse, gdzie focus był na 24/7 reliability w środowisku zintegrowanym z Bloomberg/SWIFT.

**FULL ANSWER:**

#### UBS - AI-Driven Trade Review Automation (2026)

**Problem:**
- 2,000+ term sheets do manual review przed deadline (2-3 miesiące)
- Każdy dokument w **różnej strukturze i formacie** - brak standardu
- Konieczna **głęboka wiedza biznesowa** (produkty, pojęcia, wyjątki)
- **Błędy = significant financial losses** (błędnie zabookowane wartości mogą kosztować miliony)
- **Zespół byłby physical impossible** to zrobić manual w terminie

**Rozwiązanie:**
Zaprojektowałem end-to-end AI pipeline:
- **Data preparation**: standaryzacja dokumentów z różnych formatów
- **AI/LLM prompt engineering**: custom prompt generujący spójną strukturę output'u dla wszystkich systemów
- **Validation layer**: 95% accuracy z preferencją dla **false negatives** (wątpliwe case'i trafiają do manual review)
- **System integration**: output integruje się z downstream validation systemami

**Impact:**
- ✅ **2k dokumentów processed** w timeline (inaczej byłoby undone + financial risk)
- ✅ **95% accuracy** - pozostałe 5% trafia do expert review
- ✅ **Risk mitigation**: uniknęliśmy potencjalnych strat (trudne do kalkulacji, ale znaczące)
- ✅ **Knowledge base**: SDM wiedzy biznesowej w formie prompt'u - reusable dla kolejnych systemów

**Key Learning:** 
*AI nie zastępuje ekspertów, ale amplifikuje ich - my byliśmy bottleneckiem, system zmienił to w bottleneck detection.*

---

#### Credit Suisse - Fund Order Execution System (2019-2026)

**Problem:**
- System pobierający ceny funduszy z Bloomberg'a i wysyłający zlecenia przez SWIFT
- **1000+ funduszy** - complex dependency tree
- Integracje z wieloma systemami: Bloomberg, SWIFT, market data feeds, trade database
- **Każda sekunda downtime'u = lost trades = financial loss**
- System **legacy**, budowany kilka lat wcześniej

**Rozwiązanie:**
Jako Tech Lead, przejąłem end-to-end ownership:
- **Stabilizacja**: systematic fix'owanie failure points
- **Monitoring**: proactive alerting (nie czekać aż się zepsuie)
- **Team development**: onboarding juniorów do skomplikowanej integracji (najtrудniejsza część)
- **Incident response**: 24/7 support mentality - ASAP resolution, post-mortem learnings

**Impact:**
- ✅ **Continuous availability**: system działał w production bez major outage'u (de facto 99.9%+ uptime)
- ✅ **Junior team competency**: 3 juniorów nauczeni o kompleksowych integracjach finansowych
- ✅ **Risk reduction**: żaden trade nie został lost z powodu systemu
- ✅ **Trust**: biznes polegał na systemie bez wątpliwości

**Key Learning:**
*W systemach finansowych reliability > features. 10 nowych featur'ów to nic vs jeden hour downtime'u.*

---

## 2️⃣ WYZWANIA TECHNICZNE & JAK ROZWIĄZAŁEŚ

### P: "Z jakimi najtrudniejszymi problemami technicznymi się zmierzyłeś?"

**SHORT VERSION:**
Cztery główne challenges: (1) prompt engineering dla heterogenicznych dokumentów w UBS, (2) multi-system integration reliability w Credit Suisse, (3) junior team development, (4) data quality w production.

**FULL ANSWER:**

### Challenge #1: LLM Prompt Engineering w Heterogenicznych Dokumentach

**Problem:**
- Term sheets w **różnych formatach** (PDF, scanned images, structured data, free text)
- **Brak konsystentnych struktur** - każdy template inny
- LLM **hallucinations** - model wymyślał wartości gdy nie było pewny
- Potrzebna **generic struktura output'u** dla multiple downstream systemów (porównanie z booking systemem, risk analysis, itp.)

**Jak rozwiązałem:**
1. **Iterative prompt refinement** z SME (Subject Matter Experts)
   - Zbierałem corner cases od biznesu
   - Testowałem prompt na real examples
   - Iterowałem dokimi prompt'u nie hallucinował bez sensu

2. **Confidence scoring** - zamiast binary result, model mówił jak confident jest
   - High confidence → auto-process
   - Low confidence → human review (95% accuracy target, ale preferujemy false negatives)

3. **Structured output** - zamiast free text, XML/JSON schema
   - Gwarantuje parseable output
   - Downstream systemy wiedzą dokładnie co dostaną
   - Easy to validate

4. **Data augmentation** - documentation na dokumenty w baseline
   - Standards dla common fields
   - Exception handling dla unique cases

**Learning:** *Prompt engineering to 70% engineering, 30% art. Bez domain knowledge i testing nie masz szansy.*

---

### Challenge #2: Multi-System Integration Reliability (Bloomberg + SWIFT + Internal Systems)

**Problem:**
- Fund order system zależy od **wielu external API's**: Bloomberg (price feeds), SWIFT (fund transfers), market data, trade DB
- Każdy system ma **own SLA, own failure modes**
- **Cascade failures**: Bloomberg jest down → system nie wie jakie ceny → nie może sendu trades
- Bez formal SLA, wszystko było reactive firefighting

**Jak rozwiązałem:**
1. **Dependency mapping** - narysowałem full dependency tree
   - Zaidentyfikowałem critical path
   - Znaleźliśmy single points of failure

2. **Graceful degradation**
   - Bloomberg down? Używamy cached prices (z delay)
   - Market data down? Używamy last known state
   - SWIFT down? Queue messages (retry later)
   - Zamiast: "system down" → "system operates in degraded mode"

3. **Monitoring & alerting**
   - Real-time dashboard (jest Bloomberg connection? są ceny? czy SWIFT responsuje?)
   - Alerts on:
     - Price staleness (> X minut old)
     - SWIFT latency (> Y ms)
     - Failed trades
   - Team mogł respond **before** customer notice

4. **Incident playbooks**
   - Każdy system down → co robimy? (documented)
   - Minimize MTTR (Mean Time To Recovery)
   - Post-mortems (why happened? jak prevent next time?)

**Learning:** *Production is not about perfection, it's about knowing when you're broken and recovering fast.*

---

### Challenge #3: Juniors w Skomplikowanej Integracji

**Problem:**
- 3 engineers (mostly juniors) musiało zarządzić systemem z **5+ integracjami**
- Bloomberg API, SWIFT message format, internal DB schema, trade workflow
- Brak senior'a do guidance → myself jako Tech Lead

**Jak rozwiązałem:**
1. **Knowledge transfer through ownership**
   - Każdy junior dostał **one integration** do own'owania (Bloomberg, albo SWIFT, albo market data)
   - Ja super-visor'owałem, ale oni byli responsible

2. **Incident as learning opportunity**
   - Incident happens → junior investigate + fix (ja watching)
   - Post-mortem → całe team uczy się
   - Next time similar issue → junior już wie

3. **Pair programming on critical path**
   - Nowe features → we robi razem (ja + junior)
   - Explaining not telling (pytam "why?", nie mówię "zrób to tak")

4. **Documentation**
   - Każdy junior pisał runbook dla своего system'u
   - Future team (albo inni) mogą recover bez mnie

**Learning:** *Best mentoring happens when juniors own something real, make mistakes, learn from them. Not in training sessions.*

---

### Challenge #4: Data Quality Issues

**Problem:**
- Term sheets w UBS: **każdy format, każda struktura** - brak standardizacji
- Data quality issues:
  - Scanned images (OCR errors)
  - Inconsistent field names (ten dokument: "Effective Date", inny: "Inception", trzeci: "Start Date")
  - Missing values
  - Contradictory values w różnych sekcjach

**Jak rozwiązałem:**
1. **Preprocessing pipeline**
   - Normalize formats (convert wszystko do parseable format)
   - OCR validation (check czy OCR result makes sense)
   - Field deduplication (znaleźć equivalent fields i unify)

2. **Validation rules**
   - Czy field ma sense? (date w przyszłości? amount negative?)
   - Czy dokument complete enough do processing? (vs too broken)
   - Czy wartości consistent z sobą?

3. **Fallback to human**
   - Low quality document? Human review
   - Ambiguous data? Confidence score low → human decides
   - Better safe than sorry (false negatives > false positives)

**Learning:** *Data quality is 80% of the problem. Model quality is 20%.*

---

## 3️⃣ DECYZJE ARCHITEKTONICZNE & DLACZEGO

### P: "Jakie architektoniczne decyzje podjąłeś i dlaczego?"

**SHORT VERSION:**
Dwie główne: (1) AI pipeline architecture w UBS (modular, reusable, validated), (2) Reliability-first design w Credit Suisse (degradation over failure).

**FULL ANSWER:**

### Decyzja #1: Modular AI Pipeline Architecture (UBS)

**Decyzja:** Rozdzielić prompt engineering → processing → validation → integration w **oddzielne, composable modules**

**Dlaczego CZASAMI BAD:**
- Wcześniej myślałem: "Single LLM call, output straight to system"
- Problem: hallucinations nie łapane, output format unparseable, hard to test

**Dlaczego GOOD:**
1. **Separation of concerns**
   - Prompt engineering = isolated module (easy to iterate)
   - Validation = separate (catches issues early)
   - Integration = separate (different downstream systems, different requirements)

2. **Testability**
   - Mogę test'ować każdy moduł niezależnie
   - Jeśli prompt fails → izolowane, nie cały system
   - Mogę mock LLM i test validation logic osobno

3. **Reusability**
   - Downstream system #1 (booking system) → inne validation rules
   - Downstream system #2 (risk analysis) → inne rules
   - Jeden prompt generator, różne validators
   - Skaluje się do wielu systemów

4. **Observability**
   - Widzę gdzie jest bottleneck (prompt quality? validation too strict? integration issues?)
   - Mogę iterować na każdej części niezależnie

**Trade-off:**
- More complex kod (vs single monolithic call)
- Więcej infrastructure (vs simple script)
- Worth it: flexibility + reliability > simplicity

---

### Decyzja #2: Graceful Degradation over Fail-Fast (Credit Suisse)

**Decyzja:** Jeśli Bloomberg down, system **pracuje w degraded mode** (cached prices), zamiast **fail fast** (system down).

**Dlaczego:**
1. **Financial domain**
   - Downtime = money loss
   - Trades are time-sensitive (market moves fast)
   - Better outdated prices niż żadne prices

2. **Integration reality**
   - 5+ external systems, każdy może fail
   - Nie mogę kontrolować Bloomberg uptime
   - Mogę kontrolować jak system reaguje na failure

3. **Business metric**
   - "System uptime 99%?" = Good
   - "System never failed completely?" = Excellent
   - Graceful degradation = almost never completely down

**Trade-off:**
- More complex code (fallback logic, cache management, state handling)
- Risk: cached prices mogą być stale (mitigation: age check, alert)
- Worth it: users prefer degraded system niż no system

---

### Decyzja #3: Team Ownership Model (Credit Suisse)

**Decyzja:** Zamiast "Tech Lead does everything", każdy junior own'uje **jeden system** (Bloomberg integration, SWIFT, market data, itp.)

**Dlaczego:**
1. **Juniors grow faster**
   - Real ownership > training sessions
   - Own incident response > code reviews
   - Learning through doing

2. **Distributed knowledge**
   - System doesn't depend on Tech Lead
   - If I'm on vacation, system still works
   - Succession planning (jeśli junior odchodzi, followup engineer might inherit role)

3. **Accountability**
   - Clear owner = faster decisions
   - Owner investigates incident first
   - Owner writes runbook

**Trade-off:**
- Risk: junior might make mistakes (mitigation: I'm supervising, architecture guards)
- Slower sometimes (junior doesn't move as fast as senior)
- Worth it: long-term resilience > short-term speed

---

## 4️⃣ MONOLIT VS MIKROSERWISY

### P: "Monolit czy mikroserwisy? Co by wybrał i dlaczego?"

**SHORT VERSION:**
Context matters. UBS? Monolit (focused, controlled). Credit Suisse? Hybrid (fund order = core system, Bloomberg adapter = separate, SWIFT adapter = separate). Rzeczywistość = tradeoff's między simplicity a flexibility.

**FULL ANSWER:**

### Kontekst #1: UBS Trade Review (AI System) → MONOLIT

**Architektura:**
```
Single cohesive application:
  Input (documents)
    ↓
  Preprocessing (normalize formats)
    ↓
  LLM Inference (prompt engineering)
    ↓
  Validation (check accuracy)
    ↓
  Output (structured JSON)
```

**Dlaczego monolit:**
1. **Clear data flow** - dokument wchodzi, wynik wychodzi
2. **Testability** - testuje jeden system, nie N systemów
3. **Deployment simplicity** - jedną versję deployuję
4. **Consistency** - jedna версja prompt'u dla wszystkich systemów
5. **Failure is contained** - jeśli coś fails, wiemy gdzie

**Kiedy by zmienił na mikro:**
- Gdybym miał **różne SLA** dla różnych document types (trade review vs regulatory docs)
- Gdybym miał **team per document type** (separate teams owning separate pipelines)
- Gdybym miał **independent scaling needs** (jedna część load-heavy, inna compute-heavy)
- W tym projekcie? Żaden z tych nie applies.

---

### Kontekst #2: Credit Suisse Fund Order System → HYBRID

**Architektura:**
```
Core: Fund Order System (monolit)
  ├─ Trade logic
  ├─ Order management
  └─ Trade database

Adapters (could be separate):
  ├─ Bloomberg Adapter (price feed)
  ├─ SWIFT Adapter (fund transfer)
  └─ Market Data Adapter
```

**Dlaczego hybrid:**
1. **Core is stable** - fund order logic nie zmienia się
2. **Adapters change frequently** - Bloomberg API updates, SWIFT changes
3. **Independent failure** - Bloomberg down ≠ core system down
4. **Different team ownership** - Bloomberg expert != SWIFT expert

**Trade-offs:**
| Aspect | Monolith | Microservices |
|--------|----------|---------------|
| Complexity | Simple | Complex |
| Deployment | Easy | Hard (N deployments) |
| Scaling | Monolithic | Independent |
| Team | Small | Large |
| Failure | Global | Isolated |

**Dla tego systemu:**
- Monolith by był simpler, ale bardziej fragile
- Micro by były flexible, ale overengineered (dla jednego systemu)
- **Hybrid = best of both:** core stable + adapters flexible

---

### General Rule:

**Monolit jeśli:**
- Zespół < 5 osób
- Domain = focused, clear
- Scaling = uniform (wszystko skaluje się razem)
- Deployment = rare changes

**Mikroserwisy jeśli:**
- Zespół > 10 osób (Conway's Law - system structure = team structure)
- Domain = multiple bounded contexts (financial domain, trade domain, reporting domain)
- Scaling = heterogeneous (trade system heavy, reporting light)
- Deployment = frequent changes w different parts

**W mojej karierze:** ~90% projektów = monolit. ~10% needed mikro. Większość mówi że chce mikro, ale doesn't need it.

---

## 5️⃣ LEADERSHIP & TEAM MANAGEMENT

### P: "Jak kierujesz zespołem? Jak motivujesz engineerów?"

**SHORT VERSION:**
Tech Lead mentality: (1) clear ownership, (2) learning opportunities, (3) psychological safety, (4) metrics that matter (reliability > velocity).

**FULL ANSWER:**

### Podejście do Leadership (Credit Suisse - 3 engineers, mostly juniors)

**Principle #1: Ownership Over Tasks**
- Nie: "Do this task" → task done, engineer czeka na next
- Tak: "Own this integration (Bloomberg). You're responsible for uptime, incidents, runbooks"
- **Impact:** Engineer rośnie. Cares about system. Learns through ownership.

**Principle #2: Learning Through Incidents**
- Incident happens (Bloomberg API changes, SWIFT timeout, etc.)
- Junior investigates (I supervise)
- We fix together (I ask questions, junior finds answers)
- Post-mortem (team learns)
- **Impact:** Junior now knows how to handle future incidents. Confident.

**Principle #3: Psychological Safety**
- "If you break something in production, that's learning"
- "Questions are welcome, no such thing as stupid question"
- "I make mistakes too, let's learn together"
- **Impact:** Juniors aren't afraid to try, to propose ideas, to say "I don't know"

**Principle #4: Metrics That Matter**
- NOT: "How many features did you ship?"
- YES: "What reliability improvements did you make? What did you learn? How did you grow?"
- **Impact:** Team optimizes for right thing (stability) not wrong thing (velocity)

**Principle #5: Transparent Tradeoffs**
- "We can't do feature X because we need to stabilize system Y. Here's why..."
- "This architectural decision favors reliability over simplicity. Here's the tradeoff..."
- "This is a compromise. We're not happy with it, but it's best we can do now."
- **Impact:** Team understands WHY, buys in to decisions.

---

### How I Communicate Bad News
- Early (don't wait for disaster)
- Honest (this is what happened)
- Actionable (here's what we do)
- Learning (how do we prevent next time?)

Example: "System had downtime yesterday. Bloomberg API changed undocumented. We didn't catch it. Here's what we learned, here's how we monitor for it next time."

---

## 6️⃣ KOMUNIKACJA Z BIZNESEM

### P: "Jak komunikujesz decyzje techniczne biznesowi? Jak convince ich do action?"

**SHORT VERSION:**
Always in business language, never technical jargon. Risk/cost/benefit, not features/architecture/technology.

**FULL ANSWER:**

### Framework: Risk-Based Communication

**Instead of:** "We need to refactor the fund order system, it's using WPF/WCF which is legacy"

**Say:** "Fund order system handles $XXM in trades daily. If it goes down for 1 hour, we lose ~$XXk. Right now we have single-threaded processing + synchronous SWIFT calls. If Bloomberg is slow or SWIFT has latency spike, whole system is at risk. Here's what I propose: [solution]. Cost: $XXk. Risk reduction: significant. Timeline: 3 months."

**Why this works:**
1. **Risk** - business understands risk language
2. **Dollar amount** - business understands cost/benefit
3. **Timeline** - business can decide if worth it
4. **Clear ask** - business knows what you need

### Example: UBS Trade Review

**Technical problem:** Prompt hallucinations, data quality issues

**Business communication:**
"We're processing 2k term sheets by deadline. Without AI tool, we'd need 20 people × 2 months = impossible + massive risk of missed errors = financial loss. With AI tool: 95% accuracy + 5% manual review. Cost: engineering time. Benefit: deadline met + risk mitigated. What we need: access to 5 SME's for 2 weeks = $XXk value to review edge cases. Worth it?"

**They said yes** because they understood what's at stake.

---

## 7️⃣ INCIDENT MANAGEMENT

### P: "Jaki był najtrudniejszy incident? Jak go resolve'łeś?"

**SHORT VERSION:**
Incident = cascade failure w Credit Suisse (Bloomberg stale prices + SWIFT delayed + juniors didn't know what to do). Resolved: systematic debugging, clear communication, learning.

**FULL ANSWER:**

### Scenario: Cascade Failure (Credit Suisse)

**Timeline:**
- 14:30: Bloomberg price feed stalls (no new prices for 5 mins)
- 14:31: System using old prices, sends trade orders at wrong price
- 14:32: SWIFT confirms orders, but at prices that are now far from market
- 14:33: Business realizes trades are bad
- 14:34: Panic. "System is broken!"

**Impact:**
- ~$2M in bad trades (prices moved in our against us)
- Customers angry
- System needs to be taken down immediately

**How I Handled It:**

1. **Immediate (First 30 mins)**
   - Stopped all trading (kill switch)
   - Assessed damage (which trades, which prices)
   - Communicated to business: "We've stopped system. Investigating now."
   - Assigned roles: Junior A = investigate Bloomberg, Junior B = SWIFT, Me = coordinate + business comms

2. **Root Cause Analysis (Next 2 hours)**
   - Bloomberg API changed (undocumented)
   - System didn't detect stale prices (fallback to cache, but cache age wasn't checked properly)
   - Juniors didn't know Bloomberg API had changed

3. **Fix (Next 4 hours)**
   - Patched price staleness check (alert if prices > 1 min old)
   - Added monitoring to detect API changes
   - Tested fallback behavior thoroughly
   - Deployed fix

4. **Learning (Next 2 days)**
   - Post-mortem: Why didn't we detect this?
   - Answer: Bloomberg doesn't notify of API changes
   - Solution: Monitor API health proactively
   - Training: Juniors learn about external API fragility
   - Documentation: How to handle Bloomberg failures (runbook)

**Key Decisions:**
- ❌ NOT: "Blame Bloomberg, we can't do anything"
- ✅ YES: "This is on us. How do we monitor external API health?"

**Outcome:**
- Juniors now own Bloomberg monitoring
- System has explicit price staleness check
- No similar incident since

**Learning:**
*Incidents are free education. You pay in $$, but you learn. If you handle well (fix + learn), it's worth it.*

---

## 8️⃣ DOBRE PRAKTYKI & PRINCIPLES

### P: "Jakie są Twoje golden rules w inżynierii?"

**SHORT VERSION:**
5 principles: (1) Reliability > Features, (2) Measure before optimizing, (3) Juniors grow through ownership, (4) Automate repetitive, (5) Communicate clearly.

**FULL ANSWER:**

### 1. **Reliability > Features** (especially in finance)

- One robust system > 10 buggy features
- Uptime = business metric
- Always ask: "Will this make system less reliable?" before shipping

### 2. **Measure Before Optimizing**

- Don't optimize gut feeling
- Monitor what matters: latency, throughput, uptime, error rate
- 80/20 rule: focus on top 20% problems causing 80% pain

### 3. **Juniors Grow Through Ownership**

- Give them real responsibility
- Incidents are learning ops (not disasters)
- Post-mortems are growth opportunities

### 4. **Automate Repetitive Tasks**

- Manual deployments? Automate
- Manual testing? Automate
- Manual monitoring? Alert-driven (auto-trigger runbook)
- Time saved = time for better things

### 5. **Communicate Early & Clearly**

- Issue discovered? Tell business immediately (not after panic)
- Architectural decision? Explain tradeoffs
- Mistake made? Own it, learn, share learning

---

## 🎓 CLOSING - KEY TAKEAWAYS

If asked "What's your biggest strength?":
> *I combine strong technical foundation with production mentality. I don't optimize for "most features shipped", I optimize for "reliable system that team understands and can maintain". In finance, that's what matters.*

If asked "What would you do differently?":
> *Credit Suisse: Earlier investment in monitoring & automation. We did firefighting for too long before building proper observability. UBS: Earlier integration with downstream systems - we iterated on prompt 10 times, would've been faster with target system feedback from day 1.*

If asked "What's next?":
> *Applied AI to mission-critical systems is fascinating. The learning curve is steep - data quality, hallucinations, prompt engineering are real engineering disciplines. I want to deep-dive into RAG pipelines and multi-agent systems. The technical bar is high, the business impact is huge.*

