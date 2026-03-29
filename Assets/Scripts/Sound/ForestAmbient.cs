using UnityEngine;

public class ForestAmbient : MonoBehaviour
{
    [SerializeField] private AudioClip ambientClip;
    private AudioSource ambientSource;

    private void Awake()
    {
        // создаём AudioSource на этом объекте
        ambientSource = gameObject.AddComponent<AudioSource>();

        ambientSource.clip = ambientClip;
        ambientSource.loop = true;
        ambientSource.playOnAwake = false;

        ambientSource.volume = 1f;
        ambientSource.spatialBlend = 0f; // Эмбиент всегда 2D !!!
        ambientSource.priority = 256;    // низкий приоритет, но не выключится

        ambientSource.Play();
    }
}
