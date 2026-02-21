using Sora.Events;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Alantrix.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        private const string gameScene = "GameScene";

        [SerializeField] private InputField cardGridRowCount;
        [SerializeField] private InputField cardGridColumnCount;

        [SerializeField] private SoraEvent startGame;

        [SerializeField] private Sora.Variables.Vector2Variable finalizedGrid;
        public void OnPressingPlay()
        {
            finalizedGrid.value = new Vector2(int.Parse(cardGridColumnCount.text), int.Parse(cardGridRowCount.text));

            SceneManager.LoadSceneAsync(gameScene, LoadSceneMode.Additive);
        }
    }
}