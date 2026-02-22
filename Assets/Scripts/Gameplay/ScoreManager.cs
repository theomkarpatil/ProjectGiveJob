using System;
using UnityEngine;

namespace Alantrix.Gameplay
{
    public class ScoreManager : Sora.Managers.Singleton<ScoreManager>

    {
        [SerializeField] private Sora.Events.SoraEvent updateScore;
        [SerializeField] private Sora.Events.SoraEvent updateTurns;
        [SerializeField] private Sora.Events.SoraEvent updateCombo;
        [SerializeField] private int matchScore;
        [SerializeField] private int comboMultiplier;

        private int turns;
        private int comboCounter;
        private int score;

        private void OnEnable()
        {
            score = 0;
            turns = 0;
            comboCounter = 0;
        }

        public void AddMatchScore()
        {
            AddScore(matchScore);
            Debug.Log($"MatchScore added. Score = {score}");
        }

        public void AddScore(int score)
        {
            this.score += score;
            Debug.Log($"Score {score} added. Score = {this.score}");
            updateScore.InvokeEvent(this, this.score);
        }

        public void UpdateTurns()
        {
            turns++;
            updateTurns.InvokeEvent(this, turns);
        }

        public void SetTurns(int turns)
        {
            this.turns = turns;
            updateTurns.InvokeEvent(this, this.turns);
        }

        public void UpComboCounter()
        {
            comboCounter++;
            Debug.Log("Combo Counter increased to: " + comboCounter);
        }

        public void ResetComboCounter()
        {
            if (comboCounter >= 2)
            {
                int score = comboCounter * comboMultiplier * matchScore;
                Debug.Log($"Combo counter: {comboCounter}. Adding combo score: {score}");
                AddScore(score);
                Tuple<int, int> combo = new Tuple<int, int>(score, comboCounter);
                updateCombo.InvokeEvent(this, combo);
            }
            Debug.Log("Resetting combo counter");
            comboCounter = 0;
        }

        public int GetScore()
        {
            return score;
        }

        public int GetTurns()
        {
            return turns;
        }
    }
}