using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Alantrix.Gameplay.Card
{
    public class CardAnimator : MonoBehaviour
    {
        private const string flipToFrontAnimation = "flipToFront";
        private const string flipToBackAnimation = "flipToBack";
        private const string hoverAnimation = "hover";
        private const string idleTrigger = "returnToIdle";

        private Animator animator;
        private Button button;
        private PlayingCard playingCard;

        private void OnEnable()
        {
            animator = GetComponent<Animator>();
            button = GetComponentInChildren<Button>();
            playingCard = GetComponent<PlayingCard>();
        }

        internal void FlipCardToFront()
        {
            animator.Play(flipToFrontAnimation, 0);
            GetComponent<PlayingCard>().state = CardState.FLIPPED;

            AudioManager.instance.PlayCardFlipAudio(true);
        }

        internal void FlipCardToBack()
        {
            animator.Play(flipToBackAnimation, 0);

            float duration = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length;
            StartCoroutine(DelayedIdleState(duration));

            AudioManager.instance.PlayCardFlipAudio(false);
        }

        internal void HoverOverCard(bool stop)
        {
            if (!stop)
                animator.Play(hoverAnimation, 0);
            else
                animator.SetTrigger(idleTrigger);
        }

        private IEnumerator DelayedIdleState(float duration)
        {
            yield return new WaitForSeconds(duration);
            playingCard.state = CardState.IDLE;
            button.interactable = true;
        }

        internal void StopAnimations()
        {
            animator.Play("idle");
        }
    }
}