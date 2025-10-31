# Magic Hearse

> A simple *Cities: Skylines II* mod that makes dead citizens disappear automatically — no more waiting for hearses.

---

## 🧙‍♂️ Overview

**Magic Hearse** is a lightweight gameplay utility mod that instantly removes dead citizens without relying on cemetery or crematorium transport.  
It’s designed for performance and simplicity — no configuration beyond a single checkbox.

- ✅ Works automatically when enabled  
- ⚙️ Optional toggle in mod settings  
- 🚫 No Harmony patches  
- 🧩 Fully compatible with other mods  

---

## ⚙️ Features

| Feature | Description |
|----------|-------------|
| **Enable Magical Hearse** | Toggles whether the automatic cleanup system is active. |
| **Zero Maintenance** | No setup or dependencies required. |
| **Safe and Burst-compiled** | Uses Colossal’s ECS `GameSystemBase` and Burst jobs for clean execution. |

---


## 🧩 Technical Notes

- Built for **Unity 2022.x LTS / DOTS 1.0**  
- Targets **.NET Framework 4.8** and **C# 9**  
- Uses **Colossal’s official API** (`Game.Modding`, `Game.Systems`, `Game.Citizens`, etc.)  
- Logging through Colossal’s `ILog` (no custom logger)  
- Follows the formatting and style rules from [`general.instructions2.md`](general.instructions2.md)

---

## 🧠 How It Works

`MagicHearseSystem` runs a Burst-compiled ECS job that:

1. Scans all citizens with the `HealthProblem` component.  
2. If a citizen is both **Dead** and **RequiresTransport**, it adds the `Deleted` tag.  
3. The game engine then removes those entities automatically.  

This keeps city cleanup efficient without altering the core simul
Github repo: https://github.com/River-Mochi/MagicHearse
[Paradox Mods page](https://mods.paradoxplaza.com/authors/kimosabe1?orderBy=desc&sortBy=best&time=alltime)

---

## 📥 Credits
- River-Mochi - current author and maintainer
- Thanks to Wayze, the original author and pioneer of Magical Hearse
- Necko1996 - testing and feedback



