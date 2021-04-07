using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;



public class OptionMenu : MonoBehaviour
{
    
    public AudioMixer audioMixer;

    public Dropdown resolutionDropdown;

    Resolution[] resolutions;

    void start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();
        
        List<string> options = new List<string>();

        for ( int i = 0 ; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
        }

        resolutionDropdown.AddOptions(options);

    }



    public void setVolume (float volume)
    {
        
        audioMixer.SetFloat("MainVolume" , volume);
    }



}
