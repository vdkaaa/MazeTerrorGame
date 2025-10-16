# MazeTerrorGame
A 3D psychological horror game built with **Unity 6 (URP)**.  

---

## ✅ Day 1 – Definition of Done

### 📂 Project Setup
- Unity 6 project created with **URP** assigned in *Graphics* and *Quality* settings.  
- Folder structure organized: **Core**, **Gameplay**, **Data**, **UI**, **Scenes**, **Prefabs**, **Settings**.  
- Git repository initialized with first commits.  

---

### 🎬 Scenes
- Created base scenes: `Boot`, `MainMenu`, `Labyrinth_Prototype`.  
- Build order set: **Boot → MainMenu → Prototype**.  
- Basic flow implemented:  
  - **Boot → MainMenu** via `BootStartup` + `SceneLoader`.  
  - **MainMenu** Play button loads **Prototype**.  

---

### ⚙️ Core Services (stubs)
- Interfaces defined:  
  - `IInputService`, `IAudioService`, `ISaveService`, `IEventBus`, `ITimeService`.  
- `EventBus` stub with `Publish`, `Subscribe`, `Unsubscribe` methods.  
- `AppInstaller` stub created to bootstrap global services.  

---

### 🧍 Player
- Interfaces defined:  
  - `IMovable`, `IInteractable`, `IInventory`, `IFlashlight`, `IDamageable`.  
- Stub components added:  
  - `PlayerMovement`, `PlayerInteractor`, `PlayerInventory`, `PlayerFlashlight`, `PlayerHealth`.  
- **PlayerRoot prefab** created with:  
  - Capsule + CharacterController.  
  - `CameraRig/MainCamera` hierarchy.  
  - Flashlight GameObject with Light component (disabled by default).  
  - Scripts added and references assigned.  
- PlayerRoot placed in `Labyrinth_Prototype`.  

---

### 📊 Data
- ScriptableObjects created for configuration:  
  - `PlayerConfig` (walkSpeed, runSpeed).  
  - `FlashlightConfig` (intensity, angle, drainPerSecond).  
- Default asset instances stored in `Data/ScriptableObjects/`.  

---

## ✅ Day 2 – Definition of Done (HUD wireframe)

---
### 🎨 HUD (event-driven, desacoplado)
- `Canvas_HUD` creado (Screen Space – Overlay) y guardado como **prefab** en  
  `Assets/_Project/Prefabs/UI/HUD/Canvas_HUD.prefab`.
- Estructura:
  - `Panel_Player` → `BatteryBar`, `HealthBar` (sliders normalizados 0..1).
  - `Panel_Time` → `ClockText` (mm:ss).
  - `Panel_Prompts` → `PromptText`.
- Scripts de widgets agregados:
  - `UIBattery`, `UIHealth`, `UITime`, `UIPrompt`.
- `UIManager` suscrito a `IEventBus`; actualiza widgets mediante eventos.
- DTOs de eventos creados:
  - `BatteryChanged(float normalized)`
  - `HealthChanged(float current, float max)`
  - `TimeTick(int minutes, int seconds)`
  - `ShowPrompt(string message, float duration)`
- **HUDEventSimulator** en escena (solo dev): publica eventos para prueba.
- Verificación:
  - Barras/textos se actualizan **con eventos simulados**.
  - `UIManager` **no** referencia Player ni lógica de juego (solo `IEventBus`).
  - Prefab sin referencias rotas en el Inspector.

### 🏗️ Level (Graybox + NavMesh + Anchors)
- Built initial graybox with modular **Room_A**, **Corridor_Straight**, and **Door_Generic** prefabs.
- Marked environment geometry as **Static** and placed under `Level/LabyrinthRoot`.
- Added **NavMeshSurface** (layer mask: Environment) and baked navigation mesh.
- Created **Anchors** for Puzzle, Loot, and Enemy spawns with editor **gizmos** (color-coded).
- Configured **Layers & Masks** (Environment, Interactable, Player, Enemy, Triggers).
- Verified scene smoke test: player can walk the layout; HUD still updates via events.

---

## ✅ Day 3 – Definition of Done

