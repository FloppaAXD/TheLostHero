using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [Header("Effects")]
    [SerializeField] private ParticleSystem runDust;

    private ParticleSystem.EmissionModule runDustEmission;

    private void Awake()
    {
        if (runDust != null)
            runDustEmission = runDust.emission;
    }

    public void EnableRunDust(bool enable)
    {
        if (runDust == null) return;

        runDustEmission.enabled = enable;
    }
}
