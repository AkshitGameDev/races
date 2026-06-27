using UnityEngine;

public enum GunFireMode
{
    Single,
    Auto,
    Burst
}

public class Gun : MonoBehaviour
{
    [Header("Identity")]
    public int gunIndex;

    [Header("Shooting Data")]
    public GunFireMode fireMode = GunFireMode.Auto;
    public Transform firePoint;
    public float bulletSpeed = 40f;
    public float fireRate = 0.15f;
    public float spread = 0.02f;

    [Header("Burst")]
    public int bulletsPerBurst = 3;
    public float burstDelay = 0.06f;

    [Header("Ammo")]
    public int magazineSize = 30;
    public int currentAmmo = 30;
    public int reserveAmmo = 90;
    public float reloadTime = 1.5f;

    [Header("Effects")]
    public int muzzleFlashIndex = 0;

    private void Awake()
    {
        if (currentAmmo <= 0)
            currentAmmo = magazineSize;
    }

   
}