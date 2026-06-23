using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wepon : MonoBehaviour
{
    public enum FireMode
    {
        Single,
        Auto
    }

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 40f;
    [SerializeField] private float fireRate = 0.15f;
    [SerializeField] private FireMode fireMode = FireMode.Auto;

    [Header("Ammo")]
    [SerializeField] private int magSize = 30;
    [SerializeField] private int reserveAmmo = 90;
    [SerializeField] private float reloadTime = 1.5f;

    [Header("Spread")]
    [SerializeField] private float spread = 0.02f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem muzzleFlash;

    [Header("Weapon Rotation")]
    [SerializeField] private Transform weaponParent;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float rotationSmoothness = 15f;

    private int currentAmmo;
    private bool isReloading;
    private float nextFireTime;
    private bool equipped = false;

    public void SetEquipped(bool value)
    {
        equipped = value;
    }

    private void Start()
    {
        currentAmmo = magSize;
    }

    private void Update()
    {
        if (!equipped)
            return;

        RotateWeaponWithCamera();

        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartCoroutine(Reload());
        }

        if (isReloading)
            return;

        switch (fireMode)
        {
            case FireMode.Auto:
                if (Mouse.current.leftButton.isPressed && Time.time >= nextFireTime)
                {
                    Shoot();
                }
                break;

            case FireMode.Single:
                if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextFireTime)
                {
                    Shoot();
                }
                break;
        }
    }

    private void RotateWeaponWithCamera()
    {
        if (weaponParent == null || playerCamera == null)
            return;

        Quaternion targetRotation = playerCamera.transform.rotation;

        weaponParent.rotation = Quaternion.Slerp(
            weaponParent.rotation,
            targetRotation,
            rotationSmoothness * Time.deltaTime
        );
    }

    private void Shoot()
    {
        if (currentAmmo <= 0)
            return;

        currentAmmo--;
        nextFireTime = Time.time + fireRate;

        Vector3 direction = firePoint.forward;

        direction += firePoint.right * Random.Range(-spread, spread);
        direction += firePoint.up * Random.Range(-spread, spread);

        GameObject bullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            Quaternion.LookRotation(direction)
        );

        AudioManager.Instance.PlayShoot();

        if (muzzleFlash != null)
            muzzleFlash.Play();

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = direction.normalized * bulletSpeed;
        }

        Debug.Log($"Ammo: {currentAmmo}/{reserveAmmo}");
    }

    private IEnumerator Reload()
    {
        if (currentAmmo == magSize)
            yield break;

        if (reserveAmmo <= 0)
            yield break;

        isReloading = true;

        yield return new WaitForSeconds(reloadTime);

        int ammoNeeded = magSize - currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, reserveAmmo);

        currentAmmo += ammoToLoad;
        reserveAmmo -= ammoToLoad;

        isReloading = false;
    }

    public int CurrentAmmo => currentAmmo;
    public int ReserveAmmo => reserveAmmo;
    public bool IsReloading => isReloading;
}