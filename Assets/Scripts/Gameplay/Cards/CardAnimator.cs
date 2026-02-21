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
    }

    internal void FlipCardToBack()
    {
        animator.Play(flipToBackAnimation, 0);
    }

    internal void HoverOverCard(bool stop)
    {
        if (!stop)
            animator.Play(hoverAnimation, 0);
        else
            animator.SetTrigger(idleTrigger);
    }

    internal void StopAnimations()
    {
        animator.Play("idle");
    }
}
