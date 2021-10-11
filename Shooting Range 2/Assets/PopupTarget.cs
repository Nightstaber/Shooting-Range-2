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

    // Start is called before the first frame update
    void Start()
    {
        startRot = Quaternion.Euler(0, 90, 90);
        targetRot = Quaternion.Euler(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void TargetReady()
    {
        //startRot = Quaternion.Euler(0, 90, 90);
        targetRot = Quaternion.Euler(0, 90, 0);

        transform.rotation = targetRot;
    }
    public void TargetDown()
    {

        //startRot = Quaternion.Euler(0, 90, 0);
        targetRot = Quaternion.Euler(0, 90, 90);

        transform.rotation = targetRot;

    }
}
