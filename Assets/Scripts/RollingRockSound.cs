using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class RollingRockSound : MonoBehaviour
{
    [Header("Audio Settings")]
    public AudioSource rockSource;
    public AudioClip rollingLoop;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float minVolume = 0f;
    [SerializeField] private float maxVolume = 1f;
    [SerializeField] private float silentSpeedThreshold = 0.1f;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rockSource != null && rollingLoop != null)
        {
            rockSource.clip = rollingLoop;
            rockSource.loop = true;
            rockSource.volume = 0f;
            rockSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        if (rockSource == null) return;

        float speed = rb.linearVelocity.magnitude;

        // если камень не двигаетс€ Ч звук полностью выключен
        if (speed < silentSpeedThreshold)
        {
            if (rockSource.isPlaying)
                rockSource.Stop();

            return;
        }

        // если движетс€ Ч звук включаем
        if (!rockSource.isPlaying)
            rockSource.Play();

        // нормализуем скорость от 0 до 1
        float normalized = Mathf.Clamp01(speed / maxSpeed);

        // регулируем громкость в зависимости от скорости
        rockSource.volume = Mathf.Lerp(minVolume, maxVolume, normalized);
        Debug.Log(speed);
    }
}
