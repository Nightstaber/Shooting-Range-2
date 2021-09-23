using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        // Select a weapon from the start
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        // An integer telling which weapon is equipped.
        int previousSelectedWeapon = selectedWeapon;

        // IF scrolling on the mousewheel, change weapons
        if (Input.GetAxis("Mouse ScrollWheel") >0f && !LocalPauseMenu.GameIsPaused)
        {
            if (selectedWeapon >= transform.childCount - 1)
                selectedWeapon = 0;
            else
                selectedWeapon++;
           
        }
        // IF scrolling on the mousewheel, change weapons
        if (Input.GetAxis("Mouse ScrollWheel") < 0f && !LocalPauseMenu.GameIsPaused)
        {
            if (selectedWeapon <= 0)
                selectedWeapon = transform.childCount -1;
            else
                selectedWeapon--;

        }
        // If the newly selected weapon is not the same as the previous weapon, run SelectWeapon()
        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        }

    }
    // Run through all the weapons, enable the selected one, disabled the unselected ones
    void SelectWeapon()
    {
        int i = 0;
        foreach(Transform weapon in transform)
        {
            if (i == selectedWeapon)
                weapon.gameObject.SetActive(true);
            else
                weapon.gameObject.SetActive(false);
            i++;

        }
    }
}
