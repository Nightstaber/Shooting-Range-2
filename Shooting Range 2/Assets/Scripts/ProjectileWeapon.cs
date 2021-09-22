using UnityEngine;
using System.Collections;
using TMPro;

public class ProjectileWeapon : MonoBehaviour
{
    // Bullet prefab
    public GameObject bullet;
    //public GameObject bulletHolePrefab;

    // Bullet force
    public float shootForce, upwardForce,oldShootforce;

    // Gun stats
    public float timeBetweenShooting, spread, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool fullAuto;

    int bulletsLeft, bulletsShot;

    // Gun Sounds
    public AudioSource weaponSound;
    public AudioClip gunShot;
    public AudioClip reloadSound;

    // Recoil
    public Rigidbody playerRb;
    public float recoilForce;

    // Bools
    bool shooting, readyToShoot, reloading, bouncing, fullAutoFire;

    //Reference
    public Camera fpsCam;
    public Transform attackPoint;
    MouseLook ml;

    // Graphics
    public ParticleSystem muzzleFlash;
    public TextMeshProUGUI ammunitionDisplay;
    public GameObject slowBullets;
    bool bulletSlowed = false;

    // Scope
    public bool scopedWeapon;
    private bool scoped = false;
    public GameObject scopeCanvas;
    public GameObject crosshairCanvas;
    public float scopedFOV = 15f;
    private float normalFOV;
    public float scopedSensMulti = 0.2f;
    public Animator animator;
    public Camera weaponCam;


    //Debuggging
    public bool allowInvoke = true;

    private void Awake()
    {
        // Make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
        fullAutoFire = fullAuto;
        ml = FindObjectOfType<MouseLook>();
    }

    private void OnEnable()
    {
        oldShootforce = shootForce;
        bulletSlowed = true;
        BulletSlow();
    }

    private void Update()
    {
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

        if (Input.GetKeyDown("v"))
        {
            if (fullAuto)
            {
                fullAutoFire = !fullAutoFire;
                //TODO Add a method to show to UI, if fullauto or singleshot.
            }
        }

        if (Input.GetButtonDown("Fire2"))
        {
            if (scopedWeapon)
            {
                Scope();
            }
        }
        
        if (Input.GetKeyDown(KeyCode.K))
        {
            BulletSlow();
        }

        slowBullets.SetActive(bulletSlowed);
    }

    private void Shoot()
    {
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

        float distanceTest = directionWithoutSpread.sqrMagnitude;
        Debug.Log("Current distance to target is: " + distanceTest);

        // Adjusting spread based on distance to target

        float spreadCorrection = 16000f / distanceTest;

        spreadCorrection = Mathf.Clamp(spreadCorrection, 1f, 6f);

        

        x = x / spreadCorrection;
        y = y / spreadCorrection;

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

        // If a target with a targetscript is hit
        /*
        Target target = hit.transform.GetComponent<Target>();
        if (target != null)
        {
            Debug.Log("Hit");
            target.TakeDamage(damage);
        }

        if (hit.rigidbody != null)
        {
            hit.rigidbody.AddForce(-hit.normal * impactForce);
        }

        GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(impactGO, 1f);
        */
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
        weaponSound.PlayOneShot(reloadSound);
        reloading = true;
        Invoke("ReloadFinished", reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }

    void Scope()
    {
        scoped = !scoped;


        if (scoped)
            StartCoroutine(OnScoped());
        else
            onUnScoped();

    }

    void onUnScoped()
    {
        animator.SetBool("isScoped", false);
        scopeCanvas.SetActive(false);
        crosshairCanvas.SetActive(true);
        weaponCam.enabled = true;
        fpsCam.fieldOfView = normalFOV;
        ml.UnScopedSens();
    }

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
