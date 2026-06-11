using UnityEngine;
using UnityEngine.InputSystem;

public class Wepon : MonoBehaviour
{
[Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private float fireRate = 0.15f;

    [Header("Weapon Rotation")]
    [SerializeField] private Transform weaponParent;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rotationSmoothness = 15f;

    private float nextFireTime;

    private void Update()
    {
        RotateWeaponWithCamera();

        if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void RotateWeaponWithCamera()
    {
        if (weaponParent == null || playerCamera == null) return;

        Quaternion targetRotation = playerCamera.transform.rotation;

        weaponParent.rotation = Quaternion.Slerp(
            weaponParent.rotation,
            targetRotation,
            rotationSmoothness * Time.deltaTime
        );
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = firePoint.forward * bulletSpeed;
        }
    }
}
