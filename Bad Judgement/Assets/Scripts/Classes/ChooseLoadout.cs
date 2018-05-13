using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

    #endregion

    public ChooseLoadout()
    {
        weaponsDb = new WeaponsDataBase();

        allPrimaryWeapons = weaponsDb.LoadPrimary();
        allSecondaryWeapons = weaponsDb.LoadSecondary();

        actualPrimaryWeaponSelected = null;
        actualSecondaryWeaponSelected = null;
    }

    #region Methods

    
    #endregion
}
