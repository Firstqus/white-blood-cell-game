# 🦠 Immune Defense

> A 2D tower defense educational game about the human immune system — built with Unity.

![Unity](https://img.shields.io/badge/Unity-6000.2.9f1-black?logo=unity) ![C#](https://img.shields.io/badge/C%23-purple?logo=csharp) ![Platform](https://img.shields.io/badge/Platform-WebGL-blue) ![Type](https://img.shields.io/badge/Type-Educational%20Game-green)

---

## 🎮 Play Now

> 🔗 **[Play on itch.io](#)** ← *(add your link here)*

No download needed — runs in your browser via WebGL.

---

## 📖 About

**Immune Defense** is an educational tower defense game where players deploy real immune cells to fight off pathogens invading the body. Instead of just memorizing biology facts, players *apply* their knowledge in real time — making decisions under pressure the way your immune system actually does.

Built as a submission for the **TMH Innovation Competition**, the game is designed to make immunology engaging, accurate, and interactive for students.

---

## 🧬 How It Works

The game follows a **3-phase loop** that mirrors real immune response:

| Phase | Description |
|-------|-------------|
| 🔬 **Diagnose** | Answer biology questions to identify the incoming pathogen |
| ⚔️ **Deploy** | Place immune cells in lanes to intercept the infection wave |
| 📊 **Debrief** | Review performance — wrong answers in Diagnose create in-game penalties |

---

## 🔬 Immune Cells

| Cell | Role |
|------|------|
| 🟡 **Neutrophil** | Fast first responder, handles early-wave pathogens |
| 🟠 **Macrophage** | High durability, engulfs slower but tougher enemies |
| 🔴 **NK Cell** | Targets virus-infected cells, essential for late waves |

---

## ✨ Features

- Biology-accurate immune cell mechanics
- Wave progression that mirrors real infection timelines
- DiagnosisManager system — wrong answers feed into gameplay as penalties
- Lane-based tower defense with strategic depth
- Full audio across all scenes (BGM + SFX)
- Educational feedback loop between knowledge and gameplay outcome

---

## 🛠️ Tech Stack

- **Engine:** Unity 6000.2.9f1
- **Language:** C#
- **Architecture:** Manager-based pattern (WaveManager, LaneManager, BGMManager, DiagnosisManager, PathogenReachEnd)
- **Build Target:** WebGL

---

## 💡 What I Learned

- Designing educational games that require *application* of knowledge, not just recall
- Building a manager architecture in Unity with clean script communication
- Debugging complex multi-system interactions (wave spawning, audio, UI, scoring)
- Balancing gameplay difficulty with educational accuracy

---

## 🚀 Run Locally

> Requires **Unity 6000.2.9f1** or later.

```bash
git clone https://github.com/Firstqus/white-blood-cell-game.git
```

1. Open the project in Unity Hub
2. Load the main scene from `Assets/Scenes/`
3. Press **Play** in the Unity Editor

---

## 🏆 Built For

**TMH Innovation Competition** — submitted as an educational game prototype demonstrating immune system concepts through interactive gameplay.

---

## 👤 Developer

**First** — [@Firstqus](https://github.com/Firstqus) · [Portfolio](https://firstqus.github.io/Portfolio) **Get**   _ @konteeterrak(https://github.com/konteeterrak)
