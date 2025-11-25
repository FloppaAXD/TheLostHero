using UnityEngine;
using UnityEngine.Tilemaps;

public class DisableTilemapColliderOnTrigger : MonoBehaviour
{
    public TilemapCollider2D targetTilemapCollider;
    public GameObject playerObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == playerObject)
        {
            if (targetTilemapCollider != null)
            {
                targetTilemapCollider.enabled = false;
            }
        }
    }
}