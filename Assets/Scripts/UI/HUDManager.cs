using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HUDManager : Sora.Managers.Singleton<HUDManager>
{
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text turnsText;
    [SerializeField] private TMP_Text comboText;

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
        comboText.text = "x" + (combo.Item1).ToString();
        comboText.text += "\n+" + (combo.Item2).ToString();

        StartCoroutine(DelayedComboDissapear());
    }

    private IEnumerator DelayedComboDissapear()
    {
        yield return new WaitForSeconds(3.0f);
        comboText.transform.parent.gameObject.SetActive(false);
    }

    public void OnGameEnd(Component invoker, object currentCombo)
    {

    }
}