### 🎮 Player Movement
- Implemented **DevPlayerInputBridge** to feed input (WASD, mouse, Shift) into `PlayerMovement`.
- Refactored `PlayerMovement`:
  - Uses setters (`SetMoveInput`, `SetLookInput`, `SetRun`) instead of direct Input calls.
  - Added camera pitch rotation with clamped vertical look.
  - Cursor lock enabled during Play mode.
- Player can now walk and look around the labyrinth graybox.

---

### 🔦 Flashlight System
- Created `PlayerFlashlight` component:
  - Toggle on/off with **F** key.
  - Consumes battery over time (`drainPerSecond`).
  - Publishes `BatteryChanged` events to EventBus.
  - Auto-disables when battery is depleted.
- Integrated with HUD: battery bar now reflects real flashlight usage.

---

### ❤️ Health System
- Implemented `PlayerHealth` component:
  - Supports `TakeDamage` and `Heal`.
  - Publishes `HealthChanged` events to EventBus.
- Added `HUDHotkeys` (dev helper):
  - `H` → Take damage (-10).
  - `J` → Heal (+10).
  - `B` → Add battery (+10%).
  - `P` → Show prompt (“Picked up battery”).
- HUD health bar now responds to real player health.

---

### ⏱️ Time Manager
- Created `TimeManager` service:
  - Implements `ITimeService`.
  - Publishes `TimeTick` events every second.
- HUD clock updates automatically (mm:ss) during play.
- Removed reliance on `HUDEventSimulator` for time events.

---

### 🧪 Integration & Testing
- Verified full event-driven loop:
  - Walking and running work with WASD + Shift.
  - Flashlight drains battery; HUD battery bar reflects.
  - Health bar updates on damage/heal.
  - Clock runs in real time from 00:00.
  - Prompts display via dev hotkeys.
- Confirmed UI remains **decoupled** from gameplay scripts (uses only EventBus).



## ✅ Day 4 – Definition of Done  

### Enemy Dummy  
- **Implemented scripts** in `Scripts/Gameplay/Enemy/`:  
  - `EnemyBase` → encapsulates NavMeshAgent, references, and damage parameters.  
  - `EnemyChase` → minimal chase logic towards the Player.  
  - `EnemyDamageZone` → trigger that applies periodic damage to the Player with cooldown.  

- **Created prefab** in `Prefabs/Enemies/EnemyDummy`:  
  - Root with `NavMeshAgent`, `EnemyBase`, and `EnemyChase`.  
  - Child `DamageZone` with `SphereCollider (isTrigger)` + `EnemyDamageZone`.  
  - Red material for quick visualization.  

- **Scene integration** in `Labyrinth_Prototype`:  
  - NavMesh baked for the labyrinth.  
  - Enemy prefabs placed at test anchors.  
  - Player automatically recognized by `EnemyBase`.  

- **Validated behavior**:  
  - Enemy patrols the NavMesh and **chases the Player**.  
  - On contact, reduces the Player’s **Health bar** via `PlayerHealth` events.  
  - Damage respects **cooldown** to avoid spamming.  
  - No console errors; agent does not get stuck in the maze geometry.  

---

### Save & Load System  
- **GameState DTO** defined in `Scripts/Core/Services/Save/GameState.cs`:  
  - Stores Player health, max health, flashlight battery, position, and orientation.  

- **PlayerPrefsSaveService**:  
  - Serializes `GameState` into JSON and stores/loads it via PlayerPrefs.  
  - Methods: `SaveGame`, `TryLoadGame`, `DeleteSave`.  

- **PlayerStateAdapter**:  
  - Reads data from `PlayerHealth` and `PlayerFlashlight`.  
  - Applies state back to the Player on load, publishing events for the HUD.  

- **SaveLoadController**:  
  - Hotkeys: `F5` to save, `F9` to load.  
  - Feedback messages shown in HUD via `UIPrompt`.  
  - Includes cooldown to prevent spam.  

- **Multi–slot support (F1/F2/F3)**:  
  - `SaveLoadControllerMulti` allows switching the active slot with F1/F2/F3.  
  - Saves/loads to the currently active slot using F5/F9.  
  - Remembers the last active slot across sessions.  

