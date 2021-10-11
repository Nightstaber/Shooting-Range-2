using UnityEngine;
using System.Collections;

public class Recoil : MonoBehaviour
{

    // Rotations 
    private Vector3 currentRotation;
    private Vector3 targetRotation;

    // Hipfire Recoil
    [SerializeField] private float recoilX;
    [SerializeField] private float recoilY;
    [SerializeField] private float recoilZ;

    // Settings

    [SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;
    private float actualReturnSpeed;

    // Bool
    bool speedIsReduced;

    void Update()
    {
        // Always try to make the recoil go back to zero, aka mid-screen
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, actualReturnSpeed * Time.deltaTime);

        // Calculate the current rotation needed
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        // Setting the calculated rotation
        transform.localRotation = Quaternion.Euler(currentRotation);
    }
    // TODO Turn the gunspread into a camera recoil
    public void RecoilFire()
    {
        // Decrease the return to zero speed while firing
        if (!speedIsReduced) StartCoroutine(TempReturnSpeed());

        // Add camera recoil
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
    // IEnumerator for reducing the returnSpeed for 0.2 seconds after firing.
    IEnumerator TempReturnSpeed()
    {
        speedIsReduced = true;
        actualReturnSpeed = returnSpeed / 3;
        yield return new WaitForSeconds(0.2f);
        actualReturnSpeed = returnSpeed;
        speedIsReduced = false;
    }
}
