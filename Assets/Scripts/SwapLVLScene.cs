using UnityEngine;
using System.Collections;

public class SwapLVLScene : MonoBehaviour
{
    [SerializeField] private RectTransform targetObject;
    [SerializeField] private float moveDistance = 800f;
    [SerializeField] private float moveDuration = 0.5f;

    private bool isMoving = false;

    public void MoveLeftSmooth()
    {
        if (!isMoving)
            StartCoroutine(SmoothMove(-moveDistance));
    }

    public void MoveRightSmooth()
    {
        if (!isMoving)
            StartCoroutine(SmoothMove(moveDistance));
    }

    private IEnumerator SmoothMove(float distance)
    {
        isMoving = true;

        Vector2 startPosition = targetObject.anchoredPosition;
        Vector2 endPosition = startPosition + new Vector2(distance, 0);
        float elapsedTime = 0f;

        while (elapsedTime < moveDuration)
        {
            float t = elapsedTime / moveDuration;
            targetObject.anchoredPosition = Vector2.Lerp(startPosition, endPosition, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        targetObject.anchoredPosition = endPosition;
        isMoving = false;
    }
}
