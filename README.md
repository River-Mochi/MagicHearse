# Magic Hearse + Funeral Director

> Two modes:
> **Auto Clean** removes dead citizens instantly, or
> **Funeral Director** lets deathcare capacity be tuned.

---

## Overview

**Magic Hearse** helps with deathcare overload in two ways:

- **Auto Clean (Magic):** instantly removes dead citizens waiting for a hearse.
- **Funeral Director (Self Manage):** scales **deathcare facility prefabs**
	- processing, fleet, storage, hearse speed
	- optional max workers

Notes:
- **Auto Clean and Funeral Director are mutually exclusive** (turning one on turns the other off).
- No Harmony patches or reflection.
- Works with new and existing saves.

---

## Features

### Option 1: Auto Clean
| Feature | Description |
|---|---|
| **Enable Magic** | Removes citizens flagged **Dead + RequireTransport**. |

### Option 2: Funeral Director (Self Manage)
| Slider | What it changes |
|---|---|
| **Processing rate** | Crematorium processing speed multiplier |
| **Fleet size** | Max hearses per facility multiplier |
| **Cemetery storage** | Long-term storage capacity multiplier |
| **Hearse Speed** | Speed multiplier (road limits still apply) |
| **Max workers** | (Advanced) scales max workers for deathcare facilities (existing + new buildings) |

Includes:
- **Reset Sliders** button (sets sliders back to 100%)

**Worker note:** worker changes apply to **existing and new buildings** (one-shot refresh using the game’s worker calculation).  
**Compatibility note:** leave **Control max workers** OFF if another mod (ex: ConfigXML) should control worker counts.

---

## City Status (Options menu)

A lightweight status report in the Options UI:
- **Dead waiting** (needs hearse pickup)
- **Deaths/month** vs **Cremation max/mo**
- **Active assets:** hearses, buildings, cemetery use, empty graves, max workers

Performance note: status scanning happens **only while Options is open** (refresh ~ every 15 seconds).

---

## Languages
15 languages supported:
- Français, Deutsch, Español, Italiano, English
- 日本語, 한국어, Polski, Português (Brasil), 简体中文, 繁體中文
- Português (Portugal), Українська, Tiếng Việt, ไทย

---

## How It Works (short)

### Auto Clean
`MagicHearseSystem` scans citizens with `HealthProblem` flags **Dead + RequireTransport**, then adds `Deleted`.

### Funeral Director
`FuneralDirectorSystem` runs **on demand** (on load / when sliders change):
- Applies multipliers to prefab components
- Turning **Funeral Director OFF** restores vanilla values from authoring data

---

## Links
- GitHub: https://github.com/River-Mochi/MagicHearse
- Paradox Mods: https://mods.paradoxplaza.com/authors/River-mochi/cities_skylines_2?games=cities_skylines_2&orderBy=desc&sortBy=best&time=alltime

---

## Credits
- River-Mochi: author/maintainer
- Inspired by Wayz's original “Magical Hearse”
- yenyang: technical consultant
- Necko1996: testing and feedback
- StarQ: tech advisory
