using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    #region Membres

    [SerializeField] private Transform ourPlayer;

    [SerializeField]private Text healthText;
    [SerializeField]private Text ammoText;

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
        var player = ourPlayer.GetComponent<Target>();

        float healthPercentage = (player.vie / player.vieMax) * 100;

        this.healthText.text = healthPercentage.ToString();
    }

    #endregion
}
