using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Alantrix.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Sora.Variables.Vector2Variable finalizedGrid;
        [SerializeField] private TMP_InputField cardGridRowCount;
        [SerializeField] private TMP_InputField cardGridColumnCount;
        [SerializeField] private GameObject cardCountErrorText;


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

            finalizedGrid.value = new Vector2(columns, rows);

            SceneManager.LoadSceneAsync(1);
            gameObject.SetActive(false);
        }
    }
}