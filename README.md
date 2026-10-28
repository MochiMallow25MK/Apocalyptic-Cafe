# Apocalyptic Cafe (Midterm Project Milestone)

> Run a tiny cafe at the end of the world: grow coffee beans and fruit, serve hungry survivors, and shoot the zombies that scare your customers away.

**Course:** [Game Design & Development / DMVR504] | **Instructor:** [Dr. Arafa Taher] | **Engine:** Unity 6 | **Platform:** PC (keyboard)

**Team:** Mohamed Zouhir Kazan, Nadeen Ahmed, Malak Ahmed, Umar Amin Tolba, Mahmoud Hesham Abdelraheim

**Demo video:** [link to 1-3 minute screen recording]

## How to run
1. Install Unity Hub and Unity 6 (2022.3 LTS or newer should also work).
2. Clone or download this repository.
3. In Unity Hub choose **Add > Add project from disk** and select the project folder.
4. Open the project, then open the scene `Assets/Scenes/ApocalypticCafe`.
5. Press **Play** and click once inside the Game window so it receives keyboard input.

If Unity cannot be opened, watch the demo video linked above.

## Goal
- **Win:** serve **10** customers.
- **Lose:** cafe reputation drops to **0**.
- Reputation goes **up** when you serve a customer. It goes **down** when a customer leaves angry or while zombies are attacking the cafe.

## Controls
| Key | Action |
|---|---|
| W A S D | Move |
| E | Interact: harvest a ripe plant, or serve a customer at the counter |
| SPACE (hold) | Shoot in the direction you are facing |
| R | Restart |

## How to play
1. Walk to a **coffee plant (brown)** or **fruit plant (red)** and press **E** to harvest. You can carry up to 3 beans and 3 fruit.
2. Read the customers' order labels above their heads:
   - Coffee = 1 bean ($4)
   - Fruit Juice = 1 fruit ($4)
   - Fruit Latte = 1 bean + 1 fruit ($9)
3. Go to the **counter** and press **E**. A customer's label turns **green** when you are carrying what they ordered.
4. After about 25 seconds zombies appear and walk to the cafe. Face them and hold **SPACE** (3 hits to kill). Each zombie at the cafe drains reputation until you shoot it.

## Game design summary
- **Genre:** top-down cafe / time-management with light survival defense
- **Hook:** farming, serving and defending all compete for the same limited time
- **Core loop:** harvest, carry, serve, get rewarded, deal with zombies, repeat
- **Design principles used:** clear goals and rules, meaningful choices, challenge and progression, feedback, pacing and onboarding
- Full details (MDA analysis, diagrams, playtesting) are in the Game Design Document submitted with this project.

## Project structure
`Assets/Scripts` contains 8 scripts:

| Script | Purpose |
|---|---|
| `Game` | Rules (reputation, coins, inventory, win/lose) and the on-screen HUD |
| `Player` | Movement, interaction and shooting |
| `Station` | Farm plots and the counter (choose the Kind in the Inspector) |
| `Customer` | Walks to a seat, waits with a patience timer, leaves happy or angry |
| `Spawner` | Spawns customers and zombies on timers; zombie waves get faster |
| `Zombie` | Walks to the counter and drains reputation |
| `Bullet` | Projectile that damages zombies |
| `Keys` | Keyboard input for both Unity input systems |

All visuals are Unity primitive shapes (cubes, spheres, capsules) with simple coloured materials.

## Implemented vs. planned
| Feature | Status |
|---|---|
| Move, harvest, serve, shoot, restart | Implemented |
| 3 order types, customer patience, reputation, coins | Implemented |
| Zombie waves with rising difficulty, win/lose screens | Implemented |
| HUD, floating labels, tutorial tips, feedback messages | Implemented |
| Upgrade shop (faster plants, stronger weapon, bigger bag) | **Planned** |
| Multiple days / levels and save system | **Planned** |
| More zombie types, mouse aiming, ammo | **Planned** |
| Final art, animation, sound and music | **Planned** |

## Credits and references
- All code and primitive-shape visuals in this repository were created by our team.
- Design framework: Hunicke, R., LeBlanc, M., and Zubek, R. (2004). *MDA: A Formal Approach to Game Design and Game Research.*
- Inspiration (ideas only, nothing copied): Overcooked, Plants vs. Zombies, Stardew Valley, Cook, Serve, Delicious!
- Third-party assets: None yet. 