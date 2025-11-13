using UnityEngine;

public class CornerMarker : MonoBehaviour
{
    [Tooltip("Касательная (направление движения) после поворота, например Vector2.up или Vector2.right")]
    public Vector2 newDirection = Vector2.up;

    private void Reset()
    {
        // убедимся, что у маркера есть Trigger collider
        var col = GetComponent<Collider2D>();
        if (col == null) gameObject.AddComponent<BoxCollider2D>().isTrigger = true;
        else col.isTrigger = true;
    }
}
