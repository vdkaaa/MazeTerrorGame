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


---
