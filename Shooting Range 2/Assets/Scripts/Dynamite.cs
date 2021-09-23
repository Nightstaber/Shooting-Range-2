using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dynamite : MonoBehaviour
{

    float delay = 3f;
    float blastRadius = 5f;
    float explosiveForce = 700f;
    float explosiveDamage = 100f;

    [SerializeField]
    GameObject explosionEffect;

    float countdown;
    bool hasExploded = false;

    [SerializeField]
    AudioSource explosionSound;
    [SerializeField]
    AudioClip explosionClip;
    
   
    // Start is called before the first frame update
    void Start()
    {
        // Set the delay countdown
        countdown = delay;

    }

    // Update is called once per frame
    void Update()
    {
        // When countdown is over, explode the dynamite
        countdown -= Time.deltaTime;
        if (countdown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }

    }

    void Explode()
    {

        // Instantiate explosion
        // var explosion = Instantiate(explosionEffect, transform.position, transform.rotation,gameObject.transform);
        Instantiate(explosionEffect, transform.position, transform.rotation, gameObject.transform);

        // Play audio at location
        AudioSource.PlayClipAtPoint(explosionClip, transform.position, 1f);

        // Get Nearby Objects to destroy
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

        // Find nearby objects to move
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

        // Hide the meshrenderer to allow explosion to go off before deleting it
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        mr.enabled = false;

        Destroy(gameObject, 3f);
       
    }

}

