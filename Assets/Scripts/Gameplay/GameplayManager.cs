using Sora.Variables;
using UnityEngine;

namespace Alantrix.Gameplay
{
    public class GameplayManager : Sora.Managers.Singleton<GameplayManager>
    {
        [SerializeField] private Vector2Variable gridSize;
        [SerializeField] private Sora.Events.SoraEvent gameEnd;
        internal bool gameStarted;
        internal int matchesFound;

        private void Awake()
        {
            gameStarted = false;
        }

        private void Start()
        {
            CardManager.instance.DealCards(gridSize.value);
        }

        public void OnPressingPlay()
        {

        }

        public void CheckForGameEnd()
        {
            if (matchesFound == (gridSize.value.x * gridSize.value.y) / 2)
            {
                GameEnd();
            }
        }

        private void GameEnd()
        {
            gameEnd.InvokeEvent();
        }
    }
}