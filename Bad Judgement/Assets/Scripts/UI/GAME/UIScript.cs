using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Linq;

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

    [SerializeField] private Image[] crosshair;
    [SerializeField] private GameObject pauseMenu;

    [SerializeField] private Text healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Text exhaustText;
    [SerializeField] private Slider exhaustSlider;

    [SerializeField] private Text ammoText;

    [SerializeField] private Image primaryGunBg;
    [SerializeField] private Image equipmentBg;
    [SerializeField] private Image primaryGun;
    [SerializeField] private Image equip1;
    [SerializeField] private Image equip2;
    [SerializeField] private Image equip3;
    private Text equip1Number;
    private Text equip2Number;
    private Text equip3Number;
    private Image[] equipmentsImages;

    [SerializeField] private Text protectionText;
    [SerializeField] private Slider protectionSlider;
    [SerializeField] private Slider protectionCoeffSlider;
    [SerializeField] private Text protectionCoeffText;
    private PlayerLoadout currentLoadout;
    private int selectedEquipment;

    [SerializeField] private Text dieText;
    Target playerTarget;

    #endregion

    #region Start & Update

    private void Start()
    {
        playerTarget = ourPlayer.GetComponent<Target>();

        equip1Number = equip1.GetComponentInChildren<Text>();
        equip2Number = equip2.GetComponentInChildren<Text>();
        equip3Number = equip3.GetComponentInChildren<Text>();

        equipmentsImages = new Image[] { equip1, equip2, equip3 };
        gameIsPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentLoadout = ourPlayer.GetComponent<PlayerLoadout>();
        InitEquipmentUI();

        primaryGun.sprite = Resources.Load<Sprite>(string.Format("Weapons/UISprites/{0}", currentLoadout.weapons[0].Name));
    }

    void Update()
    {
        if (this.ourPlayer != null) //If the player object hasn't been destroyed
        {
            selectedEquipment = currentLoadout.selectedItem;

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
            UpdateCrosshair();
            DieText();
        }
    }

    #endregion

    #region Méthodes de classe

    public void DieText()
    {
        if (playerTarget.vie <= 0F)
        {
            Time.timeScale = 0f;
            dieText.gameObject.SetActive(true);
            StartCoroutine(MissionFailed());
        }
    }

    IEnumerator MissionFailed()
    {
        yield return new WaitForSecondsRealtime(3);
        Cursor.visible = true;
        Teleport.ToMainMenu();
    }

    private void InitEquipmentUI()
    {
        if (PlayerLoadout.maxSelectedItem < 1) equipmentBg.gameObject.SetActive(false);
        else
        {
            for (int i = 0; i < currentLoadout.grdTable.Length; i++)
            {
                if (currentLoadout.grdTable[i] == "smoke") equipmentsImages[i].sprite = currentLoadout.grenades.FirstOrDefault(g => g.GetType() == typeof(SmokeGrenade)).uiSprite;
                else if (currentLoadout.grdTable[i] == "flash") equipmentsImages[i].sprite = currentLoadout.grenades.FirstOrDefault(g => g.GetType() == typeof(FlashGrenade)).uiSprite;
                else if (currentLoadout.grdTable[i] == "frag") equipmentsImages[i].sprite = currentLoadout.grenades.FirstOrDefault(g => g.GetType() == typeof(FragGrenade)).uiSprite;
                else equipmentsImages[i].gameObject.SetActive(false); //else it'll be null
            }
        }

        equip1 = equipmentsImages[0];
        equip2 = equipmentsImages[1];
        equip3 = equipmentsImages[2];
    }

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
        this.healthText.text = (Mathf.RoundToInt(healthPercentage)).ToString();
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
        //Alpha 180 = High || Alpha 75 = Low = 0.29F

        Color lowAlpha = new Color(1f, 1f, 1f, 0.29f);
        Color highAlpha = new Color(1f, 1f, 1f, 0.705f);

        primaryGunBg.color = lowAlpha;
        primaryGun.color = lowAlpha;
        equipmentBg.color = lowAlpha;
        equip1.color = lowAlpha;
        equip2.color = lowAlpha;
        equip3.color = lowAlpha;

        if (selectedEquipment == 0)
        {
            primaryGunBg.color = highAlpha;
            primaryGun.color = highAlpha;
        }
        else if(equipmentBg.IsActive())
        {
            equipmentBg.color = highAlpha;

            if (selectedEquipment == 1) equip1.color = highAlpha;
            else if (selectedEquipment == 2) equip2.color = highAlpha;
            else equip3.color = highAlpha;
        }
        
        equip1Number.text = currentLoadout.grenades.Count(g => g.name == currentLoadout.grdTable[0] && g.throwable).ToString();
        equip2Number.text = currentLoadout.grenades.Count(g => g.name == currentLoadout.grdTable[1] && g.throwable).ToString();
        equip3Number.text = currentLoadout.grenades.Count(g => g.name == currentLoadout.grdTable[2] && g.throwable).ToString();
    }

    private void UpdateProtection()
    {
        var totalProtection = currentLoadout.ReturnTotalProtectionDuration();
        this.protectionText.text = totalProtection.ToString();
        this.protectionSlider.value = totalProtection / 100f;

        var totalProtectionCoefficient = currentLoadout.ReturnProtectionCoefficient();
        this.protectionCoeffText.text = totalProtectionCoefficient.ToString();
        this.protectionCoeffSlider.value = totalProtectionCoefficient / 100F;
    }

    private void UpdateCrosshair()
    {
        if (GunScript.IsAiming) foreach (var x in crosshair) x.gameObject.SetActive(false);
        else
        {
            StartCoroutine(WaitForCrosshair());
        }
    }

    IEnumerator WaitForCrosshair()
    {
        yield return new WaitForSeconds(0.25F);
        foreach (var x in crosshair) x.gameObject.SetActive(true);
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
        Teleport.ToMainMenu();
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
