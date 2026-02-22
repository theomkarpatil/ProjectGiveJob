
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
        public PlayingCard[] AvailableCards;
        [SerializeField] private GameObject playArea;
        [Tooltip("Delay after which cards flip back after being dealt")]
        [SerializeField] private float delayBeforeHidingCards;
        [Tooltip("Delay after which cards flip after a match test")]
        [SerializeField] private float cardFlipDelay;

        internal Color greyedColor = new Color(0.8f, 0.8f, 0.8f);

        private Dictionary<int, PlayingCard> selectedCards = new Dictionary<int, PlayingCard>();
        private List<PlayingCard> dealtCards = new List<PlayingCard>();

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

        #region Card Dealing logic
        private void EvaluatePlayAreaMetrics(Vector2 gridSize)
        {
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
        }

        public void DealCards(Vector2 gridSize, PlayingCard[] loadedCards = null, List<CardState> cardStates = null)
        {
            EvaluatePlayAreaMetrics(gridSize);

            dealtCards = new List<PlayingCard>();
            currentClickIndex = 0;

            if (loadedCards == null)
            {
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

                // shuffle to make sure they aren't placed adjacent to their matches
                deck.Shuffle();
                foreach (PlayingCard pc in deck)
                {
                    dealtCards.Add(Instantiate(pc, playArea.transform));
                }

                // shuffling for a little randomized animation
                // making sure the initial grid of dealtCards remains the same

                dealtCards.Shuffle();
                StartCoroutine(ShowCards(dealtCards));
            }
            else
            {
                // need to make a list of cards that haven't been matched yet so that we can flip them.
                List<PlayingCard> cardsToFlip = new List<PlayingCard>();
                // and another that need to be revealed
                List<PlayingCard> cardsToReveal = new List<PlayingCard>();

                for (int i = 0; i < loadedCards.Length; ++i)
                {
                    PlayingCard card = Instantiate(loadedCards[i], playArea.transform);
                    card.state = cardStates[i];
                    if (card.state == CardState.FOUND)
                        cardsToReveal.Add(card);
                    else
                        cardsToFlip.Add(card);

                    dealtCards.Add(card);
                }

                StartCoroutine(ShowCards(cardsToFlip));
                StartCoroutine(ShowCards(cardsToReveal, false));

                foreach (PlayingCard card in cardsToReveal)
                {
                    card.MatchFound();
                }


            }
        }

        private IEnumerator ShowCards(List<PlayingCard> dealtCards, bool flip = true)
        {
            yield return new WaitForSeconds(0.5f);
            Debug.Log("Revealing dealt cards");
            foreach (PlayingCard pc in dealtCards)
            {
                pc.ShowDealtCard();
            }

            if (flip)
                StartCoroutine(HideCardsAfterDealing(dealtCards));
        }

        private IEnumerator HideCardsAfterDealing(List<PlayingCard> dealtCards)
        {
            yield return new WaitForSeconds(delayBeforeHidingCards);
            Debug.Log("Flipping dealt cards back");
            foreach (PlayingCard pc in dealtCards)
            {
                pc.FlipDealtCard();
                pc.GetComponentInChildren<Button>().interactable = true;
                yield return new WaitForSeconds(0.1f);
            }

            GameplayManager.instance.gameStarted = true;
        }

        #endregion

        #region Card Play Logic
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
                    Match currentMatch = match;
                    StartCoroutine(MatchCards(currentMatch.card1, currentMatch.card2));
                    ScoreManager.instance.AddMatchScore();

                    GameplayManager.instance.CheckForGameEnd();
                }
                else
                {
                    ScoreManager.instance.ResetComboCounter();

                    Debug.Log("Match not found, flipping cards back");
                    Match currentMatch = match;
                    StartCoroutine(FlipBackCards(currentMatch.card1, currentMatch.card2));
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
        #endregion

        public List<PlayingCard> GetDealtCards()
        {
            return dealtCards;
        }
    }
}
