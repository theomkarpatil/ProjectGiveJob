using Alantrix.Gameplay.Card;
using Sora.Variables;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Alantrix.Gameplay
{
    [System.Serializable]
    public class PlayingCardSaveData
    {
        public int index;      // index in AvailableCards
        public CardState state;
    }

    [System.Serializable]
    public class DealtCardsSaveData
    {
        public int columns;
        public int rows;
        public List<PlayingCardSaveData> cards;
    }

    public class SaveLoadManager : Sora.Managers.Singleton<SaveLoadManager>
    {
        [SerializeField] private Vector2Variable gridSize;
        [SerializeField] private Sora.Events.SoraEvent requestSave;

        private const string savedFileName = "matchSave.json";

        private void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        public void SaveDealtCards(Component invoker, object value)
        {
            List<PlayingCard> dealtCards = CardManager.instance.GetDealtCards();
            DealtCardsSaveData saveData = new DealtCardsSaveData();

            saveData.columns = (int)gridSize.value.x;
            saveData.rows = (int)gridSize.value.y;

            saveData.cards = new List<PlayingCardSaveData>();
            foreach (var card in dealtCards)
            {
                PlayingCardSaveData data = new PlayingCardSaveData();

                data.index = card.index;
                data.state = card.state;

                saveData.cards.Add(data);
            }

            string json = JsonUtility.ToJson(saveData, true);

            string path = Path.Combine(Application.persistentDataPath, savedFileName);

            File.WriteAllText(path, json);

            Debug.Log("Saved to: " + path);
        }

        public void LoadDealtCards()
        {
            string path = Path.Combine(Application.persistentDataPath, savedFileName);

            if (!File.Exists(path))
            {
                Debug.LogWarning("Save file not found at: " + path);
                return;
            }

            string json = File.ReadAllText(path);

            DealtCardsSaveData saveData =
                JsonUtility.FromJson<DealtCardsSaveData>(json);

            List<PlayingCard> loadedCards = new List<PlayingCard>();
            List<CardState> cardStates = new List<CardState>();

            foreach (var cardData in saveData.cards)
            {
                // Instantiate using saved index
                PlayingCard card = CardManager.instance.AvailableCards[cardData.index - 1];

                cardStates.Add(cardData.state);
                loadedCards.Add(card);
            }

            CardManager.instance.DealCards(new Vector2(saveData.columns, saveData.rows), loadedCards.ToArray(), cardStates);
        }
    }
}