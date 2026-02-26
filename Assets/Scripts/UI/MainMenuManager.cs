using Sora.Variables;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Alantrix.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        private const string savedFileName = "matchSave.json";

        [SerializeField] private Sora.Variables.Vector2Variable finalizedGrid;
        [SerializeField] private BoolVariable loadGame;

        [Header("UI Variables")]
        [SerializeField] private TMP_InputField cardGridRowCount;
        [SerializeField] private TMP_InputField cardGridColumnCount;
        [SerializeField] private GameObject cardCountErrorText;
        [SerializeField] private Button loadGameButton;

        private void OnEnable()
        {
            string path = Path.Combine(Application.persistentDataPath, savedFileName);
            loadGame.value = false;

            if (!File.Exists(path))
            {
                loadGameButton.interactable = false;
                return;
            }
        }

        private void LoadGameScene()
        {
            SceneManager.LoadScene(1);
            gameObject.SetActive(false);
        }

        public void OnPressingPlay()
        {
            int columns = !string.IsNullOrEmpty(cardGridColumnCount.text) ? int.Parse(cardGridColumnCount.text) : (int)finalizedGrid.value.x;

            int rows = !string.IsNullOrEmpty(cardGridRowCount.text) ? int.Parse(cardGridRowCount.text) : (int)finalizedGrid.value.y;

            finalizedGrid.value.x = columns;
            finalizedGrid.value.y = rows;

            if ((columns * rows) % 2 != 0)
            {
                cardCountErrorText.SetActive(true);
                return;
            }

            LoadGameScene();
        }


        public void OnPressingLoadLastSaved()
        {
            loadGame.value = true;

            SceneManager.LoadScene(1);
            gameObject.SetActive(false);
        }
    }
}