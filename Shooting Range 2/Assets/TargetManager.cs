using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetManager : MonoBehaviour
{
    PopupTarget[] targets;
    List<PopupTarget> availableTargets;


    // Start is called before the first frame update
    void Start()
    {
        targets = FindObjectsOfType(typeof(PopupTarget)) as PopupTarget[];
        availableTargets = new List<PopupTarget>();
         
    }

    // Update is called once per frame
    void Update()
    {
        // Reset targets
        if (Input.GetKeyDown("l"))
        {
            ResetTargets();
        }

        // Reset 1 random target
        if (Input.GetKeyDown("j"))
        {
            ResetRandomTarget(1);
        }

    }

    void ResetTargets()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            targets[i].TargetReady();
        }
    }
    // BUG Can raise the same random target twice in a row, putting down the target, then raising it again.
    void ResetRandomTarget(int group)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (!targets[i].GetRaised() && targets[i].GetTargetGroup() == group)
            {
                availableTargets.Add(targets[i]);
            }
        }

        int targetNumber = Random.Range(0, availableTargets.Count);

        if (availableTargets.Count > 0)
        {
            availableTargets[targetNumber].TargetReady();
            Debug.Log("Target number " + targetNumber + " is selected");
            availableTargets.Clear();
            
        }
        else
        {
            print("No available targets");
        }
        
    }
}


