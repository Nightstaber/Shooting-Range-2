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

        // Add up and sideway force
        goRB.AddForce(fpsPlayer.transform.right * 150);
        goRB.AddForce(fpsPlayer.transform.up * 100);

        Vector3 randomRotation = new Vector3(Random.Range(90f, 120f), 0, 0);
        goRB.AddTorque(randomRotation * 5, ForceMode.Impulse);

        // Set the parent of the casings, so they are not all in the inspector
        go.transform.SetParent(casingBin.transform);
    }
    public void Initiate()
    {
        Invoke("Eject", casingWaitTime);
    }
}
