using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UIScript : MonoBehaviour
{
    #region Membres

    #region Sound member
    public AudioMixerSnapshot pause; // Permet de modifier de redefinir tout les son du jeux lorsqu'on est en pause
    public AudioMixerSnapshot enJeux; // Permet de modifier de redefinir tout les son du jeux lorsqu'on est en jeux
    public AudioMixerSnapshot repéré; // Permet de modifier de redefinir tout les son du jeux lorsqu'on est en jeux Et que le joueur est repéré
    public AudioMixerSnapshot aPeuDeVie;
    #endregion Sound member 

    public static bool gameIsPaused { get; private set; }

    [SerializeField] private Transform ourPlayer;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Text healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Text exhaustText;
    [SerializeField] private Slider exhaustSlider;

    [SerializeField] private Text ammoText;

    [SerializeField] private Image primaryGunBg;
    [SerializeField] private Image secondaryGunBg;
    [SerializeField] private Image equipmentBg;

    [SerializeField] private Text protectionText;
    [SerializeField] private Slider protectionSlider;
    private PlayerLoadout currentLoadout;

    #endregion

    #region Start & Update

    private void Start()
    {
        gameIsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentLoadout = ourPlayer.GetComponent<PlayerLoadout>();
    }

    void Update()
    {
        if(this.ourPlayer != null) //If the player object hasn't been destroyed
        {
            UpdateAmmoQty();
            UpdatePlayerHealth();
            UpdatePlayerExhaust();

            if (Input.GetKeyDown(KeyCode.Escape)) //Handling Pause
            {
                if (gameIsPaused && !OptionsMenu.optionMenuIsActive) Resume(); 
                else Pause();
            }

            UpdateCurrentEquipment();

            UpdateProtection();
        }
    }

    #endregion

    #region Méthodes de classe

    private void UpdateAmmoQty()
    {
        //Magqty nb chargeurs
        int bulletsInMag = GunScript.CurrentMag;
        int numberMags = GunScript.MagQty;

        string textToDisplay = bulletsInMag.ToString() + " / " + (numberMags-1).ToString();

        this.ammoText.text = textToDisplay;
    }

    private void UpdatePlayerHealth()
    {
        //Getting our player's Target values like health, etc...
        var player = ourPlayer.GetComponent<Target>();
        //Creating a percentage from a simple value :
        float healthPercentage = (player.vie / player.vieMax) * 100;

        if (healthPercentage < 0) healthPercentage = 0;
        //Setting text and slider values depending on the health value :
        this.healthText.text = healthPercentage.ToString();
        this.healthSlider.value = healthPercentage;
    }

    private void UpdatePlayerExhaust()
    {
        var player = ourPlayer.GetComponent<Movement>();

        float tempExhaust = (player.playerExhaust / player.playerMaxExhaust) * 100.0F;
        int exhaustPercentage = Mathf.RoundToInt(tempExhaust); //On arrondit pour avoir un entier

        this.exhaustSlider.value = exhaustPercentage;
        this.exhaustText.text = exhaustPercentage.ToString();
    }

    private void UpdateCurrentEquipment()
    {
        int selectedEquipment = ourPlayer.GetComponent<PlayerLoadout>().selectedItem;
        //Alpha 180 = High || Alpha 75 = Low = 0.29F

        Color lowAlpha = new Color(1f, 1f, 1f, 0.29f);
        Color highAlpha = new Color(1f, 1f, 1f, 0.705f);

        primaryGunBg.color = lowAlpha;
        secondaryGunBg.color = lowAlpha;
        equipmentBg.color = lowAlpha;

        if (selectedEquipment == 0) primaryGunBg.color = highAlpha;
        else if (selectedEquipment == 1) secondaryGunBg.color = highAlpha;
        else equipmentBg.color = highAlpha;
    }

    private void UpdateProtection()
    {
        var totalProtection = currentLoadout.returnTotalProtectionDuration();
        this.protectionText.text = totalProtection.ToString();
        this.protectionSlider.value = totalProtection / 100f;
    }

    #endregion

    #region Pause Menu

    public void Resume() //Public access keyword to access it via button
    {
        Time.timeScale = 1.0F;
        gameIsPaused = false;

        #region Sounds

        if (ourPlayer.GetComponent<PlayerHealth>().estRepere == true && ourPlayer.GetComponent<PlayerHealth>().aPeuDeVie == false) Sounds.transitionSound(repéré, 0.01f);
        else if (ourPlayer.GetComponent<PlayerHealth>().estRepere == false && ourPlayer.GetComponent<PlayerHealth>().aPeuDeVie == false) Sounds.transitionSound(enJeux, 0.01f);
        else if ( ourPlayer.GetComponent<PlayerHealth>().aPeuDeVie == true) Sounds.transitionSound(aPeuDeVie, 0.01f);
        #endregion Sounds

        this.pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnSaveButtonPressed()
    {
        
    }

    public void OnQuitButtonPressed()
    {
        Teleport.toMainMenu();
    }

    private void Pause()
    {
        Time.timeScale = 0.0F;
        gameIsPaused = true;

        #region Sounds
        Sounds.transitionSound(pause, 0.011f);
        #endregion Sounds

        this.pauseMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion
}
