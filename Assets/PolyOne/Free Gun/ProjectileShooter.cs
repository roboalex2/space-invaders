using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ProjectileShooter : MonoBehaviour
{
    [Header("Projectile")]
    public GameObject projectilePrefab;
    public Transform spawnPoint;
    public float launchSpeed = 15f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootClip;

    public void OnActivated(ActivateEventArgs args)
    {
        Debug.Log("[ProjectileShooter] Activated by: " + args.interactorObject); // <- key debug
        Shoot();
    }

    private void Shoot()
    {
        if (projectilePrefab == null || spawnPoint == null)
        {
            Debug.LogWarning("[ProjectileShooter] Missing projectilePrefab or spawnPoint");
            return;
        }

        GameObject proj = Instantiate(projectilePrefab, spawnPoint.position, spawnPoint.rotation);

        if (proj.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = spawnPoint.forward * launchSpeed;
        }

        if (audioSource != null && shootClip != null)
        {
            audioSource.PlayOneShot(shootClip);
        }
    }
}
