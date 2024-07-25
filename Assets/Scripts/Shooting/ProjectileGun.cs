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
    
    int bulletsLeft, bulletsShot;
    
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
    public TextMeshProUGUI ammunitionDisplay;
    public TextMeshProUGUI reloadDisplay;
    
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
        magazineSize = gunConfig.magazineSize;
        bulletsPerTap = gunConfig.bulletsPerTap;
        allowButtonHold = gunConfig.allowButtonHold;
        recoilForce = gunConfig.recoilForce;
    }
    
    private void Update()
    {
        MyInput();
        
        // Set ammo display
        if(ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft/bulletsPerTap + " / " + magazineSize/bulletsPerTap);
        
        // Set reload display
        if(reloadDisplay != null)
        {
            // Debug.Log("Reloading: " + reloading);
            if (bulletsLeft <= 0 && !reloading)
                reloadDisplay.SetText("Press R to reload");
            else if (reloading)
                reloadDisplay.SetText("Reloading...");
            else
                reloadDisplay.SetText("");
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
        Destroy(currentBullet, 5f);
        
        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);
        
        // Instantiate muzzle flash, if you have one
        if (muzzleFlash != null && !muzzleFlashPlayed)
        {
            
            GameObject currentMuzzleFlash = Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);
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
            // if(playerRb)           
                // playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
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
        Debug.Log("Reloading...");
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }
    
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
