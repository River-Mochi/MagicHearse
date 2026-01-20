# Magic Hearse

> Two modes:
> **Auto Clean** removes dead citizens instantly, or
> **Funeral Director** lets you tune deathcare capacity.

---

## Overview

**Magic Hearse** helps with deathcare overload in two different ways:

- **Auto Clean (Magic)**: instantly removes dead citizens waiting for a hearse.
- **Funeral Director (Self Manage)**: scales deathcare prefab values (rates, fleet size, storage, hearse capacity).

Notes:
- **Auto Clean and Funeral Director are mutually exclusive** (enabling one disables the other).
- No Harmony patches or reflection.
- Works with new and existing saves.

---

## Features

### Option 1: Auto Clean
| Feature | Description |
|---|---|
| **Enable Magic** | Instantly removes dead citizens waiting for a hearse. |

### Option 2: Funeral Director (Self Manage)
| Slider | What it changes |
|---|---|
| **Processing rate** | Facility processing speed multiplier |
| **Fleet size** | Max hearses per facility multiplier |
| **Cemetery storage** | Cemetery long-term storage capacity multiplier |
| **Hearse capacity (Alpha)** | Updates hearse capacity on the prefab (not shown in game UI) |

Includes:
- **Reset Game Defaults** button (sets sliders back to 100%)

---

## Languages
11 languages supported (when locale files are enabled):
- Français, Deutsch, Español, Italiano, English
- 日本語, 한국어, Polski
- Português (Brazil)
- 简体中文, 繁體中文

---

## How It Works

### Auto Clean
`MagicHearseSystem` runs an ECS job that:
1. Scans citizens with `HealthProblem`
2. If a citizen is `Dead` and `RequireTransport`, it adds `Deleted`
3. The game removes those entities automatically

### Funeral Director
`FuneralDirectorSystem` runs **on demand** (on load / when sliders change):
- Applies multipliers to **deathcare + hearse prefabs**
- No per-frame runtime cost once applied

---

## Links
- GitHub: https://github.com/River-Mochi/MagicHearse
- Paradox Mods: https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime

---

## Credits
- River-Mochi: author/maintainer
- Thanks to Wayze, creator of the original “Magical Hearse”
- Necko1996: testing and feedback
