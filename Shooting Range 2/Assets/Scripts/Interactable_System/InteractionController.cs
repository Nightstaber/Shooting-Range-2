using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionController : MonoBehaviour
{
    #region Variables
        [Header("Data")]

        public InteractionInputData interactionInputData;

        public InteractionData interactionData;

        [Space]
        [Header("RaySettings")]
   
        public float rayDistance;
   
        public float raySphereRadius;

        public LayerMask interactableLayer;


    #endregion

    #region Private

    private Camera cam;

    private bool interacting;

    private float holdTimer = 0f;

    [SerializeField] private TextMeshProUGUI useText;

    bool textActive;

    #endregion

    #region Built In Methods
    private void Awake()
        {
            cam = Camera.main;
        }

        private void Update()
        {
            CheckForInteractable();
            CheckForInteractableInput();
        }
    #endregion

    #region Custom Methods

    void CheckForInteractable()
    {
        Ray _ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit _hitInfo;

        bool _hitSomething = Physics.SphereCast(_ray, raySphereRadius, out _hitInfo, rayDistance, interactableLayer);

        if (_hitSomething)
        {
            InteractableBase _interactable = _hitInfo.transform.GetComponent<InteractableBase>();

            if (_interactable != null)
                {
                if (interactionData.IsEmpty())
                {
                    interactionData.Interactable = _interactable;
                }
                else
                {
                    if (!interactionData.IsSameInteractable(_interactable))
                    {
                        interactionData.Interactable = _interactable;
                    }
                }
                
            }

            
            if (!textActive)
            {
                textActive = true;
                useText.gameObject.SetActive(true);
            }
            
            //useText.gameObject.SetActive(true);
        }
        else
        {
            interactionData.ResetData();

            if (textActive)
            {
                textActive = false;
                useText.gameObject.SetActive(false);
            }

            //useText.gameObject.SetActive(false);
        }

        Debug.DrawRay(_ray.origin, _ray.direction * rayDistance, _hitSomething ? Color.green : Color.red);

    }

    void CheckForInteractableInput()
    {
        if (interactionData.IsEmpty())
            return;
        if (interactionInputData.InteractClicked)
        {
            interacting = true;
            holdTimer = 0f;

        }

        if (interactionInputData.InteractReleased)
        {
            interacting = false;
            holdTimer = 0f;
        }

        if (interacting)
        {
            if (!interactionData.Interactable.IsInteractable)
                return;

            if (interactionData.Interactable.holdInteract)
            {
                holdTimer += Time.deltaTime;

                if (holdTimer >= interactionData.Interactable.holdDuration)
                {
                    interactionData.Interact();
                    interacting = false;
                }
            }

            else

            {
                interactionData.Interact();
                interacting = false;
            }
        }
    }

    #endregion
}
