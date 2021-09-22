using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowedBulletsEnable : MonoBehaviour
{

    public void SlowedEnable(bool status)
    {
        gameObject.SetActive(status);
    }

}
