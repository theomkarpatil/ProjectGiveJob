
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
        [Tooltip("Delay after which cards flip back after being dealt")]
        [SerializeField] private float delayBeforeHidingCards;
        [Tooltip("Delay after which cards flip after a match test")]
        [SerializeField] private float cardFlipDelay;

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
            grid.spacing = Vector2.one * gridSpacing;

            int columns = (int)gridSize.x;
            int rows = (int)gridSize.y;

            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;

            float totalSpacingX = (columns - 1) * gridSpacing;
            float totalSpacingY = (rows - 1) * gridSpacing;

            float maxCellWidth = (playAreaSize.x - totalSpacingX) / columns;
            float widthBasedHeight = maxCellWidth * (7f / 5f);

            float maxCellHeight = (playAreaSize.y - totalSpacingY) / rows;
            float heightBasedWidth = maxCellHeight * (5f / 7f);

            float finalWidth;
            float finalHeight;

            // Check if width-based sizing fits vertically
            if (widthBasedHeight * rows + totalSpacingY <= playAreaSize.y)
            {
                finalWidth = maxCellWidth;
                finalHeight = widthBasedHeight;
            }
            else
            {
                finalWidth = heightBasedWidth;
                finalHeight = maxCellHeight;
            }

            grid.cellSize = new Vector2(finalWidth, finalHeight);

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
            StartCoroutine(ShowCards(cards));
        }

        private IEnumerator ShowCards(List<PlayingCard> deck)
        {
            yield return new WaitForSeconds(0.5f);
            // shuffling for a little randomized animation
            deck.Shuffle();
            foreach (PlayingCard pc in deck)
            {
                pc.ShowDealtCard();
            }

            StartCoroutine(HideCardsAfterDealing(deck));
        }

        private IEnumerator HideCardsAfterDealing(List<PlayingCard> deck)
        {
            yield return new WaitForSeconds(delayBeforeHidingCards);
            foreach (PlayingCard pc in deck)
            {
                pc.FlipDealtCard();
                pc.GetComponentInChildren<Button>().interactable = true;
                yield return new WaitForSeconds(0.1f);
            }

            GameplayManager.instance.gameStarted = true;
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
                ScoreManager.instance.UpdateTurns();

                if (match.isAMatch())
                {
                    Debug.Log("Match found");
                    GameplayManager.instance.matchesFound++;

                    if (currentClickIndex - previousMatch == 2)
                    {
                        ScoreManager.instance.UpComboCounter();
                    }

                    previousMatch = currentClickIndex;
                    StartCoroutine(MatchCards(match.card1, match.card2));
                    ScoreManager.instance.AddMatchScore();

                    GameplayManager.instance.CheckForGameEnd();
                }
                else
                {
                    ScoreManager.instance.ResetComboCounter();

                    Debug.Log("Match not found, flipping cards back");
                    StartCoroutine(FlipBackCards(match.card1, match.card2));
                }
            }

            Debug.Log("CurrentClick: " + currentClickIndex);
        }

        private IEnumerator FlipBackCards(PlayingCard card1, PlayingCard card2)
        {
            yield return new WaitForSeconds(cardFlipDelay);
            card1.MatchNotFound();
            card2.MatchNotFound();
        }

        private IEnumerator MatchCards(PlayingCard card1, PlayingCard card2)
        {
            yield return new WaitForSeconds(cardFlipDelay);

            card1.MatchFound();
            card2.MatchFound();
        }
    }
}
