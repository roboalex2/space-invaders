using UnityEngine;

public class SelfDestructOnBulletHit : MonoBehaviour
{
    [Header("Bullet")]
    public string bulletTag = "Bullet";

    [Header("Explosion")]
    public GameObject explosionPrefab;
    public float destroyDelay = 0f;

    [Header("Debug")]
    public bool logHit = true;

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag(bulletTag))
            return;

        // Use this object's own position
        Vector3 explosionPos = transform.position;

        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, explosionPos, Quaternion.identity);
        }

        if (logHit)
        {
            Debug.Log(
                $"[SelfDestructOnBulletHit] Destroyed by bullet: {collision.gameObject.name}",
                this
            );
        }

        Destroy(gameObject, destroyDelay);
    }
}
