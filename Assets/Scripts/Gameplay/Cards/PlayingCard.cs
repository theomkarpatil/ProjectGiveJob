using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Alantrix.Gameplay.Card
{
    internal enum CardState
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
        [SerializeField] internal int index;
        [SerializeField] private Image cardFace;

        private Button button;
        private CardAnimator cardAnimator;

        // card state variables
        internal CardState state;
        private bool isFlipped;

        private void OnEnable()
        {
            state = CardState.IDLE;

            button = GetComponentInChildren<Button>();
            button.onClick.AddListener(OnCardSelected);

            cardAnimator = GetComponent<CardAnimator>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // starts hovering when mouse pointer hovers over the card
            // this therefore only works on PC/Mac
            if (state == CardState.IDLE)
            {
                cardAnimator.HoverOverCard(false);
                state = CardState.HOVERING;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // stop hovering when mouse pointer exits the card rect
            // this therefore only works on PC/Mac
            if (state == CardState.HOVERING)
            {
                cardAnimator.HoverOverCard(true);
                state = CardState.IDLE;
            }
        }

        private void OnCardSelected()
        {
            if (state != CardState.FLIPPED)
            {
                cardAnimator.FlipCardToFront();
                CardManager.instance.OnCardSelected(this);
                state = CardState.FLIPPED;
            }
        }

        internal void MatchFound()
        {
            cardFace.color = CardManager.instance.greyedColor;
        }
    }
}