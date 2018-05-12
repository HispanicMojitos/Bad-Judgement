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
    #endregion sounds

    [SerializeField] private GameObject optionsMenu; //The menu itself
    [SerializeField] private GameObject optionsPanel; //The panel of the options menu
    [SerializeField] private Scrollbar navSlider; //Navigation up-down with the slider
    private static float panelMinPosition =  -135F;
    private static float panelMaxPosition = 145F;

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

    [SerializeField] private Slider verticalSensitivitySlider;
    [SerializeField] private Text verticalSensitivityText;
    [SerializeField] private Slider horizontalSensitivitySlider;
    [SerializeField] private Text horizontalSensitivityText;

    private OptionsSave optionsFile = new OptionsSave();

    #region Properties

    public static bool optionMenuIsActive { get; private set; }

    #endregion

    private void Start()
    {
        InitPanelPos();
        optionsFile.Init(); //Initializes user config

        InitFullScreen();
        InitResolution();
        InitQuality();
        InitVolume();
        InitGameplay();
    }

    private void Update()
    {
        if (optionMenuIsActive) MouseNav(Input.GetAxis("Mouse ScrollWheell"));
        if (Input.GetKeyDown(KeyCode.Escape) && optionMenuIsActive) DeactivateMenu();
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

        int dropDownValue = 0;
        Resolution currentRes = optionsFile.GetResolution();
        Screen.SetResolution(currentRes.width, currentRes.height, Screen.fullScreen);

        possibleResolutions = Screen.resolutions; //Getting the possible resolutions in an array
        resolutionDropdown.ClearOptions(); //Clearing dropdown displayed options to be sure

        List<string> resolutionOptions = new List<string>(); //Creating a list of the displayed options. Needed.
        for (int i = 0; i < possibleResolutions.Length; i++)
        {
            string option = possibleResolutions[i].width + "x" + possibleResolutions[i].height;
            resolutionOptions.Add(option);
            //For each resolution, we put it in the list under the "width x height" form

            if (possibleResolutions[i].width == currentRes.width && possibleResolutions[i].height == currentRes.height) dropDownValue = i;
        }

        //Finally, we add the options => Takes a list of string as an argument
        resolutionDropdown.AddOptions(resolutionOptions);
        //List isn't declared in the class, it is not in the memory anymore after ending this method.
        //But we need the array so we declared it as a class member.
        resolutionDropdown.value = dropDownValue;
        resolutionDropdown.RefreshShownValue(); //Refreshes displayed value

        #endregion
    }

    private void InitFullScreen()
    {
        bool fullscreen = bool.Parse(optionsFile.GetValueOfSetting("fullscreen"));
        ChangeFullScreen(fullscreen);
        fullscreenToggle.isOn = fullscreen;
    }

    private void InitQuality()
    {
        int qualityValue = int.Parse(optionsFile.GetValueOfSetting("quality"));
        ChangeQuality(qualityValue);
        qualityDropdown.value = qualityValue; 
    }

    private void InitVolume()
    {
        float effectsVolume = float.Parse(optionsFile.GetValueOfSetting("effects sound"));
        float musicVolume = float.Parse(optionsFile.GetValueOfSetting("music sound"));

        MusicVolume(musicVolume);
        EffectsVolume(effectsVolume);
    }

    private void InitGameplay()
    {
        #region Difficulty

        int difficultyValue = int.Parse(optionsFile.GetValueOfSetting("difficulty"));
        DifficultyChange(difficultyValue);

        difficultyDropdown.ClearOptions(); //We clear the choices of the dropdown to be sure
        difficultyDropdown.AddOptions(Difficulté.difficultiesList); //We get the list of difficulties from the Difficulty script
        difficultyDropdown.value = difficultyValue;
        difficultyDropdown.RefreshShownValue();

        #endregion

        #region Invert Mouse Y Axis

        bool invertYAxis = bool.Parse(optionsFile.GetValueOfSetting("invert mouse y axis"));
        InvertMouseYAxis(invertYAxis);
        invertMouseYAxis.isOn = invertYAxis;

        #endregion

        #region Sensitivity

        float vSens = float.Parse(optionsFile.GetValueOfSetting("vertical sensitivity"));
        float hSens = float.Parse(optionsFile.GetValueOfSetting("horizontal sensitivity"));
        HorizontalSensitivity(hSens);
        VerticalSensitivity(vSens);

        horizontalSensitivitySlider.value = hSens;
        verticalSensitivitySlider.value = vSens;

        #endregion
    }

    #endregion

    #region Options Tweak

    public void ChangeQuality(int qualityIndex) //We get the index of the selected quality in the dropdown
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        optionsFile.ModifySetting("quality", qualityIndex.ToString());
    } 

    public void ChangeResolution(int resolutionIndex) //We get the index of the selected resolution in the dropdown
    {
        int width = possibleResolutions[resolutionIndex].width;
        int height = possibleResolutions[resolutionIndex].height;
        //Getting the resolution height and width from the array we created at the start using the index of the selected resolution in the dropdown 
        Screen.SetResolution(width, height, Screen.fullScreen);
        //Setting the resolution with the width, height. For the fullscreen parameter, we just let it as it was by getting if the screen was fullscreen or not.

        string value = width + "x" + height;
        optionsFile.ModifySetting("resolution", value); 
    } 

    public void ChangeFullScreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        optionsFile.ModifySetting("fullscreen", isFullscreen.ToString());
    }
    
    public void EffectsVolume(float volume)
    {   
        Sounds.SoundEffectVolumeSet(MasterMixer, volume);
        optionsFile.ModifySetting("effects sound", volume.ToString());

        float volumePercentage = (volume / (-80)) * 100;
        //From a value that goes from -80 to 0, we make it go from 100 to 0
        effectsVolumeText.text = CreatePercentageWithDecibels(volume).ToString();
    }

    public void MusicVolume(float volume)
    {
        Sounds.MusicVolumSet(MasterMixer, volume);
        optionsFile.ModifySetting("music sound", volume.ToString());

        musicVolumeText.text = CreatePercentageWithDecibels(volume).ToString();
    }

    private int CreatePercentageWithDecibels(float decibels)
    {
        int volumePercentage = Mathf.RoundToInt(((decibels / (-80.0F)) * 100.0F));
        //From a value that goes from -80 to 0, we make it go from 100 to 0
        int differenceToHundred = 100 + volumePercentage;

        volumePercentage -= 100;
        volumePercentage *= (-1);

        return volumePercentage;
    }

    public void DifficultyChange(int difficultyValue)
    {
        changeDifficultée = true;
        Difficulté.ChangeDifficulty(difficultyValue);
        //We change the difficulty with the new one
        optionsFile.ModifySetting("difficulty", difficultyValue.ToString());
    }

    public void InvertMouseYAxis(bool state)
    {
        CamControl.isVerticalAxisInverted = state;
        VanCamControl.isYAxisInverted = state;
        optionsFile.ModifySetting("invert mouse y axis", state.ToString());
    }

    public void HorizontalSensitivity(float value)
    {
        //Slider has a value between 1 and 10
        CamControl.horizontalSensitivity = value;
        VanCamControl.horizontalSensitivity = value;
        horizontalSensitivityText.text = value.ToString();
        optionsFile.ModifySetting("horizontal sensitivity", value.ToString());
    }

    public void VerticalSensitivity(float value)
    {
        //Slider has a value between 1 & 10
        CamControl.verticalSensitivity = value;
        VanCamControl.verticalSensitivity = value;
        verticalSensitivityText.text = value.ToString();
        optionsFile.ModifySetting("vertical sensitivity", value.ToString());
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
        optionsFile.Save();
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
