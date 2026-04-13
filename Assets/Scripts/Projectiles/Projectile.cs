using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] protected float speed = 8f;
    [SerializeField] protected float lifeTime = 5f;

    protected Vector2 direction;

    protected virtual void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    public virtual void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    protected virtual void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("ground") ||
            other.gameObject.layer == LayerMask.NameToLayer("Danger"))
        {
            Destroy(gameObject);
        }
    }
}