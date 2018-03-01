using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAmmo : MonoBehaviour
{
    [SerializeField] private Text ammoText;
    private GunScript gun; //Getting a gunScript to take some properties out of it.

    void Start()
    {
        gun = new GunScript(); //Instanciating gun object
        ammoText.text = "";
    }

    void Update()
    {
        UpdateAmmo(); //Updating ammo in the GUI
    }


    #region Class Methods

    private void UpdateAmmo()
    {
        int bullets = gun.Bullets;
        int magazines = gun.Magazines; //Getting current ammo

        string text = bullets.ToString() + " / " + magazines.ToString(); //Text to replace in the GUI

        ammoText.text = text;
    }

    #endregion


}
