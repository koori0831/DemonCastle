using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Blade.SkillSystem
{
    public class RoundDecal : MonoBehaviour
    {
        [SerializeField] private DecalProjector decal;
        [SerializeField] private float depth = 3f;

        public void SetProjectActive(bool isActive)
        {
            decal.enabled = isActive;
        }

        public void SetDecalSize(float radius)
        {
            decal.size = new Vector3(radius * 2, radius * 2, depth);
        }
    }
}