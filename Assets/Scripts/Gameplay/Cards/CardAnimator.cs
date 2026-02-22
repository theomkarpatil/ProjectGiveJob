using Alantrix.Gameplay.Card;
using System.Collections;
using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    private const string flipToFrontAnimation = "flipToFront";
    private const string flipToBackAnimation = "flipToBack";
    private const string hoverAnimation = "hover";
    private const string idleTrigger = "returnToIdle";

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
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
        GetComponent<PlayingCard>().state = CardState.IDLE;
    }

    internal void StopAnimations()
    {
        animator.Play("idle");
    }
}
