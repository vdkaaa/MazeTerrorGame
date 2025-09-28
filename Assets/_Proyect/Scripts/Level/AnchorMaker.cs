using UnityEngine;

namespace Project.Level
{
    public enum AnchorType { Puzzle, Loot, EnemySpawn }

    public class AnchorMarker : MonoBehaviour
    {
        public AnchorType type = AnchorType.Puzzle;
        [Min(0f)] public float radius = 0.5f;

        private Color GetColor()
        {
            return type switch
            {
                AnchorType.Puzzle => new Color(1f, 0.85f, 0.2f, 0.9f),  // amarillo
                AnchorType.Loot => new Color(0.2f, 0.9f, 1f, 0.9f),   // cian
                AnchorType.EnemySpawn => new Color(1f, 0.25f, 0.25f, 0.9f), // rojo
                _ => Color.white
            };
        }

        private void OnDrawGizmos()
        {
            var c = GetColor();
            Gizmos.color = c;
            Gizmos.DrawSphere(transform.position, radius);
            Gizmos.color = new Color(c.r, c.g, c.b, 0.4f);
            Gizmos.DrawWireSphere(transform.position, Mathf.Max(radius, 0.1f));
        }
    }
}
