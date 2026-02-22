using UnityEngine;

public class AudioManager : Sora.Managers.Singleton<AudioManager>
{
    [SerializeField] private AudioClip successfulMatchAC;
    [SerializeField] private AudioClip incorrectMatchAC;
    [SerializeField] private AudioClip cardFlipAC1;
    [SerializeField] private AudioClip cardFlipAC2;
    [SerializeField] private AudioClip dealCardsAC;
    [SerializeField] private AudioClip gameOverAC;

    private AudioSource audioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
    }

    internal void PlayMatchFoundAudio()
    {
        audioSource.PlayOneShot(successfulMatchAC);
    }

    internal void PlayMatchFailedAudio()
    {
        audioSource.PlayOneShot(incorrectMatchAC);
    }

    internal void PlayDealCardsAudio()
    {
        audioSource.PlayOneShot(dealCardsAC);
    }

    internal void PlayCardFlipAudio(bool toFront)
    {
        if (toFront)
            audioSource.PlayOneShot(cardFlipAC1);
        else
            audioSource.PlayOneShot(cardFlipAC2);
    }

    internal void PlayGameOverAudio()
    {
        audioSource.PlayOneShot(gameOverAC);
    }
}
