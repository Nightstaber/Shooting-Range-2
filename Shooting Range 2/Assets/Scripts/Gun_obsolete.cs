using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Gun : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float firerate = 10f;
    public float impactForce = 75f;

    public int maxAmmo = 10;
    private int currentAmmo;
    public float reloadTime = 1f;
    private bool isReloading = false;

    public bool fullAuto;
    private bool fullAutoFire = false;
    public bool scopedWeapon;
    private bool scoped = false;


    public Animator animator;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public GameObject scopeCanvas;
    public GameObject crosshairCanvas;

    private float nextTimeToFire = 0f;

    public AudioSource gunShotSound;
    public AudioClip gunShot;

    public Text ammoCount;
    public Camera weaponCam;

    public float scopedFOV = 15f;
    private float normalFOV;

    // Update is called once per frame


    private void Start()
    {
        currentAmmo = maxAmmo;
        AmmoCount();
        //animator.SetBool("isFiring", false);
    }

    private void OnEnable()
    {
        isReloading = false;
        animator.SetBool(("Reloading"), false);
    }

    void Update()
    {
        //animator.SetBool("isFiring", false);


        if (isReloading)
            return;

        if (currentAmmo <= 0)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetKeyDown("v"))
        {
            if (fullAuto) 
            { 
                fullAutoFire = !fullAutoFire;
               
            }
        }

        if (fullAutoFire)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && !LocalPauseMenu.GameIsPaused)
            {
                nextTimeToFire = Time.time + 1f / firerate;
                Shoot();
            }
        }
        else
                if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire && !LocalPauseMenu.GameIsPaused)
        {
            nextTimeToFire = Time.time + 1f / firerate;
            Shoot();
        }

        if (Input.GetKeyDown("r") && currentAmmo != maxAmmo)
            StartCoroutine(Reload());

        if (Input.GetButtonDown("Fire2"))
        {
            if (scopedWeapon)
            {
                Scope();
            }
        }



    }

    IEnumerator Reload()
    {

        if (scoped)
        onUnScoped();

        isReloading = true;

        animator.SetBool("Reloading", true);

        yield return new WaitForSeconds(reloadTime - .25f);

        animator.SetBool("Reloading", false);

        yield return new WaitForSeconds(.25f);

        currentAmmo = maxAmmo;

        AmmoCount();

        isReloading = false;
    }

    void AmmoCount()
    {
        ammoCount.text = currentAmmo + " / " + maxAmmo;
    }

    void Shoot()
    {
        //animator.SetBool("isFiring", true);

        gunShotSound.PlayOneShot(gunShot);

        muzzleFlash.Play();

        currentAmmo--;

        AmmoCount();

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            //Debug.Log(hit.transform.name);

            

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
        }
        //animator.SetBool("isFiring", false);
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
    }

    IEnumerator OnScoped()
    {
        animator.SetBool("isScoped", true);
        yield return new WaitForSeconds(.25f);
        scopeCanvas.SetActive(true);
        crosshairCanvas.SetActive(false);
        weaponCam.enabled = false;

        normalFOV = fpsCam.fieldOfView;
        fpsCam.fieldOfView = scopedFOV;
    }
}

