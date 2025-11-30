using UnityEngine;
using System.Collections;

public class Kamen : MonoBehaviour
{
    public float TimeFall = 0f;
    public float StartDelay = 0f;
    private Vector3 StartPosition;
    private bool ReternFall = true;
    private Rigidbody2D rb;

    void Start()
    {
        StartPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartCycleDelay());
    }

    private IEnumerator StartCycleDelay()
    {
        yield return new WaitForSeconds(StartDelay);

        while (true)
        {
            if (ReternFall)
            {
                ReternFall = false;
                yield return StartCoroutine(StartFallDelay());
            }
            yield return null;
        }
    }

    private IEnumerator StartFallDelay()
    {
        yield return new WaitForSeconds(TimeFall);
        rb.linearVelocity = Vector2.zero;
        transform.position = StartPosition;
        ReternFall = true;
    }
}
