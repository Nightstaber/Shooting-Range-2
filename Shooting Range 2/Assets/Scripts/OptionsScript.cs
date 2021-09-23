using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OptionsScript : MonoBehaviour
{
    // Insert AudioMixer for volume slider
    [SerializeField]
    AudioMixer audioMixer;

    // Create resolution array, to be used in the setup
    Resolution[] resolutions;

    // Dropdowns and slider for options
    [SerializeField]
    TMP_Dropdown resolutionDropdown;
    [SerializeField]
    TMP_Dropdown qualityDropdown;
    [SerializeField]
    Slider sensSlider;
    

    MouseLook ml;

    void Start()
    {


        // Default quality as high on startup
        int qualLevel = QualitySettings.GetQualityLevel();
        qualityDropdown.value = qualLevel;
        qualityDropdown.RefreshShownValue();

        // Removing any non-60 hz resolutions
        //var resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate == 60);

        // Finding possible resolutions
        resolutions = Screen.resolutions;

        // Clears the resolution dropdown
        resolutionDropdown.ClearOptions();
        
        // Create a list of options
        List<string> options = new List<string>();

        // New int to be used for setting actual resolution in dropdown
        int currentResolutionIndex = 0;

        // Go through the list of resolutions and add them to a string list
        for (int i = 0; i < resolutions.Length; i++)
        {
            string dropDownLabel = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(dropDownLabel);

            // Finding the current resolution to be selected in the dropdown
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        // Add the resolutions to dropdown, select current value, and refresh the values shown
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Make a reference to the MouseLook Script
        GameObject mc = GameObject.FindGameObjectWithTag("MainCamera");
        ml = mc.GetComponent<MouseLook>();

        // Default sens slider
        sensSlider.value = ml.GetMouseSensitivity();
       
    }

    // Called from the Volume slider in Options
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    // Called from the Quality dropdown in the Options menu
    public void SetQuality(int selection)
    {
        QualitySettings.SetQualityLevel(selection);
    }

    // Called from the resolutions dropdown in options
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }
     
    //Called from checkmark in Options
    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

}
