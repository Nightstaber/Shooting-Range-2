using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTarget : MonoBehaviour
{
    Vector3 startVector;
    Vector3 readyVector;
    [SerializeField] float prepTime;

    Quaternion startRot;
    Quaternion targetRot;
    float popUpTime = 1f;
    float speedMultiplier = 2f;
    bool raised = false;
    bool pointsAllowed;

    // Start is called before the first frame update
    void Start()
    {
        startRot = Quaternion.Euler(0, 90, 90);
        targetRot = Quaternion.Euler(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
       if (popUpTime < 1)
        {


            transform.rotation = Quaternion.Slerp(startRot, targetRot, popUpTime);
            popUpTime += Time.deltaTime * speedMultiplier;
            Mathf.Clamp(popUpTime, 0, 1f);
        }
            
    }

    public void TargetReady()
    {
        startRot = Quaternion.Euler(0, 90, 90);
        targetRot = Quaternion.Euler(0, 90, 0);

        popUpTime = 0f;
        speedMultiplier = 2f;

        raised = true;
    }
    public void TargetDown()
    {

        startRot = Quaternion.Euler(0, 90, 0);
        targetRot = Quaternion.Euler(0, 90, 90);

        popUpTime = 0f;
        speedMultiplier = 4f;

        raised = false;
    }
    public void SetRaised(bool _raised)
    {
        raised = _raised;

    }

    public bool GetRaised()
    {
        return raised;
    }
}
