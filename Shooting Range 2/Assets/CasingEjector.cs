using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasingEjector : MonoBehaviour
{
    // Time to wait before ejecting casing
    [SerializeField]
    float casingWaitTime;

    // Which casing should be ejected?
    [SerializeField]
    GameObject casingToEject;

    // Scaling the casing
    [SerializeField]
    float caseScale;

    // Referencing FPS player for casing force direction
    GameObject fpsPlayer;

    // Grabbing character movement script
    PlayerMovement pm;

    GameObject casingBin;

    private void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        casingBin = FindObjectOfType<DroppedCasings>().gameObject;
        fpsPlayer = FindObjectOfType<PlayerMovement>().gameObject;
    }

    public void Eject()
    {

        // BUG : The force of the ejection needs to be changed to include character movement, so casings doesn't clip through weapon when moving right
        // TODO : Add a way to limit the amount of casings on the ground

        // Instantiate the casing
        var go = Instantiate(casingToEject, gameObject.transform.position, gameObject.transform.rotation);

        // Find the Rigidbody of the casing
        Rigidbody goRB = go.GetComponent<Rigidbody>();

        //Scale the casing if needed
        go.transform.localScale = go.transform.localScale * caseScale;


        /*
        // Grabbing character movement, to combat shells moving through the gun when moving right.
        Vector3 charMove = pm.GetMovement();
        Vector3 forceMove = new Vector3(0,0,0);

        // Deciding if character is moving left or right, so the force applied is normalized
        if (charMove.x < 0) forceMove = new Vector3(-charMove.x*100, charMove.y, charMove.z);
        else if (charMove.x > 0) forceMove = charMove;

        // Add the force
        goRB.AddForce(forceMove.x * 10 + 150f, 100f, forceMove.z * 10);
        */

        goRB.AddForce(fpsPlayer.transform.right * 150);
        goRB.AddForce(fpsPlayer.transform.up * 100);

        // Set the parent of the casings, so they are not all in the inspector
        go.transform.SetParent(casingBin.transform);
    }
    public void Initiate()
    {
        Invoke("Eject", casingWaitTime);
    }
}
