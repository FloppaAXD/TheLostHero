using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] protected LayerMask playerLayer;
    [SerializeField] protected Vector2 detectionBoxSize = new Vector2(4f, 3f);
    [SerializeField] protected Vector2 detectionOffset = new Vector2(2f, 0f);

    protected Transform player;
    protected bool isPlayerDetected;

    protected int facingDirection = 1; // 1 = вправо, -1 = влево

    protected virtual void Update()
    {
        DetectPlayer();
    }

    protected virtual void DetectPlayer()
    {
        Vector2 center = (Vector2)transform.position +
                         new Vector2(detectionOffset.x * facingDirection, detectionOffset.y);

        Collider2D hit = Physics2D.OverlapBox(center, detectionBoxSize, 0f, playerLayer);

        if (hit != null)
        {
            player = hit.transform;
            isPlayerDetected = true;
        }
        else
        {
            player = null;
            isPlayerDetected = false;
        }
    }

    protected void Flip()
    {
        facingDirection *= -1;

        Vector3 scale = transform.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        transform.localScale = scale;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Vector2 center = (Vector2)transform.position +
                         new Vector2(detectionOffset.x * facingDirection, detectionOffset.y);

        Gizmos.DrawWireCube(center, detectionBoxSize);
    }
}