using UnityEngine;



public class MushroomEditor : MonoBehaviour
{

    [Header("Sound")]
    public AudioSource audioSource;
    public AudioClip RoundLikeARecord;

    void Start()
    {
        if (audioSource != null && RoundLikeARecord != null)
        {
            audioSource.clip = RoundLikeARecord;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void Update()
    {
        
    }
}
