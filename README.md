# MazeTerrorGame
A 3D psychological horror game built with Unity 6 (URP).
✅ Day 1 – Definition of Done

Project Setup

Unity 6 project created with URP assigned in Graphics and Quality settings.

Folder structure organized (Core, Gameplay, Data, UI, Scenes, Prefabs, Settings).

Git repository initialized with first commits.

Scenes

Created base scenes: Boot, MainMenu, Labyrinth_Prototype.

Build order set: Boot → MainMenu → Prototype.

Basic flow implemented:

Boot → MainMenu via BootStartup + SceneLoader.

MainMenu Play button loads Prototype.

Core Services (stubs)

Interfaces defined: IInputService, IAudioService, ISaveService, IEventBus, ITimeService.

EventBus stub with Publish, Subscribe, Unsubscribe methods.

AppInstaller stub created to bootstrap global services.

Player

Interfaces defined: IMovable, IInteractable, IInventory, IFlashlight, IDamageable.

Stub components added: PlayerMovement, PlayerInteractor, PlayerInventory, PlayerFlashlight, PlayerHealth.

PlayerRoot prefab created:

Capsule + CharacterController.

CameraRig/MainCamera hierarchy.

Flashlight GameObject with Light component (disabled by default).

Scripts added to prefab and basic references assigned.

PlayerRoot placed in Labyrinth_Prototype.

Data

ScriptableObjects created for configuration:

PlayerConfig (walkSpeed, runSpeed).

FlashlightConfig (intensity, angle, drainPerSecond).

Default asset instances stored in Data/ScriptableObjects.
