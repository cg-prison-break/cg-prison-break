# 🏛️🚨 Prison Escape 3D ⛓️‍👮‍

Dieses Repository enthält das Prison-Escape-Projekt für das Modul Computer Games im Wintersemester 2025/2026  
Große Dateien (z. B. Texturen, Audio, Modelle) werden über **Git LFS (Large File Storage)** verwaltet.
Das Projekt basiert auf der URP.

---

## ⚙️ Voraussetzungen

Bevor du das Repository klonst, stelle sicher, dass folgende Tools installiert sind:

- [Git](https://git-scm.com/downloads)
- [Git LFS](https://git-lfs.com/)
- [Unity Hub](https://unity.com/download)
- **Unity-Version:** `Unity 6.2 (6000.2.9f1)`
- **Empfohlene IDE:** [Rider](https://www.jetbrains.com/rider/) oder [Visual Studio Code](https://code.visualstudio.com/)

> 💡 Hinweis: Über den **Unity Hub** lässt sich die korrekte Unity-Version bequem installieren und verwalten.

---

## 🧩 Einrichtung

1. **Repository klonen**
```bash
git clone <REPO_URL>
cd <PROJEKTNAME>
```

2. Git LFS initialisieren
Nach dem ersten Klonen des Repositories einmalig ausführen:

```bash
git lfs install
```

3. LFS-Dateien herunterladen
```bash
git lfs pull
```

4. Projekt in Unity öffnen
Öffne das Projekt anschließend in der passenden Unity-Version (siehe oben).

5. Standard Code-Editor eurer Wahl einstellen
`Edit > Preferences > External Tools > Dann euren Editor (VSCode, Rider, ...) einstellen`

---

## 🌿 Branching

Dieses Repository verwendet ein einfaches Main/Develop-Branching-Modell:

- `main`

    - Hauptbranch

    - Spiegelt immer den stabilen und getesteten Stand wider

    - Geschützt: Direkte Commits oder Pushes sind nicht erlaubt

- `develop`

    - Entwicklungsbranch

    - Dient als Sammelstelle für neue Features und Bugfixes

    - Geschützt: Nur Pull Requests dürfen gemergt werden
    - Wird außerdem bei **Reviews** genutzt, um neue Features am Freitag zu präsentieren

---

## ➕ Neue Branches anlegen

Neue Branches sollten immer von `develop` aus erstellt werden:

```git checkout develop
git pull
git checkout -b feature/<beschreibung>
```

Oder alternativ über eine UI deiner Wahl (VSCode, Rider, ...).

Beispiele:

- `feature/player-movement`
- `bugfix/ui-glich`
- `hotfix/crash-on-start`

Gerne regelmäßig pushen auf 

```bash
git push -u origin feature/<beschreibung>
```

Und bei fertigstellung einen Pull-Request zurück auf den `develop`-Branch erstellen.

---

## 🧠 Arbeiten ohne Mergekonflikte

Unity-Szenen-Dateien (`.unity`) können leicht Merge-Konflikte verursachen, wenn
mehrere Personen gleichzeitig an derselben Szene arbeiten.

Um das zu vermeiden, gilt folgende Regel:

- **Alle Szenen mit dem Suffix `TestScene.unity` werden nicht versioniert** (stehen in der `.gitignore`).
- Jeder Entwickler kann sich **eine eigene Testszene** anlegen, z.B.:

```markdown
Local_TestScene.unity
```

- Diese Test-Szenen dienen ausschließlich zum **lokalen Testen von Features**.
- Wenn ein Feature fertig ist, soltle es in eine gemeinsame Szene integriert werden, die versioniert wird (z.B. MainScene.unity)
- Die Integration in die MainScene.unity **muss** unter Absprache mit dem Map-Building-Team erfolgen
- Um eine neue Testszene zu erstellen, kann einfach die MainScene.unity kopiert und umbenannt werden

> 🔒 So bleiben alle produktiven Szenen konfliktfrei, und jeder kann unabhängig entwickeln und testen.

## 💡 Hinweise

Falls LSF-Dateien nicht automatisch geladen werden, führe manuell aus:

```bash
git lfs fetch
git lfs checkout
```

Prüfe bei Problemen mit großen Dateien, ob `git lfs` korrekt installiert und aktiviert ist.

---