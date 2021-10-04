using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorScript : MonoBehaviour
{

    
    Animator anim;
    string currentState;
    ProjectileWeapon weaponScript;

    void OnEnable()
    {
        weaponScript = gameObject.GetComponent<ProjectileWeapon>();
        anim = gameObject.GetComponent<Animator>();
        anim.Play("EmptyChamber");
        

    }
    // BUG: Gun sometime shoots before animation is ready, when rapidfiring
    // Eject function called in Animator
    public void Eject()
    {
        
    }

    public void Idle()
    {
        anim.Play("Idle");
    }

    public void ChangeState(string newState)
    {
        // If the desired state is the one being played, don't change anything
        if (currentState == newState) return;

        // Play the animator state called for
        anim.Play(newState);
    }
}
