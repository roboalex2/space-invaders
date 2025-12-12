using UnityEngine;

public class TargetBulletSpawner : MonoBehaviour
{
    [Header("Target")]
    [Tooltip("Object to shoot towards (direction = target.position - shooter.position)")]
    public Transform target;

    [Header("Projectile")]
    public GameObject bulletPrefab;
    public float bulletSpeed = 20f;

    [Header("Timing")]
    public float initialDelay = 0f;
    public float spawnInterval = 10f;

    [Header("Spawn Offset")]
    public float spawnDistance = 0.5f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootClip;

    [Header("Debug")]
    public bool logOnFire = true;

    void Start()
    {
        InvokeRepeating(
            nameof(SpawnAndShoot),
            Mathf.Max(0f, initialDelay),
            Mathf.Max(0.01f, spawnInterval)
        );
    }

    void SpawnAndShoot()
    {
        if (target == null || bulletPrefab == null)
        {
            if (logOnFire)
                Debug.LogWarning("[TargetBulletSpawner] Missing target or bulletPrefab.", this);
            return;
        }

        Vector3 shooterPos = transform.position;
        Vector3 direction = (target.position - shooterPos).normalized;

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Vector3 spawnPos = shooterPos + direction * spawnDistance;
        Quaternion spawnRot = Quaternion.LookRotation(direction);

        GameObject bullet = Instantiate(bulletPrefab, spawnPos, spawnRot);

        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        // 🔊 Play shooting sound (same pattern as your example)
        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }

        if (logOnFire)
        {
            Debug.Log(
                $"[TargetBulletSpawner] Fired\n" +
                $"Target: {target.name}\n" +
                $"Direction: {direction}\n" +
                $"Speed: {bulletSpeed}",
                this
            );
        }
    }
}
