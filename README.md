# Apocalyptic Cafe  (Midterm Milestone)

> Run a tiny cafe at the end of the world: farm coffee beans and fruit, serve survivors, and shoot the zombies that scare your customers away.

**Course:** Game Design & Development | **Engine:** Unity (2022.3 LTS or newer, Unity 6 works) | **Platform:** PC (keyboard)

## How to run
1. Install Unity Hub + a Unity 2022.3 LTS (or Unity 6) editor.
2. Clone or download this repository and add the project folder in Unity Hub (**Add > Add project from disk**), then open it.
   * If you are starting from an empty project instead: copy the `Assets/Scripts` and `Assets/Editor` folders into your project's `Assets` folder.
3. In the top menu click **Apocalyptic Cafe > Create Prototype Scene** (creates and saves `Assets/Scenes/Prototype.unity`).
4. Press **Play**. No other setup or imported art is needed - the level is built from Unity primitives when the game starts.

## Goal
* **Win:** serve 10 customers.
* **Lose:** cafe reputation drops to 0.
* Reputation goes **up** when you serve customers and **down** when a customer leaves angry or zombies are at the cafe.

## Controls
| Key | Action |
|---|---|
| W A S D / Arrow keys | Move |
| E | Interact: harvest a ripe plot / serve at the counter |
| SPACE (hold) | Shoot in the direction you are facing |
| R | Restart |

## How to play (1 minute)
1. Walk to a **brown Coffee plot** or **red Fruit plot** and press **E** to harvest (you can carry 3 of each).
2. Look at the customers' orders: Coffee = 1 bean, Fruit Juice = 1 fruit, Fruit Latte = 1 bean + 1 fruit.
3. Walk to the **counter** and press **E**. A customer's label turns **green** when you can serve them.
4. After about 25 seconds zombies appear and walk to the cafe. Face them and hold **SPACE** to shoot (3 hits each).

## Implemented vs. planned
| Feature | Status |
|---|---|
| Move, harvest, serve, shoot, win/lose, restart | Implemented |
| 3 order types, patience timers, reputation system | Implemented |
| Zombie spawning with rising difficulty | Implemented |
| On-screen HUD + tutorial hints | Implemented |
| Upgrades shop (faster plots, stronger gun, better machine) | **Planned** |
| Multiple days / levels, save system | **Planned** |
| Final art, animation, sound, music | **Planned** |
| Mouse aiming, ammo, zombie types | **Planned** |

## Scripts (all in `Assets/Scripts`)
`GameBootstrap` (builds the level) - `GameManager` (rules + HUD) - `PlayerController` - `FarmPlot` - `CafeCounter` - `Customer` - `Spawner` - `Zombie` - `Bullet` - `Interactable` - `GameInput` - `Prim`

## Credits
* All code and primitive-shape visuals in this repository are original work by our team.
* Design framework: Hunicke, LeBlanc, Zubek - *MDA: A Formal Approach to Game Design and Game Research* (2004).
* Inspiration (ideas only, nothing copied): Overcooked, Cook Serve Delicious, Plants vs. Zombies, Stardew Valley.
