using UnityEngine;

public class CardAnimator : MonoBehaviour
{
    private const string flipToFrontAnimation = "flipToFront";
    private const string flipToBackAnimation = "flipToBack";
    private const string hoverAnimation = "hover";

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

    internal void HoverOverCard()
    {
        animator.Play(hoverAnimation, 0);

    }
}
