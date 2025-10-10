// En una nueva carpeta, por ejemplo: Scripts/Gameplay/Items
using UnityEngine;
namespace Project.Data
{
public abstract class ItemData : ScriptableObject
{
    [Tooltip("ID único para este objeto (ej: 'HealthPotion')")]
    public string itemId;
    [Tooltip("Nombre que se muestra en el juego")]
    public string displayName;
    [TextArea]
    public string description;
    public Sprite icon;

    // El método clave: cada item define qué hace al usarse.
    public abstract bool Use(GameObject user);
}
}