using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{

    #region sounds
    [SerializeField] private AudioMixer MasterMixer; // Permet de controller de manière optimisée et efficace TOUT les sons du jeux
    private float VolumeSon = 0f; // Valeur max des sons initialisé
    private float VolumeMusique = 0f; // Valeur max des musiques initialisé
    #endregion sounds

    [SerializeField] private GameObject optionsMenu; //The menu itself
    [SerializeField] private GameObject optionsPanel; //The panel of the options menu
    [SerializeField] private Scrollbar navSlider; //Navigation up-down with the slider
    private static float navScrollSpeed = 1.3F;

    [SerializeField] private Dropdown resolutionDropdown;
    private Resolution[] possibleResolutions; //This is for resolution changing

    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private Dropdown qualityDropdown;

    [SerializeField] private Text effectsVolumeText;
    [SerializeField] private Slider effectsVolumeSlider;

    [SerializeField] private Text musicVolumeText;
    [SerializeField] private Slider musicVolumeSlider;

    #region Properties

    public static bool optionMenuIsActive { get; private set; }

    #endregion

    private void Start()
    {
        InitResolution(); //Initialize resolution options
        InitFullScreen();
        InitQuality();
        InitVolume();
    }

    #region Initializing options menu components

    private void InitResolution()
    {
        #region Resolution Initialization

        possibleResolutions = Screen.resolutions; //Getting the possible resolutions in an array
        resolutionDropdown.ClearOptions(); //Clearing dropdown displayed options to be sure

        List<string> resolutionOptions = new List<string>(); //Creating a list of the displayed options. Needed.
        for (int i = 0; i < possibleResolutions.Length; i++)
        {
            string option = possibleResolutions[i].width + "x" + possibleResolutions[i].height;
            resolutionOptions.Add(option);
            //For each resolution, we put it in the list under the "width x height" for

            if (possibleResolutions[i].width == Screen.currentResolution.width && possibleResolutions[i].height == Screen.currentResolution.height)
            {
                resolutionDropdown.value = i;
                //If our current resolution is the same as the one we are treating, we say that the selected resolution in the dropdown is the same as
                //the one that is effective. Otherwise, the selected value in the options menu wouldn't be the same as the effective at the first time
            }
        }
        //Finally, we add the options => Takes a list of string as an argument
        resolutionDropdown.AddOptions(resolutionOptions);
        //List isn't declared in the class, it is not in the memory anymore after ending this method.
        //But we need the array so we declared it as a class member.

        resolutionDropdown.RefreshShownValue(); //Refreshes displayed value

        #endregion
    }

    private void InitFullScreen()
    {
        fullscreenToggle.isOn = Screen.fullScreen;
    }

    private void InitQuality()
    {
        qualityDropdown.value = 2;
        //Default value at medium
        QualitySettings.SetQualityLevel(2);
    }

    private void InitVolume()
    {
        float volumeEffets = VolumeSon; // Jai changé le Sounds.VolumeSon en VolumeSon
        float volumeMusique = VolumeMusique; // jai changé le Sounds.VolumeMusique en VolumeMusique 

        //Initializing effects slider and text values :
        effectsVolumeSlider.value = volumeEffets; //Setting the slider to the correct value
        effectsVolumeText.text = (volumeEffets + 100).ToString() + "%"; //Displaying a percentage

        //Initializing musique slider and text values :
        musicVolumeSlider.value = volumeMusique; //Slider at the correct value aswell
        musicVolumeText.text = (volumeMusique + 100).ToString() + "%"; //Creating a percentage
    } 

    #endregion

    #region Options Tweak

    public void ChangeQuality(int qualityIndex) //We get the index of the selected quality in the dropdown
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    } 

    public void ChangeResolution(int resolutionIndex) //We get the index of the selected resolution in the dropdown
    {
        int width = possibleResolutions[resolutionIndex].width;
        int height = possibleResolutions[resolutionIndex].height;
        //Getting the resolution height and width from the array we created at the start using the index of the selected resolution in the dropdown 
        Screen.SetResolution(width, height, Screen.fullScreen);
        //Setting the resolution with the width, height. For the fullscreen parameter, we just let it as it was by getting if the screen was fullscreen or not.
    } 

    public void ChangeFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    
    public void EffectsVolume(float volume)
    {
        VolumeSon = volume;
        effectsVolumeText.text = volume.ToString();
        Sounds.SoundEffectVolumeSet(MasterMixer, volume);
    }

    public void MusicVolume(float volume)
    {
        VolumeMusique = volume;
        musicVolumeText.text = volume.ToString();
        Sounds.MusicVolumSet(MasterMixer, volume);
    }

    #endregion

    #region Options menu display

    public void ActivateMenu()
    {
        optionsMenu.SetActive(true); //Activating options menu
        optionMenuIsActive = true;
    }

    public void DeactivateMenu()
    {
        optionsMenu.SetActive(false); //Deactivating options menu
        optionMenuIsActive = false;
    }

    public void OnApplySettingButtonPressed()
    {
        //SAVE HERE
        //WAIT
        this.DeactivateMenu();
    }

    #endregion

    #region Mouse ScrollNavigation



    #endregion
}
