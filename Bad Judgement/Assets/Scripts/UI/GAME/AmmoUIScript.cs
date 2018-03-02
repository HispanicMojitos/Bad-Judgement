﻿using System.Collections;
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
        int bullets = this.gun.Bullets;
        int totalAmmo = this.gun.Magazines * bullets; //To be changed

        string textToDisplay = (bullets.ToString()) + " / " + (totalAmmo.ToString());

        this.ammoText.text = textToDisplay;
    }

    #endregion
}
