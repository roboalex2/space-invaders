using UnityEngine;

public class BulletDespawn : MonoBehaviour
{
    [Header("Lifetime")]
    public float lifetime = 20f;

    [Header("Collision Despawn")]
    public bool despawnOnCollision = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!despawnOnCollision)
            return;

        Destroy(gameObject);

        Debug.Log(
         $"[BulletDespawn] Deleted bullet\n" +
         $"Shooter: {name}\n" +
         $"Target: {collision}",
         this
         );
    }
}