- **HUD Overlay**:  
  - `UISaveSlotIndicator` displays the active slot in the corner.  
  - Updates via the `SaveSlotChanged` event.  
  - Includes fade-in/out animation when switching slots.  

- **Validated behavior**:  
  - `F1/F2/F3` switch active save slots and announce in the HUD.  
  - `F5` saves Player state (health, battery, position).  
  - `F9` loads Player state and the HUD reflects the changes.  
  - No console errors; save/load works across multiple slots.  


## ✅ Day 5 – Definition of Done  

### Inventory & Key–Locked Door System  

- **PlayerInventory** extended:  
  - Added `HasItem(string id)` to check if the player owns an item.  
  - Added `GetAllItems()` and `LoadFromList(List<string>)` for future Save/Load support.  

- **KeyItemPickup** script & prefab:  
  - Collectible object that adds a key (e.g., `"RedKey"`) to the player’s inventory.  
  - Publishes a `ShowPrompt` event (e.g., `"Picked up RedKey"`) for HUD feedback.  
  - Prefab created with collider + visuals.  

- **LockedDoor** script & prefab:  
  - Door requires a specific key (`requiredKeyId`) before it can be opened.  
  - Without the key → publishes `"The door is locked"`.  
  - With the key → unlocks and forwards to the door mechanics.  
  - HUD prompts fully integrated.  

- **DoorJoint** mechanics updated:  
  - Controlled via `Toggle()` / `Open()` / `Close()`.  
  - Uses Unity’s `HingeJoint` motor for smooth physical opening/closing.  
  - Starts locked by default (`IsLocked = true`).  
  - No longer bypasses lock when interacted directly.  

- **Scene integration** (`Labyrinth_Prototype`):  
  - A `KeyItemPickup` placed in the level.  
  - A `LockedDoor` blocks progression further inside the maze.  
  - Player cannot open the door without the key.  
  - After picking up the key, the door unlocks and opens correctly.  

- **Validation**:  
  - Prompts appear on HUD while looking at the door or picking up items.  
  - Door cannot be opened without the key.  
  - Once unlocked, door behaves consistently with hinge physics.  
  - No errors in console; interaction flow tested end-to-end.  


## ✅ Day 6 – Definition of Done  

### Enemy Patrol & Chase FSM  

- **EnemyState enum** added (`Idle`, `Patrol`, `Chase`) to formalize enemy logic.  

- **EnemyBase** extended:  
  - Holds reference to `NavMeshAgent` and target Player.  
  - Added `SetState()` to switch speed and behavior based on state.  
  - Implements lose-target timer to return from `Chase` to `Patrol`.  

- **EnemyDetector** component:  
  - Uses a `SphereCollider` trigger to detect Player presence.  
  - Optional line-of-sight raycast check from “eye” transform.  
  - Publishes detection info to EnemyBase.  

- **EnemyPatrol** component:  
  - Handles waypoint navigation (loop or ping-pong).  
  - Draws Gizmos for patrol paths in the editor.  
  - Works independently of chase logic.  

- **EnemyChaseFSM** component:  
  - Listens to `EnemyDetector`.  
  - Transitions Patrol → Chase when Player detected.  
  - Publishes HUD prompt `"Enemy spotted you!"` when switching to Chase.  
  - Returns to Patrol if Player escapes for configured delay.  

- **Prefab: EnemyPatroller**  
  - Created from EnemyDummy.  
  - Added Detector child with SphereCollider + EnemyDetector.  
  - Added EnemyPatrol and EnemyChaseFSM scripts.  
  - Waypoints set up in `Labyrinth_Prototype`.  

- **Validated behavior**:  
  - Enemy starts patrolling between waypoints.  
  - Enters Chase state when Player enters detection range.  
  - Displays HUD prompt `"Enemy spotted you!"`.  
  - If Player escapes, enemy returns to patrol.  
  - Still deals damage on contact via `EnemyDamageZone`.  
  - No errors in console after initialization fix (NavMeshAgent correctly linked).  

## ✅ Day 7 – Definition of Done

### Inspectable Boxes + Screamer mechanic

