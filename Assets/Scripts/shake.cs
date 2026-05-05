using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class Shake : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            CinemachineConfiner2D confiner = other.GetComponentInChildren<CinemachineConfiner2D>();
            GameObject barrier = GameObject.Find("Barrier camera 3");

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
    }
}
