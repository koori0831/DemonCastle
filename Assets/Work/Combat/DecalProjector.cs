using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Work.Combat
{
    public class DecalProjector : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Rendering.Universal.DecalProjector projector;

        public void SetRadiuse(float radius)
        {
            projector.size = new Vector3(radius*2, radius * 2,3);
        }

        public void SetActiveDecal(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}