using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour { 

    public float throwForce = 40f;
    public GameObject grenadePrefab;

    public AudioSource gunShotSound;
    public AudioClip gunShot;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        var grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.parent.forward * throwForce, ForceMode.VelocityChange);

    }

    public void ExplosionSound()
    {
        // Explosion sound

        gunShotSound.PlayOneShot(gunShot);
    }
}

