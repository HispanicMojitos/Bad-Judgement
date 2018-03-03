using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUIScript : MonoBehaviour
{
    #region Membres

    [SerializeField] private Text ammoText;
    private GunScript gun; //Getting gun instance to get current ammo

    #endregion

    #region Start & Update

    void Start()
    {
        gun = new GunScript();
    }

    void Update()
    {
        UpdateAmmoQty();
    }

    #endregion

    #region Méthodes de classe

    private void UpdateAmmoQty()
    {
        //Magqty nb chargeurs
        int bulletsInMag = GunScript.CurrentMag;
        int numberMags = GunScript.MagQty;

        string textToDisplay = bulletsInMag.ToString() + " / " + (numberMags - 1).ToString();

        this.ammoText.text = textToDisplay;
    }

    #endregion
}
