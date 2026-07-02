using UnityEngine;

public class ExplosiveArrow : MonoBehaviour
{
    [Header("Explosion")]
    [SerializeField] private float explosionRadius = 5f;
    [SerializeField] private float explosionForce = 700f;
    [SerializeField] private float damage = 80f;
    [SerializeField] private LayerMask damageLayer;

    [Header("Lifetime")]
    [SerializeField] private float lifeTime = 5f;

    [Header("Effects")]
    [SerializeField] private GameObject explosionEffect;

    private bool hasExploded;

    private void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Explode();
    }

    private void Explode()
    {
        if (hasExploded) return;

        hasExploded = true;

        if (explosionEffect != null)
        {
            Instantiate(
                explosionEffect,
                transform.position,
                Quaternion.identity
            );
        }

        Collider[] hits = Physics.OverlapSphere(
            transform.position,
            explosionRadius,
            damageLayer
        );

        foreach (Collider hit in hits)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.AddExplosionForce(
                    explosionForce,
                    transform.position,
                    explosionRadius
                );
            }

            // Health health = hit.GetComponent<Health>();

            // if (health != null)
            // {
            //     health.TakeDamage(damage);
            // }
        }

        Destroy(gameObject);
    }
}