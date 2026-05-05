using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class TrigerBoss : MonoBehaviour
{
    [SerializeField] private GameObject BossSprite;
    [SerializeField] private GameObject Wall;

    private Animator anim;
    private bool isTriggered = false;

    void Start()
    {
        if (BossSprite != null)
            anim = BossSprite.GetComponent<Animator>();

        // Проверяем всё ли назначено
        if (anim == null)
            Debug.LogError("Animator не найден! Назначь BossSprite с компонентом Animator");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;

            if (Wall != null)
                Wall.SetActive(true);

            StartCoroutine(BossFight(other));
        }
    }

    IEnumerator BossFight(Collider2D other)
    {
        // Движение вверх
        if (BossSprite != null)
        {
            Vector3 startPos = BossSprite.transform.position;
            Vector3 endPos = startPos + Vector3.up * 15f;

            float t = 0;
            while (t < 1)
            {
                t += Time.deltaTime;
                if (BossSprite != null)
                    BossSprite.transform.position = Vector3.Lerp(startPos, endPos, t);
                yield return null;
            }
        }
        if (other != null)
            {
                CinemachineConfiner2D confiner = other.GetComponentInChildren<CinemachineConfiner2D>();
                GameObject barrier = GameObject.Find("Barrier camera 2");

                if (barrier != null && confiner != null)
                {
                    PolygonCollider2D collider = barrier.GetComponent<PolygonCollider2D>();
                    if (collider != null)
                    {
                        confiner.BoundingShape2D = collider;
                        confiner.InvalidateBoundingShapeCache();
                    }
                }
        }
        // Анимации (с проверкой)
        if (anim != null)
        {
            yield return StartCoroutine(PlayAnim("Attack1"));
            yield return StartCoroutine(PlayAnim("Attack2"));
            yield return StartCoroutine(PlayAnim("Hurt"));
            yield return StartCoroutine(PlayAnim("Die"));
        }
        else
        {
            Debug.LogError("Animator null, анимации не будут проиграны!");
        }

        
    }

    IEnumerator PlayAnim(string name)
    {
        if (anim == null) yield break;

        anim.Play(name);

        float length = GetAnimLength(name);
        yield return new WaitForSeconds(length);
    }

    float GetAnimLength(string name)
    {
        if (anim == null || anim.runtimeAnimatorController == null)
            return 1f;

        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (AnimationClip clip in clips)
        {
            if (clip.name == name)
                return clip.length;
        }
        return 1f;
    }
}