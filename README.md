# Magic Hearse

> A simple *Cities: Skylines II* mod that makes dead citizens disappear automatically — no more waiting for hearses.

---

## Overview

**Magic Hearse Redux** is a mod that instantly removes dead citizens without relying on cemetery or crematorium transport.  
It’s designed for performance and simplicity — no configuration beyond a single checkbox.

- Works automatically when enabled  
- Optional toggle in mod settings  
- No Harmony patches or Reflection used (less likely to break on game updates)
- Fully compatible with other mods  

---

## Features

| Feature | Description |
|----------|-------------|
| **Enable Magic Hearse** | Toggles whether the automatic cleanup system is active. |
| **Works Automatically** | No setup or dependencies required. |


---

## How It Works

`MagicHearseSystem` runs a Burst-compiled ECS job that:

1. Scans all citizens with the `HealthProblem` component.  
2. If a citizen is both Dead and RequiresTransport, a `Deleted` tag is added. 
3. The game then removes those entities automatically.  

- Github repo: https://github.com/River-Mochi/MagicHearse
- [Paradox Mods page](https://mods.paradoxplaza.com/authors/kimosabe1?orderBy=desc&sortBy=best&time=alltime)


---

## Credits
- River-Mochi - current author/maintainer
- Thanks to Wayze, the original author and pioneer of "Magical Hearse" mod
- Necko1996 - testing and feedback



