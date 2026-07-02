using UnityEngine;

public class ExplosiveBowAbility : AbilityBase
{
    [Header("Explosive Bow")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject explosiveArrowPrefab;
    [SerializeField] private float arrowSpeed = 35f;
    [SerializeField] private float spread = 0.01f;

    public override void Activate()
    {
        if (isOnCooldown) return;

        ShootExplosiveArrow();

        StartCoroutine(CooldownRoutine());
    }

    private void ShootExplosiveArrow()
    {
        Vector3 direction = playerCamera.transform.forward;

        direction += playerCamera.transform.right * Random.Range(-spread, spread);
        direction += playerCamera.transform.up * Random.Range(-spread, spread);
        direction.Normalize();

        GameObject arrow = Instantiate(
            explosiveArrowPrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        Rigidbody rb = arrow.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = direction * arrowSpeed;
        }

        Debug.Log("Adrien used Explosive Bow!");
    }
}