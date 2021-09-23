using UnityEngine;
using System.Collections;
using TMPro;

public class ProjectileWeapon : MonoBehaviour
{
    // Bullet prefab
    [SerializeField]
    GameObject bullet;

    // Bullet force
    [SerializeField]
    float shootForce, upwardForce, oldShootforce;

    // Gun stats
    [SerializeField]
    float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    [SerializeField]
    int magazineSize, bulletsPerTap;
    [SerializeField]
    bool fullAuto;
    int bulletsLeft, bulletsShot;

    // Gun Sounds
    [SerializeField]
    AudioSource weaponSound;
    [SerializeField]
    AudioClip gunShot;
    [SerializeField]
    AudioClip reloadSound;

    // Bools
    bool shooting, readyToShoot, reloading, bouncing, fullAutoFire;

    //Reference
    [SerializeField]
    Camera fpsCam;
    [SerializeField]
    Transform attackPoint;
    MouseLook ml;

    // Graphics
    [SerializeField]
    ParticleSystem muzzleFlash;
    [SerializeField]
    TextMeshProUGUI ammunitionDisplay;
    [SerializeField]
    GameObject slowBullets;
    bool bulletSlowed = false;

    // Scope
    [SerializeField]
    bool scopedWeapon;
    bool scoped = false;
    [SerializeField]
    GameObject scopeCanvas;
    [SerializeField]
    GameObject crosshairCanvas;
    float scopedFOV = 15f;
    float normalFOV;
    float scopedSensMulti = 0.2f;
    [SerializeField]
    Animator animator;
    [SerializeField]
    Camera weaponCam;


    // Debuggging tool
    public bool allowInvoke = true;

    private void Awake()
    {
        // Make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
        fullAutoFire = fullAuto;

        // Create a reference to MouseLook script
        ml = FindObjectOfType<MouseLook>();
    }

    private void OnEnable()
    {
        // Make sure BulletSlowed is disabled when switching to a weapon
        oldShootforce = shootForce;
        bulletSlowed = true;
        BulletSlow();
    }

    private void Update()
    {
        // Call the MyInput method
        MyInput();

        // Set ammo display if it exists
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap);
    }
    private void MyInput()
    {
        // Check if full auto or not
        if (fullAuto) shooting = (Input.GetKey(KeyCode.Mouse0) && !LocalPauseMenu.GameIsPaused);
        else shooting = (Input.GetKeyDown(KeyCode.Mouse0) && !LocalPauseMenu.GameIsPaused);

        // Reloading
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        // Reload automatically when trying to shoot without ammo
        if (readyToShoot && shooting && !reloading && bulletsLeft <= 0) Reload();

        // Shooting
        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            // Set bullets fired to 0
            bulletsShot = 0;
            
            Shoot();
        }
        // Switch between full-auto or singleshot with V key
        if (Input.GetKeyDown("v"))
        {
            if (fullAuto)
            {
                fullAutoFire = !fullAutoFire;
                //TODO Add a method to show to UI, if fullauto or singleshot.
            }
        }

        // If weapon is scoped, bring forth the scope
        if (Input.GetButtonDown("Fire2"))
        {
            if (scopedWeapon)
            {
                Scope();
            }
        }
        
        // Press K to start BulletSlow, which allows you to inspect the bullets in the air
        if (Input.GetKeyDown(KeyCode.K))
        {
            BulletSlow();
        }
        // Enable or disable the red text that shows if BulletSlow is enabled
        slowBullets.SetActive(bulletSlowed);
    }

    private void Shoot()
    {
        // No longer ready to shoot, once the shot has been fired
        readyToShoot = false;

        // Find the exact hit position using a raycast in the middle of the screen
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point; // Set's the hit point in the middle of the screen
        else
            targetPoint = ray.GetPoint(75); // If no hit point, select a point away from the player, in the air.



        // Calculating direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Instantiate bullethole prefab

        //Instantiate(bulletHolePrefab, targetPoint-attackPoint.position, Quaternion.identity);

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Measure the distance in the raycast, to correct the spred from low range
        float distanceTest = directionWithoutSpread.sqrMagnitude;

        // Adjusting spread based on distance to target
        float spreadCorrection = 16000f / distanceTest;

        // Make sure spread multiplier is between 1 and 6
        spreadCorrection = Mathf.Clamp(spreadCorrection, 1f, 6f);

        // Calculate the new spread value
        x = x / spreadCorrection;
        y = y / spreadCorrection;

        // If distance is below 20 (point blank), no spread will be added.
        if (distanceTest < 20f)
        {
            x = 0f;
            y = 0f;
        }

        // Calculate new direction with spread, by adding the spread vector to the direction
        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);

        // Instantiate bullet / projectile
        GameObject currentBullet = Instantiate(bullet, attackPoint.position, Quaternion.identity);

        // Play gunShotSound
        weaponSound.PlayOneShot(gunShot);

        // Rotate bullet to shoot direction
        currentBullet.transform.forward = directionWithSpread.normalized;

        // Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(directionWithSpread.normalized * shootForce, ForceMode.Impulse);
        if(bouncing)
        currentBullet.GetComponent<Rigidbody>().AddForce(fpsCam.transform.up * upwardForce, ForceMode.Impulse);


        // Instantiate muzzleflash if there is one
        if (muzzleFlash != null)
            muzzleFlash.Play();

        bulletsLeft--;
        bulletsShot++;

        // Invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
            // Add recoil to player
            //playerRb.AddForce(-directionWithSpread.normalized * recoilForce, ForceMode.Impulse);
        }

        // If more than one bulletsPerTap repeat the shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);


        // Destroy the bullet if not exploded, time based on BulletSlow or not
        if (!bulletSlowed)
            Destroy(currentBullet, 2f);
        else
            Destroy(currentBullet, 30f);
    }

    private void ResetShot()
    {

        // Allow Shooting and invoking again.
        readyToShoot = true;
        allowInvoke = true; 
    }

    private void Reload()
    {
        // Play reloadsound, set reloading to true, and call ReloadFinishehd after reloadTime
        weaponSound.PlayOneShot(reloadSound);
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    // Finished reloading
    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    // Decides whether to scope or unscope
    void Scope()
    {
        scoped = !scoped;


        if (scoped)
            StartCoroutine(OnScoped());
        else
            onUnScoped();

    }

    // Unscope the gun, change FOV to normal, and enable crosshair
    void onUnScoped()
    {
        animator.SetBool("isScoped", false);
        scopeCanvas.SetActive(false);
        crosshairCanvas.SetActive(true);
        weaponCam.enabled = true;
        fpsCam.fieldOfView = normalFOV;
        ml.UnScopedSens();
    }

    // Scope the gun, change FOV to zoom in, and disable the crosshair
    IEnumerator OnScoped()
    {
        animator.SetBool("isScoped", true);
        yield return new WaitForSeconds(.25f);
        scopeCanvas.SetActive(true);
        crosshairCanvas.SetActive(false);
        weaponCam.enabled = false;
        ml.ScopedSens(scopedSensMulti);

        normalFOV = fpsCam.fieldOfView;
        fpsCam.fieldOfView = scopedFOV;
    }
    // BulletSlow method. Sets low shootingforce, saving the old force, and enlarging the projectile
    private void BulletSlow()
    {
        bulletSlowed = !bulletSlowed;

        if (bulletSlowed)
        {
            oldShootforce = shootForce;
            shootForce = 1f;
            bullet.transform.localScale = new Vector3(5, 5, 5);
        }
        else
        {
            shootForce = oldShootforce;
            bullet.transform.localScale = new Vector3(1, 1, 1);
        }
        
    }
}
