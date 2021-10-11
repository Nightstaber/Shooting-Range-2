using UnityEngine;
using System.Collections;
using TMPro;

public class ProjectileWeapon : MonoBehaviour
{
    // TODO : See how many references can be automated

    // Bullet prefab
    [Header("Bullet Prefab")]
    [SerializeField]
    GameObject bullet;
    [Space(20)]

    // Bullet force
    [Header("Bullet Force")]
    [SerializeField]
    float shootForce;
    [SerializeField]
    float upwardForce;
    [SerializeField]
    float oldShootforce;
    [Space(20)]

    // Gun stats
    [Header("Gun stats")]
    [SerializeField]
    float timeBetweenShooting;
    [SerializeField]
    float defaultSpread;
    float spread;
    [SerializeField]
    float reloadTime;
    [SerializeField]
    float timeBetweenShots;
    [SerializeField]
    int magazineSize, bulletsPerTap;
    [SerializeField]
    bool fullAuto;
    int bulletsLeft, bulletsShot;
    string showFullAuto = "Semi";
    [Space(20)]


    // Gun Sounds
    [Header("Gun sounds")]
    [SerializeField]
    AudioSource weaponSound;
    [SerializeField]
    AudioClip gunShot;
    [SerializeField]
    AudioClip reloadSound;
    [Space(20)]

    // Bools
    [Header("Bools")]
    bool shooting, readyToShoot, reloading, bouncing, fullAutoFire;
    [Space(20)]

    //Reference
    [Header("References")]
    [SerializeField]
    Camera fpsCam;
    [SerializeField]
    Transform attackPoint;
    MouseLook ml;
    CasingEjector ejectorScript;
    Recoil recoilScript;
    [Space(20)]

    // Graphics
    [Header("Graphics")]
    [SerializeField]
    ParticleSystem muzzleFlash;
    [SerializeField]
    TextMeshProUGUI ammunitionDisplay;
    [SerializeField]
    GameObject slowBullets;
    bool bulletSlowed = false;
    private AnimatorScript anim;
    [Space(20)]

    // Scope
    [Header("Scope")]
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
    [Space(20)]


    // Debuggging tool
    [Header("Debugging Tool")]
    public bool allowInvoke = true;

    private void Awake()
    {
        // Make sure magazine is full
        bulletsLeft = magazineSize;
        readyToShoot = true;
        fullAutoFire = fullAuto;

        // Create a reference to MouseLook script
        ml = FindObjectOfType<MouseLook>();

        // Make a reference to the CaseEjector script
        
        ejectorScript = gameObject.transform.Find("CasingEjectorHolder").gameObject.GetComponent<CasingEjector>();

        if (gameObject.GetComponent<Animator>())
        {
            anim = gameObject.GetComponent<AnimatorScript>();
        }
        recoilScript = GameObject.FindWithTag("MainCamera").transform.parent.GetComponent<Recoil>();
    }

    private void OnEnable()
    {
        // Make sure BulletSlowed is disabled when switching to a weapon
        oldShootforce = shootForce;
        bulletSlowed = true;
        BulletSlow();
        spread = defaultSpread;

    }

    private void Update()
    {
        // Call the MyInput method
        MyInput();

        // Evaluate full-auto or Semi
        if (fullAutoFire == true)
            showFullAuto = "Full";
        else showFullAuto = "Semi";

        // Set ammo display if it exists, including firing mode
        if (ammunitionDisplay != null)
            ammunitionDisplay.SetText(bulletsLeft / bulletsPerTap + " / " + magazineSize / bulletsPerTap + "  Mode: " + showFullAuto);
    }
    private void MyInput()
    {
        // Check if full auto or not
        if (fullAutoFire) shooting = (Input.GetKey(KeyCode.Mouse0) && !LocalPauseMenu.GameIsPaused);
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

        // Calculate the spread of firing a weapon
        if (fullAutoFire)
            CountSpread();


        // Switch between full-auto or singleshot with V key
        if (Input.GetKeyDown("v"))
        {
            if (fullAuto)
            {
                fullAutoFire = !fullAutoFire;


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

    public void CasingEject()
    {
        ejectorScript.Eject();
    }

    private void CountSpread()
    {

            if (shooting)
            {
                if (spread < 12)
                    spread += (Time.deltaTime / timeBetweenShooting);
            }
            else
            {
                if (spread > defaultSpread)
                    spread -= Time.deltaTime * 10;
            }
    }

    private void Shoot()
    {

        // No longer ready to shoot, once the shot has been fired
        readyToShoot = false;


        bulletsLeft--;

        // Check if Animator is present
        if (anim)
        {
            if (bulletsLeft <= 0)
                anim.ChangeState("LastBullet");
            else
                anim.ChangeState("Fire");
        }
              

        // Find the exact hit position using a raycast in the middle of the screen
        Ray ray = fpsCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Check if ray hits something
        Vector3 targetPoint;
        if (Physics.Raycast(ray, out hit))
            targetPoint = hit.point; // Set's the hit point in the middle of the screen
        else
            targetPoint = ray.GetPoint(75); // If no hit point, select a point away from the player, in the air.

        ejectorScript.Initiate();

        // Calculating direction from attackPoint to targetPoint
        Vector3 directionWithoutSpread = targetPoint - attackPoint.position;

        // Instantiate bullethole prefab

        //Instantiate(bulletHolePrefab, targetPoint-attackPoint.position, Quaternion.identity);

        // Calculate spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Measure the distance in the raycast, to correct the spread from low range
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

       
        bulletsShot++;

        // Invoke resetShot function (if not already invoked)
        if (allowInvoke)
        {
            Invoke("ResetShot", timeBetweenShooting);
            allowInvoke = false;
        }

        // If more than one bulletsPerTap repeat the shoot function
        if (bulletsShot < bulletsPerTap && bulletsLeft > 0)
            Invoke("Shoot", timeBetweenShots);


        // Destroy the bullet if not exploded, time based on BulletSlow or not
        if (!bulletSlowed)
            Destroy(currentBullet, 2f);
        else
            Destroy(currentBullet, 30f);

        recoilScript.RecoilFire();

    }

    private void ResetShot()
    {

        // Allow Shooting and invoking again.
        readyToShoot = true;
        allowInvoke = true; 
    }

    private void Reload()
    {
        // Play the Reload animation
        if (anim)
        {
            anim.ChangeState("Reload");
        }

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
