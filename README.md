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

