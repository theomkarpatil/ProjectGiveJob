
using Alantrix.Gameplay.Card;
using Sora.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Alantrix.Gameplay
{
    public class CardManager : Sora.Managers.Singleton<CardManager>
    {
        [SerializeField] private PlayingCard[] AvailableCards;
        [SerializeField] private GameObject playArea;
        [SerializeField] private float delayBeforeHidingCards;
        [SerializeField] private float cardSettleDelay;
        internal Color greyedColor = new Color(0.8f, 0.8f, 0.8f);

        private Dictionary<int, PlayingCard> selectedCards = new Dictionary<int, PlayingCard>();
        private int currentClickIndex;

        private Vector2 playAreaSize;
        private const int gridSpacing = 50;
        private Match match = new Match();
        private int previousMatch;

        private void OnEnable()
        {
            playAreaSize.x = playArea.GetComponent<RectTransform>().rect.width;
            playAreaSize.y = playArea.GetComponent<RectTransform>().rect.height;

            Random.InitState((int)System.DateTime.Now.Ticks);
        }

        internal void DealCards(Vector2 gridSize)
        {
            currentClickIndex = 0;
            GridLayoutGroup grid = playArea.GetComponent<GridLayoutGroup>();

            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = (int)gridSize.x;

            grid.spacing = Vector2.one * gridSpacing;

            Vector2 cellSize;
            //cellSize.x = (playAreaSize.x - ((gridSize.x - 1) * gridSpacing)) / gridSize.x;
            cellSize.y = (playAreaSize.y - ((gridSize.y - 1) * gridSpacing)) / gridSize.y;
            cellSize.x = cellSize.y * (5.0f / 7.0f);
            grid.cellSize = cellSize;

            int cardCount = (int)gridSize.x * (int)gridSize.y;
            List<PlayingCard> deck = new List<PlayingCard>();

            // making sure we add half the value of cardCount because we need 2 of each
            for (int i = 0; i < cardCount / 2; ++i)
            {
                int index = Random.Range(0, 8);

                // adding the same instance again to account for matches
                deck.Add(AvailableCards[index]);
                deck.Add(AvailableCards[index]);
            }

            deck.Shuffle();
            List<PlayingCard> cards = new List<PlayingCard>();
            foreach (PlayingCard pc in deck)
            {
                cards.Add(Instantiate(pc, playArea.transform));
            }
            StartCoroutine(HideCardsAfterDealing(cards));
        }

        private IEnumerator HideCardsAfterDealing(List<PlayingCard> deck)
        {
            yield return new WaitForSeconds(delayBeforeHidingCards);
            // shuffling for a little randomized animation
            deck.Shuffle();
            foreach (PlayingCard pc in deck)
            {
                pc.FlipDealtCard();
                yield return new WaitForSeconds(0.1f);
            }

        }

        internal void OnCardSelected(PlayingCard card)
        {
            selectedCards.Add(currentClickIndex, card);

            if (currentClickIndex % 2 == 0)
            {
                match = new Match();
                Debug.Log("New Match created");
            }

            currentClickIndex++;

            if (match.card1 == null)
            {
                match.card1 = card;
                Debug.Log("Card 1 added to match");
            }
            else
            {
                match.card2 = card;
                Debug.Log("Card 2 added to match");
                match.Display();

                if (match.isAMatch())
                {
                    Debug.Log("Match found");

                    if (previousMatch - currentClickIndex == 2)
                    {
                        ScoreManager.instance.UpComboCounter();
                    }
                    else
                    {
                        ScoreManager.instance.ResetComboCounter();
                    }
                    previousMatch = currentClickIndex;
                    StartCoroutine(MatchCards(match.card1, match.card2));
                    ScoreManager.instance.AddMatchScore();
                }
                else
                {
                    Debug.Log("Match not found, flipping cards back");
                    StartCoroutine(FlipBackCards(match.card1, match.card2));
                }
            }

            Debug.Log("CurrentClick: " + currentClickIndex);
        }

        private IEnumerator FlipBackCards(PlayingCard card1, PlayingCard card2)
        {
            yield return new WaitForSeconds(cardSettleDelay);
            card1.MatchNotFound();
            card2.MatchNotFound();
        }

        private IEnumerator MatchCards(PlayingCard card1, PlayingCard card2)
        {
            yield return new WaitForSeconds(cardSettleDelay);

            card1.MatchFound();
            card2.MatchFound();
        }
    }

}
