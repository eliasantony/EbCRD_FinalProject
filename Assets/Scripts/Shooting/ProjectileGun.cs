using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProjectileGun : MonoBehaviour
{
    public InputManager _inputManager;
    // Bullet
    [Header("Bullet Prefab")]
    public GameObject bullet;
    
    // Bullet Force
    [Header("Bullet Force")]
    public float shootForce, upwardForce;
    
    // Gun stats
    [Header("Gun Stats")]
    public string weaponName;
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;

    public int bulletsLeft;
    int bulletsShot;

    // Add fields for total ammo and ammo cost
    [Header("Ammunition")]
    public int totalAmmo;
    public int ammoCost;
    
    // Recoil
    [Header("Recoil")]
    public Rigidbody playerRb;
    public float recoilForce;
    
    // Bools
    bool shooting, readyToShoot, reloading;
    
    // Reference
    public Camera fpsCam;
    public Transform attackPoint;
    
    // Graphics
    [Header("Graphics")]
    public GameObject muzzleFlash;
    private bool muzzleFlashPlayed = false;
    
    // Bug Fixing
    public bool allowInvoke = true;
    
    // Config Loader
    public ConfigLoader configLoader;
    
    private void Awake()
    {
        _inputManager = GameObject.FindGameObjectWithTag("Player").GetComponent<InputManager>();
        // Make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
        
        configLoader.LoadGunConfig(weaponName);

        GunConfig gunConfig = configLoader.gunConfig;
        bulletsLeft = gunConfig.magazineSize;
        readyToShoot = true;

        // Use gunConfig fields in your script logic
        shootForce = gunConfig.shootForce;
        upwardForce = gunConfig.upwardForce;
        timeBetweenShooting = gunConfig.timeBetweenShooting;
        spread = gunConfig.spread;
        reloadTime = gunConfig.reloadTime;
        timeBetweenShots = gunConfig.timeBetweenShots;
        magazineSize = gunConfig.magazineSize;
        bulletsPerTap = gunConfig.bulletsPerTap;
        allowButtonHold = gunConfig.allowButtonHold;
        recoilForce = gunConfig.recoilForce;
        
        bulletsLeft = magazineSize;
        readyToShoot = true;

        // Initialize total ammo
        totalAmmo = magazineSize * 2; // Start with two magazines
    }
    
    private void Update()
    {
        MyInput();
        
        // Set ammo display
        if(UIManager.instance != null)
            UIManager.instance.UpdateAmmoDisplay(bulletsLeft/bulletsPerTap + " / " + totalAmmo/bulletsPerTap);
        
        // Set reload display
        if(UIManager.instance != null)
        {
            // Debug.Log("Reloading: " + reloading);
            if (bulletsLeft <= 0 && !reloading)
                UIManager.instance.UpdateReloadDisplay("Press R to reload");
            else if (reloading)
                UIManager.instance.UpdateReloadDisplay("Reloading...");
            else
                UIManager.instance.UpdateReloadDisplay("");
        }
    }
    
    private void MyInput()
    {
        // Check if allowed to hold down button and take button input
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);
        
        // Reloading
        if (_inputManager._onFootActions.Reload.triggered && bulletsLeft < magazineSize && !reloading) Reload();
        // Reloading automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();
        
        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = 0;
            Shoot();
        }
    }
    
    private void Shoot()
    {
        readyToShoot = false;
        
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        
        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            targetPoint = hit.point;
        }
        else
        {
            targetPoint = ray.GetPoint(75);
        }
        
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;
        
        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);
        
        // Calculate new direction with spread
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        
        // Instantiate bullet
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);
        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;
        Destroy(currentBullet, 2f);
        
        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        
        // Instantiate muzzle flash, if you have one
        if (muzzleFlash != null && !muzzleFlashPlayed)
        {
            GameObject currentMuzzleFlash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
    
            // Scale down the muzzle flash
            float scaleValue = 0.5f; // adjust this value as needed
            currentMuzzleFlash.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
    
            // Destroy the muzzle flash after 1 second
            Destroy(currentMuzzleFlash, 0.5f);
            muzzleFlashPlayed = true;
        }
        
        bulletsLeft--;
        bulletsShot++;

        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            
            // Add recoil to player
            if(playerRb)           
                playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }
        
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
        {
            Invoke("Shoot", timeBetweenShots);
        }
    }
    
    private void ResetShot()
    {
        readyToShoot = true;
        allowInvoke = true;
        muzzleFlashPlayed = false;
    }
    
    private void Reload()
    {
        if (totalAmmo > 0)
        {
            reloading = true;
            Invoke("ReloadFinished", reloadTime);
        }
        else
        {
            // Play empty ammo sound or feedback
            Debug.Log("No ammo left!");
        }
    }
    
    private void ReloadFinished()
    {
        int ammoNeeded = magazineSize - bulletsLeft;
        int ammoToReload = Mathf.Min(ammoNeeded, totalAmmo);

        bulletsLeft += ammoToReload;
        totalAmmo -= ammoToReload;
        reloading = false;
    }

    public void AddAmmo(int amount)
    {
        totalAmmo += amount;
    }
}
