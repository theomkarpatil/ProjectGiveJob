using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Alantrix.Gameplay.Card
{
    public enum CardState
    {
        IDLE,
        HOVERING,
        FLIPPED,
        FOUND
    }

    internal class Match
    {
        internal PlayingCard card1;
        internal PlayingCard card2;

        internal bool isAMatch()
        {
            if (card1 == null || card2 == null) return false;
            return card1.index == card2.index;
        }

        internal void Display()
        {
            string s = "Match: Card1: " + card1.index;
            s += ", Card2: " + card2.index;
            Debug.Log(s);
        }
    }

    public class PlayingCard : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public int index;
        public Image cardFace;

        private Button button;
        private CardAnimator cardAnimator;

        // card state variables
        public CardState state;

        private void OnEnable()
        {
            cardAnimator = GetComponent<CardAnimator>();
            button = GetComponentInChildren<Button>();
            button.onClick.AddListener(OnCardSelected);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!GameplayManager.instance.gameStarted)
                return;

            // starts hovering when mouse pointer hovers over the card
            // this therefore only works on PC/Mac
            if (state == CardState.IDLE)
            {
                Debug.Log("Hovering over card", this);
                cardAnimator.HoverOverCard(false);
                state = CardState.HOVERING;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!GameplayManager.instance.gameStarted)
                return;

            // stop hovering when mouse pointer exits the card rect
            // this therefore only works on PC/Mac
            if (state == CardState.HOVERING)
            {
                Debug.Log("Stop hover", this);

                cardAnimator.HoverOverCard(true);
                state = CardState.IDLE;
            }
        }

        private void OnCardSelected()
        {
            if (state != CardState.FLIPPED || state != CardState.FOUND)
            {
                cardAnimator.FlipCardToFront();
                CardManager.instance.OnCardSelected(this);
                button.interactable = false;
            }
        }

        internal void MatchFound()
        {
            state = CardState.FOUND;
            cardFace.color = CardManager.instance.greyedColor;

            AudioManager.instance.PlayMatchFoundAudio();
        }

        internal void MatchNotFound()
        {
            Debug.Log("Flipping card to idle", this.gameObject);
            AudioManager.instance.PlayMatchFailedAudio();

            cardAnimator.FlipCardToBack();
        }

        internal void FlipDealtCard()
        {
            cardAnimator.FlipCardToBack();
        }

        internal void ShowDealtCard()
        {
            cardAnimator.FlipCardToFront();
            AudioManager.instance.PlayCardFlipAudio(true);

            GetComponentInChildren<Button>().interactable = false;
        }
    }
}