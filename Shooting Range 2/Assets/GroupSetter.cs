using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupSetter : MonoBehaviour
{
    // References
    TargetGroupManager groupManager;

    // Zone selection in inspector
    [SerializeField] int currentZone;

    // Start is called before the first frame update
    void Start()
    {
        groupManager = gameObject.GetComponentInParent<TargetGroupManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            groupManager.SetCurrentZone(currentZone);
            print("Current zone changed to: " + currentZone);
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            groupManager.SetCurrentZone(0);
            print("Current zone changed to: 0");
        }
        
    }
    public int GetCurrentZone()
    {
        return currentZone;
    }
}
