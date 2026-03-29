using UnityEngine;
using System.Collections;

public class PlatformMove : MonoBehaviour
{
    public float distance = 50f;
    public bool moveY = false;
    public float speed = 5f;
    public float startDelay = 0f;

    private Vector3 startPos;
    private bool movingToEnd = true;
    private bool isMoving = false;

    void Start()
    {
        startPos = transform.position;

        if (startDelay > 0f)
        {
            StartCoroutine(StartMovingAfterDelay());
        }
        else
        {
            isMoving = true;
        }
    }

    void Update()
    {
        if (!isMoving) return;

        Vector3 target;

        if (moveY)
        {
            target = startPos + new Vector3(0f, movingToEnd ? distance : 0f, 0f);
        }
        else
        {
            target = startPos + new Vector3(movingToEnd ? distance : 0f, 0f, 0f);
        }

        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            movingToEnd = !movingToEnd;
        }
    }

    private IEnumerator StartMovingAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        isMoving = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("GroundCheckPoint") && other.transform.parent != null)
        {
            other.transform.parent.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("GroundCheckPoint") && other.transform.parent != null)
        {
            other.transform.parent.SetParent(null);
        }
    }
}