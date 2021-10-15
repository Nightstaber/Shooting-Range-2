using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetGroupManager : MonoBehaviour
{
    Dictionary<int, string> groupSelection = new Dictionary<int, string>();
    int currentZone = 0;

    private void Start()
    {
        groupSelection.Add(0, "No shooting area");
        groupSelection.Add(1, "Initial Popup Target Area");
        groupSelection.Add(2, "Testzone 2");
    }

    public Dictionary<int, string> GetTargetGroups()
    {

        return groupSelection;

    }

    public void SetCurrentZone(int _zone)
    {
        currentZone = _zone;
    }

    public int GetCurrentZone()
    {
        return currentZone;
    }
}
