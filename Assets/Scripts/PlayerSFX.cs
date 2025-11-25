using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    [Header("Main SFX Source")]
    [SerializeField] private AudioSource sfxSource;

    [Header("Wall Slide Source (looping)")]
    [SerializeField] private AudioSource wallSlideSource;

    [Header("Step Sounds (Randomized)")]
    public AudioClip[] stepSounds;
    private int lastStepIndex = -1;

    [Header("Other Sounds")]
    public AudioClip jumpSound;
    public AudioClip deathSound;
    public AudioClip wallSlideLoopSound;   

    private void Start()
    {
        if (wallSlideSource != null)
        {
            wallSlideSource.clip = wallSlideLoopSound;
            wallSlideSource.loop = true;
            wallSlideSource.playOnAwake = false;
        }
    }

    public void PlayRunStep()
    {
        if (stepSounds == null || stepSounds.Length == 0) return;

        int index;
        do
        {
            index = Random.Range(0, stepSounds.Length);
        }
        while (index == lastStepIndex);

        lastStepIndex = index;
        sfxSource.PlayOneShot(stepSounds[index], 0.8f);
    }

    public void PlayJump() => sfxSource.PlayOneShot(jumpSound);
    public void PlayDeath() => sfxSource.PlayOneShot(deathSound);

    public void StartWallSlide()
    {
        if (!wallSlideSource.isPlaying)
            wallSlideSource.Play();
    }

    public void StopWallSlide()
    {
        if (wallSlideSource.isPlaying)
            wallSlideSource.Stop();
    }
}
