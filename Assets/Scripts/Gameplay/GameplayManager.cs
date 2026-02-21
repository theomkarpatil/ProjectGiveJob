using Sora.Variables;
using UnityEngine;

namespace Alantrix.Gameplay
{
    public class GameplayManager : Sora.Managers.Singleton<GameplayManager>
    {
        [SerializeField] private Vector2Variable gridSize;

        private void Awake()
        {

        }

        private void Start()
        {
            CardManager.instance.DealCards(gridSize.value);
        }

        public void OnPressingPlay()
        {

        }
    }
}