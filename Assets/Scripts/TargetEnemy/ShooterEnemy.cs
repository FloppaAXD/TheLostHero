using UnityEngine;

public class ShooterEnemy : EnemyBase
{
    [Header("Shooting")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float shootCooldown = 2f;

    private float shootTimer;

    protected override void Update()
    {
        base.Update();

        shootTimer -= Time.deltaTime;

        if (isPlayerDetected && player != null)
        {
            ShootAtPlayer();
        }
    }

    private void ShootAtPlayer()
    {
        if (shootTimer > 0) return;

        Vector2 dir = (player.position - shootPoint.position).normalized;

        GameObject proj = Instantiate(projectilePrefab, shootPoint.position, Quaternion.identity);

        proj.GetComponent<Projectile>().SetDirection(dir);

        shootTimer = shootCooldown;
    }
}