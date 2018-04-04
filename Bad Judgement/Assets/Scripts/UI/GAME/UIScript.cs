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
    #endregion Sound member 

    public static bool gameIsPaused { get; private set; }

    [SerializeField] private Transform ourPlayer;

    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Text healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Text exhaustText;
    [SerializeField] private Slider exhaustSlider;

    [SerializeField] private Text ammoText;

    #endregion

    #region Start & Update

    private void Start()
    {
        gameIsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
                if (gameIsPaused) Resume();
                else Pause();
            }
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

    #endregion

    #region Pause Menu

    public void Resume() //Public access keyword to access it via button
    {
        Time.timeScale = 1.0F;
        gameIsPaused = false;

        #region Sounds
        Sounds.soundPaused(enJeux);
        #endregion Sounds

        this.pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void OnOptionsButtonPressed()
    {
        //Instanciate Menu Here
    }

    public void OnSaveButtonPressed()
    {
        
    }

    public void OnQuitButtonPressed()
    {

    }

    private void Pause()
    {
        Time.timeScale = 0.0F;
        gameIsPaused = true;

        #region Sounds
        Sounds.soundPaused(pause);
        #endregion Sounds

        this.pauseMenu.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion
}
