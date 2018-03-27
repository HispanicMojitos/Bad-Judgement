using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    #region Membres

    [SerializeField] private Transform ourPlayer;

    [SerializeField] private Text healthText;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private Text ammoText;

    #endregion

    #region Start & Update

    void Update()
    {
        UpdateAmmoQty();
        UpdatePlayerHealth();
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
        //Setting text and slider values depending on the health value :
        this.healthText.text = healthPercentage.ToString();
        this.healthSlider.value = healthPercentage;
    }

    #endregion
}
