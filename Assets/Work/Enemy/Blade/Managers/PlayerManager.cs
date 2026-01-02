using Blade.Entities;
using Blade.Players;
using GondrLib.Dependencies;
using UnityEngine;
using Work.Characters.Code;

namespace Blade.Managers
{
    [DefaultExecutionOrder(-1)]
    public class PlayerManager : MonoBehaviour
    {
        [Inject] public Character _player;
        [SerializeField] private EntityFinderSO playerFinder;

        
        private void Awake()
        {
            playerFinder.SetTarget(_player);
        }
    }
}