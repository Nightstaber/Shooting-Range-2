using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class OptionsScript : MonoBehaviour
{

    public AudioMixer audioMixer;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;
    public Slider sensSlider;

    float sensMultiplier = 1f;
    float sensTempMulti = 0f;

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

        //var uniquelist = options.Distinct().ToList();
        //options = uniquelist;
        // Add the resolutions to dropdown, select current value, and refresh the values shown
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        GameObject mc = GameObject.FindGameObjectWithTag("MainCamera");
        ml = mc.GetComponent<MouseLook>();

        // Default sens slider
               
      
        sensSlider.value = ml.defaultMouseSensitivity;
       
    }
/*
    private void Update()
    {
        sensTempMulti = sensSlider.value;
        if (sensMultiplier != sensTempMulti)
        {
            sensMultiplier = sensTempMulti;
            SetMouseSensitivity(sensMultiplier);
        }
            
    }
*/
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
    }

    public void SetQuality(int selection)
    {
        QualitySettings.SetQualityLevel(selection);
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetFullScreen (bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetMouseSensitivity(float sens)
    {
        sensMultiplier = sens;
        float defaultSensitivity = ml.defaultMouseSensitivity;
        float setSensitivity = defaultSensitivity * sensMultiplier;
        ml.SetMouseSensitivity(setSensitivity);
    }

}
