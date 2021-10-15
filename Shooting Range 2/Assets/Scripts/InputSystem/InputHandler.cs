using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{

    #region Data
    [Header("Input Data")]
    public InteractionInputData interactionInputData;

    #endregion
    // Start is called before the first frame update
    void Start()
    {
        interactionInputData.ResetInput();
    }

    // Update is called once per frame
    void Update()
    {
        GetInteractionInputData();
    }

    void GetInteractionInputData()
    {
        interactionInputData.InteractClicked = Input.GetKeyDown(KeyCode.E);
        interactionInputData.InteractReleased = Input.GetKeyUp(KeyCode.E);
    }
}
