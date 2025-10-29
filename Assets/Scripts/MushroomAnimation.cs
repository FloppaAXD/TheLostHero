using UnityEngine;
using System.Collections;
using System.Collections.Generic; // для List<Vector2>

[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class MushroomAnimationWithCollider : MonoBehaviour
{
    [Header("Аниматоры")]
    public Animator firstAnimator;
    public Animator secondAnimator;
    public float interval = 3f; // каждые N секунд
    public float startDelay = 0f; // начальная задержка

    private SpriteRenderer sr;
    private PolygonCollider2D poly;
    private List<Vector2> shape = new List<Vector2>();

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        poly = GetComponent<PolygonCollider2D>();

        // Скрываем второй объект (визуал + коллайдер)
        if (secondAnimator != null)
        {
            var rend = secondAnimator.GetComponent<SpriteRenderer>();
            var col = secondAnimator.GetComponent<Collider2D>();
            if (rend != null) rend.enabled = false;
            if (col != null) col.enabled = false;
        }
    }

    void LateUpdate()
    {
        if (sr.sprite == null) return;

        int shapeCount = sr.sprite.GetPhysicsShapeCount();
        poly.pathCount = shapeCount;

        for (int i = 0; i < shapeCount; i++)
        {
            shape.Clear();
            sr.sprite.GetPhysicsShape(i, shape);
            poly.SetPath(i, shape);
        }
    }

    void Start() => StartCoroutine(Loop());

    IEnumerator Loop()
    {
        // Добавляем начальную задержку
        if (startDelay > 0f)
            yield return new WaitForSeconds(startDelay);

        string firstName = firstAnimator.runtimeAnimatorController.animationClips[0].name;
        string secondName = secondAnimator.runtimeAnimatorController.animationClips[0].name;

        float firstLen = firstAnimator.runtimeAnimatorController.animationClips[0].length / firstAnimator.speed;
        float secondLen = secondAnimator.runtimeAnimatorController.animationClips[0].length / secondAnimator.speed;

        var secondRenderer = secondAnimator.GetComponent<SpriteRenderer>();
        var secondCollider = secondAnimator.GetComponent<Collider2D>();

        while (true)
        {
            float startTime = Time.time;

            // Первая анимация
            firstAnimator.Play(firstName, 0, 0f);
            yield return new WaitForSeconds(firstLen);

            // Вторая анимация
            if (secondRenderer != null) secondRenderer.enabled = true;
            if (secondCollider != null) secondCollider.enabled = true;
            secondAnimator.Play(secondName, 0, 0f);
            yield return new WaitForSeconds(secondLen);
            if (secondRenderer != null) secondRenderer.enabled = false;
            if (secondCollider != null) secondCollider.enabled = false;

            // Ждём до следующего цикла
            float elapsed = Time.time - startTime;
            if (elapsed < interval)
                yield return new WaitForSeconds(interval - elapsed);
        }
    }
}