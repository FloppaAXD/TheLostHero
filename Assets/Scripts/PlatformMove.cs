using UnityEngine;

public class PlatformMove : MonoBehaviour
{
    public float distance = 50f;
    public bool moveY = false;
    public float speed = 5f;

    private Vector3 startPos;
    private bool movingToEnd = true;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
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