using Sora.Variables;
using System.Collections;
using UnityEngine;

namespace Alantrix.Gameplay
{
    public class GameplayManager : Sora.Managers.Singleton<GameplayManager>
    {
        [SerializeField] internal Vector2Variable gridSize;
        [SerializeField] private Sora.Events.SoraEvent gameEnd;
        [SerializeField] private Sora.Events.SoraEvent requestGameSave;

        [SerializeField] Sora.Events.SoraEvent loadLastSavedGame;
        [SerializeField] private BoolVariable loadGame;
        [SerializeField] private BoolVariable autoSave;

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
                StartCoroutine(GameEnd());
            }
            else
            {
                if (autoSave.value)
                {
                    requestGameSave.InvokeEvent();
                }
            }
        }

        private IEnumerator GameEnd()
        {
            yield return new WaitForSeconds(2.0f);
            AudioManager.instance.PlayGameOverAudio();
            gameEnd.InvokeEvent(this, ScoreManager.instance.GetScore());
        }


    }
}