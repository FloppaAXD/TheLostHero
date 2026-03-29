using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

public class PlayerSkinLoader : MonoBehaviour
{
    [Header("Скины (аниматоры)")]
    [SerializeField] private List<RuntimeAnimatorController> skins;

    [Header("Материалы для партиклов")]
    [SerializeField] private List<Material> materials;

    private void Start()
    {
        GameObject barrier = GameObject.Find("Barrier camera");
        PolygonCollider2D collider = barrier.GetComponent<PolygonCollider2D>();

        CinemachineConfiner2D confiner = GetComponentInChildren<CinemachineConfiner2D>();

        confiner.BoundingShape2D = collider;
        confiner.InvalidateBoundingShapeCache();

        int index = PlayerPrefs.GetInt("SelectedSkin", 0);

        Animator animator = GetComponent<Animator>();
        if (animator != null && index < skins.Count)
        {
            animator.runtimeAnimatorController = skins[index];
        }

        ParticleSystem ps = GetComponentInChildren<ParticleSystem>();
        if (ps != null && index < materials.Count)
        {
            ParticleSystemRenderer renderer = ps.GetComponent<ParticleSystemRenderer>();
            if (renderer != null)
            {
                renderer.sharedMaterial = materials[index];
            }
        }
    }
}