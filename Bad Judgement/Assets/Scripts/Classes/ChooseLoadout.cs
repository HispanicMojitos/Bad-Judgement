using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ChooseLoadout
{
    WeaponsDataBase weaponsDb;

    public List<PrimaryWeapon> allPrimaryWeapons { get; private set; }
    public List<SecondaryWeapon> allSecondaryWeapons { get; private set; }

    #region Chosen things

    public static readonly string directoryPath = "Loadout";
    public static readonly string primaryWeaponPath = @"Loadout\primaryWp";
    public static readonly string secondaryWeaponPath = @"Loadout\secondaryWp";

    public int? actualPrimaryWeaponSelected { get; private set; }
    public int? actualSecondaryWeaponSelected { get; private set; }

    PrimaryWeapon CZ805 = new PrimaryWeapon(6, 30, 35f, 1f, 600, new Vector3(0.03565392F, -0.40402F, 0.5512002F), "CZ805");

    #endregion

    public ChooseLoadout()
    {
        weaponsDb = new WeaponsDataBase();

        weaponsDb.SaveInside(CZ805);
        allPrimaryWeapons = weaponsDb.LoadPrimary();
        //allSecondaryWeapons = weaponsDb.LoadSecondary();

        actualPrimaryWeaponSelected = null;
        actualSecondaryWeaponSelected = null;
    }

    #region Methods

    
    #endregion
}
