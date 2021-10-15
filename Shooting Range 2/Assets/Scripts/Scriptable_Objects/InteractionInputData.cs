using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InteractionInputData", menuName = "InteractionSystem/InputData")]
public class InteractionInputData : ScriptableObject
{
    [Header("Data")]
    private bool interactClicked;
    private bool interactReleased;

    public bool InteractClicked
    {
        get => interactClicked;
        set => interactClicked = value;
    }

    public bool InteractReleased
    {
        get => interactReleased;
        set => interactReleased = value;
    }

    public void ResetInput()
    {
        interactReleased = false;
        interactClicked = false;
    }
}