- Implemented `AimDetector` that detects when the player points the camera/flashlight at an object and holds for N seconds.
- `InspectableBox` prefab:
  - Can be a trap (`isTrap = true`) or contain the key (`keyPrefab`).
  - When correctly examined, spawns the key or reveals a clue.
  - When improperly interacted (not examined), triggers screamer behavior.
- `ScreamerController`:
  - Displays full-screen image and plays SFX.
  - Optionally locks player controls for the screamer duration.
- Key spawn & pickup:
  - Key prefab integrates with `PlayerInventory` (AddItem) and HUD prompts.
  - Key persists via existing Save/Load pipeline.
- Scene:
  - Room with multiple inspectable boxes; player must inspect to find the correct one to get the key.
  - Prompts show when pointing at objects and when screamer triggers.
- Validation:
  - Player must point and hold to inspect the correct box to obtain the key.
  - Triggering traps plays screamer image/audio and optionally blocks movement briefly.
  - No console errors; interactions tested end-to-end.


# 🧩 Day 8 – Definition of Done (October 7)

### 🎯 Context
Today’s session focused on reviewing the current state of the project and identifying key architectural adjustments needed for better modularity and gameplay scalability.  
The result was a clear list of technical improvements, configuration clean-ups, and new systems to implement during the next development stream.

---

### 🧠 Key Findings and Decisions
- Player data must be reorganized and split into more specific ScriptableObjects.  
- Refactoring is required in the **Player Movement** and **Input Bridge** systems.  
- A new **Player Inventory** system will be implemented soon.  
- The **Canvas/Screamer** logic requires cleanup and consistency.  
- Anchors will be used strategically for enemy spawns, loot points, and puzzle triggers.

---

### 🧱 Tasks and Changes Identified

#### 1. Player Config (SO)
- Remove outdated or redundant data.  
- Move **Player Health** into this ScriptableObject.  
- Prepare integration hooks for the upcoming **Inventory System**.

#### 2. Player Movement
- Review dependencies with `Player Input Bridge`.  
- Improve movement logic for future features (stamina, inventory weight, etc.).

#### 3. Player Inventory (New System)
- Create a modular base for item storage (batteries, keys, health items).  
- Plan interaction with the HUD and pickup system.

#### 4. Battery Config (Separate SO)
- Extract battery configuration from `Player Config`.  
- Define parameters for lifetime, drain rate, and recharge behavior.

#### 5. Enemy Base
- Link each enemy to its own `EnemyConfigSO`.  
- Allow individual tuning for speed, detection radius, and aggression.

#### 6. Aim Detector
- Centralize tunable values in a dedicated Config SO (range, layers, etc.).  

#### 7. Canvas / Screamer System
- Review if the Screamer Canvas is intended for single or multiple events.  
- Refactor for clarity and reuse across multiple screamers if necessary.

#### 8. Anchors & Spawn Points
- Create anchor prefabs for:
  - Enemy spawn zones.  
  - Loot (battery, health, items).  
  - Puzzle triggers or interactables.

#### 9. Puzzle Implementation (x2)
- Begin implementing two base puzzle types.  
- Define modular interfaces for interaction and event triggers.

---

### 🔍 Summary
Day 8 focused on **structural refactoring and system alignment**.  
The next development session will revolve around implementing the new configs and polishing the player core systems to achieve cleaner data management and more predictable gameplay flow.
## ✅ Day 9 – Definition of Done

### 🏗️ Architectural Refactor: Data-Driven Systems

- **Player & Flashlight Configs (SOs)**:
  - Refactored `PlayerConfig` and `FlashlightConfig` to expose all relevant variables to the Inspector using `[SerializeField]`.
  - Maintained encapsulation by keeping fields `private`.

- **Decoupled Save/Load System**:
  - `PlayerStateAdapter` no longer holds direct references to `PlayerHealth` or `PlayerFlashlight`.
  - `Read()` now gets data directly from `PlayerConfig` and `FlashlightConfig`.
  - `Apply()` now sets data on the SOs and publishes events (`HealthChanged`, `BatteryChanged`) for the HUD to consume.

