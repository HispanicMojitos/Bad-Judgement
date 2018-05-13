using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class ChooseLoadout
{
    WeaponsDataBase weaponsDb;

    public List<MainWeaponsClass> allPrimaryWeapons { get; private set; }
    public List<MainWeaponsClass> allSecondaryWeapons { get; private set; }

    #region Chosen things

    public static readonly string directoryPath = "Loadout";
    public static readonly string primaryWeaponPath = @"Loadout\primaryWp.json";
    public static readonly string secondaryWeaponPath = @"Loadout\secondaryWp.json";

    public int actualPrimaryWeaponSelected { get; private set; }
    public int actualSecondaryWeaponSelected { get; private set; }

    public int actualPrimaryWeaponDisplayed { get; private set; }
    public int actualSecondaryWeaponDisplayed { get; private set; }

    public bool isChoosingPrimary { get; set; }
    public bool isChoosingSecondary { get; set; }

    public int maxCredits { get; private set; }
    public int credits { get; private set; }

    public MainWeaponsClass chosenPrimaryWeapon { get; private set; }
    public MainWeaponsClass chosenSecondaryWeapon { get; private set; }

    #endregion

    PrimaryWeapon CZ805 = new PrimaryWeapon(6, 30, 35f, 1f, 600, new Vector3(0.03565392F, -0.40402F, 0.5512002F), "CZ805");

    public ChooseLoadout()
    {
        weaponsDb = new WeaponsDataBase();

        weaponsDb.SaveInside(CZ805);
        allPrimaryWeapons = weaponsDb.LoadPrimary();
        //allSecondaryWeapons = weaponsDb.LoadSecondary();

        actualPrimaryWeaponSelected = 0;
        actualSecondaryWeaponSelected = 0;

        actualPrimaryWeaponDisplayed = 0;
        actualSecondaryWeaponDisplayed = 0;

        maxCredits = 5;
        credits = 5;

        isChoosingPrimary = true;
        isChoosingSecondary = false;
    }

    #region Methods

    public void Init()
    {
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!File.Exists(primaryWeaponPath)) File.Create(primaryWeaponPath);
        if (!File.Exists(secondaryWeaponPath)) File.Create(secondaryWeaponPath);
    }

    public void NextWeapon()
    {
        if (isChoosingPrimary)
        {
            if (actualPrimaryWeaponDisplayed < (allPrimaryWeapons.Count - 1)) actualPrimaryWeaponDisplayed++;
            else actualPrimaryWeaponDisplayed = 0;
        }
        else
        {
            if (actualSecondaryWeaponDisplayed < (allSecondaryWeapons.Count - 1)) actualSecondaryWeaponDisplayed++;
            else actualSecondaryWeaponDisplayed = 0;
        }
    }

    public void PreviousWeapon()
    {
        if (isChoosingPrimary)
        {
            if (actualPrimaryWeaponDisplayed > 0) actualPrimaryWeaponDisplayed--;
        }
        else
        {
            if (actualSecondaryWeaponDisplayed > 0) actualSecondaryWeaponDisplayed--;
        }
    }

    public void SelectThatWeapon()
    {
        if (isChoosingPrimary)
        {
            chosenPrimaryWeapon = allPrimaryWeapons[actualPrimaryWeaponDisplayed];
            actualPrimaryWeaponSelected = actualPrimaryWeaponDisplayed;
        }
        else
        {
            chosenSecondaryWeapon = allSecondaryWeapons[actualSecondaryWeaponDisplayed];
            actualSecondaryWeaponSelected = actualSecondaryWeaponDisplayed;
        }
    }

    public void Save()
    {
        var tempPrimaryJson = JsonConvert.SerializeObject(chosenPrimaryWeapon, Formatting.Indented);
        var tempSecondaryJson = JsonConvert.SerializeObject(chosenSecondaryWeapon, Formatting.Indented);

        File.WriteAllText(primaryWeaponPath, tempPrimaryJson);
        File.WriteAllText(secondaryWeaponPath, tempSecondaryJson);
    }

    #endregion
}
