using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{

    public float delay = 3f;
    public float blastRadius = 5f;
    public float explosiveForce = 700f;
    public float explosiveDamage = 100f;

    public GameObject explosionEffect;

    float countdown;
    bool hasExploded = false;

    public AudioSource explosionSound;
    public AudioClip explosionClip;
    
    //GrenadeThrower explosionSound = new GrenadeThrower();


    // Start is called before the first frame update
    void Start()
    {
        countdown = delay;

    }

    // Update is called once per frame
    void Update()
    {
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;

        }
        /*
        if (health <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
        */
    }

    void Explode()
    {

        // Show effect
        var explosion = Instantiate(explosionEffect, transform.position, transform.rotation,gameObject.transform);
        //explosion.transform.parent = gameObject.transform;
        


        AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);

        // Get Nearby Objects

        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, blastRadius);

        // Add Damage to nearby objects

        foreach (Collider nearbyObject in collidersToDestroy)
        {

            Target dest = nearbyObject.GetComponent<Target>();
            if (dest != null)
            {
                dest.TakeDamage(explosiveDamage);
            }
        }

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, blastRadius);

        // Add force

        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosiveForce, transform.position, blastRadius);
            }
        }






        // Remove Grenade
        StartCoroutine(destroy());
    }

    IEnumerator destroy()
    {
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        mr.enabled = false;

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);

    }
}

