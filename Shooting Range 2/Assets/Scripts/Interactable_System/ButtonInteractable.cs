using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonInteractable : InteractableBase
{

    float delayTime;

    public override void OnInteract()
    {
        base.OnInteract();
        StartCoroutine(timedReset());
        animator.Play("ButtonPress");
    }

    IEnumerator timedReset()
    {
        PopupTarget nextTarget;
        nextTarget = targetManager.SelectRandomTarget(zone);
        if (nextTarget != null)
        {

        nextTarget.SetRaised(true);
        delayTime = Random.Range(0f, 3f);
        yield return new WaitForSeconds(delayTime);

        nextTarget.TargetReady();
        //targetManager.ResetRandomTarget(zone);

        }
        else
        {
        }
        
    }


}