- **Modular Item & Inventory System**:
  - Created abstract `ItemData` ScriptableObject, allowing each item to define its own `Use(GameObject user)` logic.
  - Implemented `ItemDatabase` to act as a central registry for all possible items in the game.
  - `PlayerInventory` now uses the `ItemDatabase` to execute item effects, removing the need for hardcoded `if/else` logic.

- **Event-Driven Inventory**:
  - Created `InventoryChanged` event DTO.
  - `PlayerInventory` now publishes this event whenever an item is added, used, or loaded, allowing the UI to react dynamically.
  - DTOs for all events (`HealthChanged`, `BatteryChanged`, etc.) were hardened by making their fields `readonly` to ensure immutability.

- **Decoupled Interaction Logic**:
  - `LockedDoor` no longer depends on `PlayerInventory`.
  - It now gets the `PlayerConfig` from the `interactor`'s `PlayerMovement` component to check for keys (`HasItem`).
  - This makes the door system more robust and independent of the inventory's implementation details.

- **Validation**:
  - Player state (health, battery, position) saves and loads correctly using the new SO-driven approach.
  - The `LockedDoor` correctly checks the player's inventory via `PlayerConfig` and unlocks.
  - The new item system architecture is in place, ready for creating new items like health potions or other keys.
  - The project's architecture is now significantly more modular, scalable, and aligned with best practices for data management in Unity.


  ## ✅ Day 10 – Definition of Done + +### 🧹 Code Refinement & Decoupling + +- ScreamerController Refactor:

  Removed direct reference to the main HUD's CanvasGroup.
  Now uses FindFirstObjectByType<HUDController>() to find the HUD controller and call its ShowHUD()/HideHUD() methods.
  This change completely decouples the screamer logic from the HUD implementation, making both systems more modular and robust.
  +- Input System Integration:

  PlayerInputController was updated to prepare for a proper Service Locator or Dependency Injection pattern.
  The direct instantiation new InputService() is now marked as a temporary step, aligning with best practices for service management.
  +--- + +### 🔋 Gameplay: Consumable Items + +- Battery Item:

  Created BatteryItem.cs, a new ItemData ScriptableObject that represents a consumable battery.
  Implemented the Use() method to restore a fixed amount of energy to the PlayerFlashlight.
  The item returns true upon use, ensuring it is consumed from the player's inventory.
  +- Battery Pickup:

  Created BatteryPickup.cs and a corresponding prefab.
  When the player interacts with it, the battery item is added to PlayerInventory.
  The player can then "use" the battery from a (future) inventory UI to recharge the flashlight.
  +- Validation:

  Picking up a battery adds it to the inventory.
  Using the battery correctly recharges the flashlight and removes the item from the inventory.
  The screamer continues to function correctly, now using a more robust, decoupled method to interact with the HUD.



## 🧠 Day 11 – Ambient & Clues (Lighting + Symbol Setup)

**Goal:** Establish the first mysterious atmosphere with baked lighting and the 3 main clue symbols that react to light and sound.

### 🎯 Deliverables
- [x] **Scene Setup:** Organized hierarchy for `Labyrinth_VSlice` with Environment, Anchors, and Lighting groups.  
- [x] **Baked Global Illumination:** All static geometry marked and baked with a single Directional Light (URP-friendly).  
- [x] **Playable Lighting:** Flashlight as the only dynamic light source (Spot Light, ~8 m range).  
- [x] **3 Hidden Symbols:**  
  - Dark material (URP/Lit) visible only when illuminated.  
  - Positioned in different corridors to encourage exploration.  
  - Slight surface detail or normal map to catch the flashlight.  
- [x] **3D Audio Hints:**  
  - Subtle looping hum for each symbol.  
  - Spatialized (full 3D blend), low volume (0.05–0.1), range ≈ 10 m.  
- [x] **Battery Pickup (optional):** Placed in a side path as a first reward.  
- [x] **Lighting Build Test:** Scene baked successfully, smooth shadows, no heavy post-process.  

### ✅ Definition of Done
- The environment is fully readable with baked light only.  
- Symbols “reveal” naturally under flashlight illumination.  
- Audio hints can be located directionally by the player.  
- Scene runs smoothly in-editor and ready for WebGL test build.  
