using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Wepon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Gun currentGun;

    [Header("Bullet Pool")]
    [SerializeField] private int poolSize = 60;
    [SerializeField] private Transform bulletPoolParent;

    [Header("Muzzle Effects")]
    [SerializeField] private ParticleSystem[] muzzleFlashes;

    private GameObject[] bulletPool;
    private int bulletIndex;

    private float nextFireTime;
    private bool isReloading;
    private bool isBursting;

    private void Start()
    {
        CreateBulletPool();

        if (currentGun == null)
            currentGun = GetComponentInChildren<Gun>();
    }

    private void Update()
    {
        if (currentGun == null) return;

        HandleReloadInput();
        HandleShootInput();
    }

    private void CreateBulletPool()
    {
        bulletPool = new GameObject[poolSize];

        for (int i = 0; i < poolSize; i++)
        {
            GameObject bullet = Instantiate(
                bulletPrefab,
                Vector3.zero,
                Quaternion.identity,
                bulletPoolParent
            );

            bullet.SetActive(false);
            bulletPool[i] = bullet;
        }
    }

    private GameObject GetBulletFromPool()
    {
        GameObject bullet = bulletPool[bulletIndex];

        bulletIndex++;

        if (bulletIndex >= bulletPool.Length)
            bulletIndex = 0;

        return bullet;
    }

    private void HandleReloadInput()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            StartReload();
        }
    }

    private void HandleShootInput()
    {
        if (isReloading) return;

        if (currentGun.currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        switch (currentGun.fireMode)
        {
            case GunFireMode.Single:
                if (Mouse.current.leftButton.wasPressedThisFrame)
                    TryShoot();
                break;

            case GunFireMode.Auto:
                if (Mouse.current.leftButton.isPressed)
                    TryShoot();
                break;

            case GunFireMode.Burst:
                if (Mouse.current.leftButton.wasPressedThisFrame && !isBursting)
                    StartCoroutine(BurstFire());
                break;
        }
    }

    private void TryShoot()
    {
        if (Time.time < nextFireTime) return;

        Shoot();
        nextFireTime = Time.time + currentGun.fireRate;
    }

    private void Shoot()
    {
        if (currentGun.currentAmmo <= 0) return;

        currentGun.currentAmmo--;

        Vector3 shootDirection = GetShootDirection();

        GameObject bullet = GetBulletFromPool();

        bullet.transform.SetPositionAndRotation(
            currentGun.firePoint.position,
            Quaternion.LookRotation(shootDirection)
        );

        bullet.SetActive(true);

        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        if (bulletRb != null)
        {
            bulletRb.linearVelocity = Vector3.zero;
            bulletRb.angularVelocity = Vector3.zero;
            bulletRb.linearVelocity = shootDirection * currentGun.bulletSpeed;
        }

        PlayMuzzleFlash();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlayShoot();

        Debug.Log($"Ammo: {currentGun.currentAmmo}/{currentGun.reserveAmmo}");
    }

    private Vector3 GetShootDirection()
    {
        Vector3 direction = playerCamera.transform.forward;

        direction += playerCamera.transform.right * Random.Range(-currentGun.spread, currentGun.spread);
        direction += playerCamera.transform.up * Random.Range(-currentGun.spread, currentGun.spread);

        return direction.normalized;
    }

    private IEnumerator BurstFire()
    {
        isBursting = true;

        for (int i = 0; i < currentGun.bulletsPerBurst; i++)
        {
            if (currentGun.currentAmmo <= 0)
                break;

            Shoot();

            yield return new WaitForSeconds(currentGun.burstDelay);
        }

        nextFireTime = Time.time + currentGun.fireRate;
        isBursting = false;
    }

    private void StartReload()
    {
        if (isReloading) return;
        if (currentGun.currentAmmo == currentGun.magazineSize) return;
        if (currentGun.reserveAmmo <= 0) return;

        StartCoroutine(Reload());
    }
    public void SetCurrentGun(Gun gun)
    {
    currentGun = gun;
    isReloading = false;    
    isBursting = false;
    }

    private IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(currentGun.reloadTime);

        int ammoNeeded = currentGun.magazineSize - currentGun.currentAmmo;
        int ammoToLoad = Mathf.Min(ammoNeeded, currentGun.reserveAmmo);

        currentGun.currentAmmo += ammoToLoad;
        currentGun.reserveAmmo -= ammoToLoad;

        isReloading = false;

        Debug.Log($"Reloaded: {currentGun.currentAmmo}/{currentGun.reserveAmmo}");
    }

    private void PlayMuzzleFlash()
    {
        int index = currentGun.muzzleFlashIndex;

        if (muzzleFlashes == null) return;
        if (index < 0 || index >= muzzleFlashes.Length) return;
        if (muzzleFlashes[index] == null) return;

        muzzleFlashes[index].Play();
    }


}