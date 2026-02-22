using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDManager : Sora.Managers.Singleton<HUDManager>
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnsText;
    [SerializeField] private TMP_Text comboText;

    [Space]
    [SerializeField] private GameObject gameEndUI;
    [SerializeField] private TMP_Text finalScoreText;

    void Start()
    {
        scoreText.text = "0";
        turnsText.text = "0";
        comboText.text = "";
    }

    public void UpdateScoreText(Component invoker, object score)
    {
        scoreText.text = ((int)score).ToString();
    }

    public void UpdateTurnsText(Component invoker, object turns)
    {
        turnsText.text = ((int)turns).ToString();
    }

    public void UpdateComboText(Component invoker, object currentCombo)
    {
        Tuple<int, int> combo = currentCombo as Tuple<int, int>;
        comboText.transform.parent.gameObject.SetActive(true);
        comboText.text = "x" + (combo.Item2).ToString();
        comboText.text += "\n+" + (combo.Item1).ToString();

        StartCoroutine(DelayedComboDissapear());
    }

    private IEnumerator DelayedComboDissapear()
    {
        yield return new WaitForSeconds(3.0f);
        comboText.transform.parent.gameObject.SetActive(false);
    }

    public void OnGameEnd(Component invoker, object finalScore)
    {
        gameEndUI.SetActive(true);
        finalScoreText.text = finalScore.ToString();
    }

    public void OnPressingReplay()
    {
        SceneManager.LoadScene(1);
    }

    public void OnPressingHome()
    {
        SceneManager.LoadScene(0);
    }
}
