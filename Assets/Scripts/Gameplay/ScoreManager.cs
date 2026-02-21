using UnityEngine;

public class ScoreManager : Sora.Managers.Singleton<ScoreManager>

{
    [SerializeField] private Sora.Events.SoraEvent updateScore;
    [SerializeField] private int matchScore;
    [SerializeField] private int comboMultiplier;

    private int comboCounter;

    private int score;

    private void OnEnable()
    {
        score = 0;
        comboCounter = 1;
    }

    public void AddMatchScore()
    {
        score += matchScore;
        Debug.Log($"MatchScore added. Score = {score}");

    }

    public void AddScore(int score)
    {
        this.score += score;
        Debug.Log($"Score {score} added. Score = {this.score}");
        updateScore.InvokeEvent(this, this.score);
    }

    public void UpComboCounter()
    {
        comboCounter++;
        Debug.Log("Combo Counter increased to: " + comboCounter);
    }

    public void ResetComboCounter()
    {
        if (comboCounter > 2)
        {
            int score = comboCounter * comboMultiplier * matchScore;
            Debug.Log("Adding combo score: " + score);
            AddScore(score);
        }
        comboCounter = 1;
    }

    public int GetScore()
    {
        return score;
    }
}
