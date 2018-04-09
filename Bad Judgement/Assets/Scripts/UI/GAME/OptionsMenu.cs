using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Audio;

public class OptionsMenu : MonoBehaviour
{
    public static bool changeDifficultée = true; // Permet de savoir si la changée a été changée, et pouvoir la changer dans l'IA script

    #region sounds
    [SerializeField] private AudioMixer MasterMixer; // Permet de controller de manière optimisée et efficace TOUT les sons du jeux
    private float VolumeSon = 0f; // Valeur max des sons initialisé
    private float VolumeMusique = 0f; // Valeur max des musiques initialisé
    #endregion sounds

    [SerializeField] private GameObject optionsMenu; //The menu itself
    [SerializeField] private GameObject optionsPanel; //The panel of the options menu
    [SerializeField] private Scrollbar navSlider; //Navigation up-down with the slider
    private static float panelMinPosition = -134F;
    private static float panelMaxPosition = 80F;

    [SerializeField] private Dropdown resolutionDropdown;
    private Resolution[] possibleResolutions; //This is for resolution changing

    [SerializeField] private Toggle fullscreenToggle;

    [SerializeField] private Dropdown qualityDropdown;

    [SerializeField] private Text effectsVolumeText;
    [SerializeField] private Slider effectsVolumeSlider;

    [SerializeField] private Text musicVolumeText;
    [SerializeField] private Slider musicVolumeSlider;

    [SerializeField] private Dropdown difficultyDropdown;
    [SerializeField] private Toggle invertMouseYAxis;

    #region Properties

    public static bool optionMenuIsActive { get; private set; }

    #endregion

    private void Start()
    {
        InitPanelPos();
        InitResolution();
        InitFullScreen();
        InitQuality();
        InitVolume();
        InitGameplay();
    }

    private void Update()
    {
        if (optionMenuIsActive) MouseNav(Input.GetAxis("Mouse ScrollWheell"));
    }

    #region Initializing options menu components

    private void InitPanelPos()
    {
        Vector3 panelPos = optionsPanel.transform.localPosition;
        panelPos.y = panelMinPosition;

        optionsPanel.transform.localPosition = panelPos;
    }

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

    private void InitGameplay()
    {
        #region Difficulty

        difficultyDropdown.ClearOptions(); //We clear the choices of the dropdown to be sure
        difficultyDropdown.AddOptions(Difficulté.difficultiesList); //We get the list of difficulties from the Difficulty script
        
        difficultyDropdown.value = 2;
        difficultyDropdown.RefreshShownValue();

        #endregion

        #region Invert Mouse Y Axis

        CamControl.isVerticalAxisInverted = false;
        invertMouseYAxis.isOn = false;

        #endregion
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
        VolumeSon = volume; // Jai changé le Sounds.VolumeSon en VolumeSon
        effectsVolumeText.text = volume.ToString();
        Sounds.SoundEffectVolumeSet(MasterMixer, volume);
    }

    public void MusicVolume(float volume)
    {
        VolumeMusique = volume; // jai changé le Sounds.VolumeMusique en VolumeMusique 
        musicVolumeText.text = volume.ToString();
        Sounds.MusicVolumSet(MasterMixer, volume);
    }

    public void DifficultyChange(int difficultyValue)
    {
        changeDifficultée = true;
        Difficulté.ChangeDifficulty(difficultyValue);
        //We change the difficulty with the new one
    }

    public void InvertMouseYAxis(bool state)
    {
        CamControl.isVerticalAxisInverted = state;
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

    public void OnScrollValueChanged(float value)
    {
        float panelDeltaHeight = panelMaxPosition - panelMinPosition;
        //Getting the height between the top and bottom of the panel
        float targetYPos = (value * panelDeltaHeight) + panelMinPosition; //Target position calcul
        //Value is the value of the scrollbar (0 => 1)
        //We multiply it by the height to so that the scrollbar is "calibrated". By doing that we have a "common" scale for scrollbar and panel
        //After that, we add so that we can go in the negative values (the "+" is used and not a "-" because there's a negative number (- & - = +)
        //Pour une explication détaillée venez me voir

        optionsPanel.transform.localPosition = new Vector3(0F, targetYPos, 0F);
        //Increases or decreases Y position of the panel.

        //This system works thanks to a mask that fits the options menu panel dimensions.
    }

    private void MouseNav(float mouseAxis)
    {
        mouseAxis *= -1F;
        //When we scroll down we want to increase value and when we scroll up we want to decrease => so we invert the mouse scroll axis value

        navSlider.value += mouseAxis; //Automatically clamped between 0 & 1
    }

    #endregion
}
