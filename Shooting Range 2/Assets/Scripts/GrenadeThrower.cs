using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeThrower : MonoBehaviour { 

    // Throwforce and prefab
    float throwForce = 20f;
    [SerializeField]
    GameObject grenadePrefab;

    // Audioclip 
    [SerializeField]
    AudioClip gunShot;

    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {
        // Throw grenade on mouseclick
        if (Input.GetMouseButtonDown(0))
        {
            ThrowGrenade();
        }
    }

    void ThrowGrenade()
    {
        // Instantiate grenade, find Rigidbody and addforce to throw it away from the player
        var grenade = Instantiate(grenadePrefab, transform.position, transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(transform.parent.forward * throwForce, ForceMode.VelocityChange);

    }
}

