using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource musicSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

<<<<<<< HEAD
    public void PlayMusic(AudioClip clip, float volume = 0.2f)
=======
    public void PlayMusic(AudioClip clip, float volume = 0.05f)
>>>>>>> ea1a2eed7e5c779cc686e2db429636d7cbb2de22
    {
        if (musicSource.clip == clip) return; 

        musicSource.clip = clip;
        musicSource.volume = volume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }
}
