using Sora.Variables;
using UnityEngine;

namespace Alantrix.Gameplay
{
    public class GameplayManager : Sora.Managers.Singleton<GameplayManager>
    {
        [SerializeField] internal Vector2Variable gridSize;
        [SerializeField] private Sora.Events.SoraEvent gameEnd;

        [SerializeField] Sora.Events.SoraEvent loadLastSavedGame;
        [SerializeField] private BoolVariable loadGame;

        internal bool gameStarted;
        internal int matchesFound;

        private void Awake()
        {
            gameStarted = false;
        }

        private void Start()
        {
            if (!loadGame.value)
                CardManager.instance.DealCards(gridSize.value);
            else
                loadLastSavedGame.InvokeEvent();
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
            gameEnd.InvokeEvent(this, ScoreManager.instance.GetScore());
        }


    }
}