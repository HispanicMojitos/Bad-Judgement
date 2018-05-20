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

    public static readonly string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static readonly string directoryPath = string.Format(@"{0}\BadJudgement", documentsPath);
    public static readonly string loadoutPath = string.Format(@"{0}\Loadout", directoryPath);
    public static readonly string primaryWeaponPath = string.Format(@"{0}\primaryWp.json", loadoutPath);
    public static readonly string secondaryWeaponPath = string.Format(@"{0}\secondaryWp.json", loadoutPath);
    public static readonly string equipmentPath = string.Format(@"{0}\equipment.bjg", loadoutPath);

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
    public int amountFlashGrenade { get; private set; } 
    public int amountSmokeGrenade { get; private set; }
    public int amountFragGrenade { get; private set; }
    public bool helmetSelected { get; private set; }
    public bool vestSelected { get; private set; }

    #endregion

    PrimaryWeapon M4A1 = new PrimaryWeapon(3, 30, 10.4f, 30f, 700, new Vector3(0, -0.011f, 0.017f), "M4A1");
    PrimaryWeapon MP7 = new PrimaryWeapon(3, 40, 5.3f, 30f, 950, new Vector3(0, 0, 0.04f), "MP7");
    PrimaryWeapon SCARH = new PrimaryWeapon(2, 25, 27.8f, 30f, 600, new Vector3(0.0104f, -0.01f, 0.0387f), "SCAR-H");
    SecondaryWeapon M1911 = new SecondaryWeapon(2, 7, 13.4f, 20f, 30, new Vector3(0, -0.015f, 0.057f), "M1911");

    public ChooseLoadout()
    {
        weaponsDb = new WeaponsDataBase();

        weaponsDb.Save(M4A1);
        weaponsDb.Save(MP7);
        weaponsDb.Save(SCARH);
        weaponsDb.Save(M1911);

        allPrimaryWeapons = weaponsDb.LoadPrimary();
        allSecondaryWeapons = weaponsDb.LoadSecondary();

        actualPrimaryWeaponSelected = 0;
        actualSecondaryWeaponSelected = 0;

        actualPrimaryWeaponDisplayed = 0;
        actualSecondaryWeaponDisplayed = 0;

        maxCredits = 5;
        credits = 5;

        isChoosingPrimary = true;
        isChoosingSecondary = false;

        amountFlashGrenade = 0;
        amountFragGrenade = 0;
        amountSmokeGrenade = 0;
        helmetSelected = false;
        vestSelected = false;
    }

    #region Methods

    public void Init()
    {
        Debug.Log(directoryPath);
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);
        if (!Directory.Exists(loadoutPath)) Directory.CreateDirectory(loadoutPath);
        if (File.Exists(primaryWeaponPath)) File.Delete(primaryWeaponPath);
        if (File.Exists(secondaryWeaponPath)) File.Delete(secondaryWeaponPath);
        if (File.Exists(equipmentPath)) File.Delete(equipmentPath);

        File.Create(equipmentPath);
        File.Create(primaryWeaponPath);
        File.Create(secondaryWeaponPath);
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
            if (credits > 0)
            {
                if (chosenPrimaryWeapon == null) credits--;
                chosenPrimaryWeapon = allPrimaryWeapons[0];
                actualPrimaryWeaponSelected = 0;
            }
        }
        else
        {
            if (credits > 0)
            {
                if (chosenSecondaryWeapon == null) credits--;
                chosenSecondaryWeapon = allSecondaryWeapons[actualSecondaryWeaponDisplayed];
                actualSecondaryWeaponSelected = actualSecondaryWeaponDisplayed;
            }
        };
    }

    public void AddGrenade(string grenadeType)
    {
        if (credits > 0)
        {
            switch (grenadeType)
            {
                case "smoke":
                    amountSmokeGrenade++;
                    break;
                case "frag":
                    amountFragGrenade++;
                    break;
                case "flash":
                    amountFlashGrenade++;
                    break;
                default:
                    throw new Exception("(A prendre en considération que si t'es développeur) Ntm c'est pas la bonne string que t'as mis pour la grenade enculé");
                    break;
            }

            credits--;
        }
    }

    public void RemoveGrenade(string grenadeType)
    {
        bool hasToAddCredits = false;

        switch (grenadeType)
        {
            case "smoke":
                if (amountSmokeGrenade > 0)
                {
                    amountSmokeGrenade--;
                    hasToAddCredits = true;
                }
                break;
            case "frag":
                if (amountFragGrenade > 0)
                {
                    amountFragGrenade--;
                    hasToAddCredits = true;
                }
                break;
            case "flash":
                if (amountFlashGrenade > 0)
                {
                    amountFlashGrenade--;
                    hasToAddCredits = true;
                }
                break;
            default:
                throw new Exception("Ntm le type de grenade n'existe pas");
                break;
        }

        if (hasToAddCredits) credits++;
    }

    public void SelectOrUnSelectProtection(bool value, string protectionType)
    {
        if (value)
        {
            if (protectionType == "helmet")
            {
                helmetSelected = true;
                credits--;
            }
            else if (protectionType == "vest")
            {
                vestSelected = true;
                credits--;
            }
            else throw new Exception("Ntm tu t'es trompé dans ta string");
        }
        else
        {
            if(credits < maxCredits)
            {
                if (protectionType == "helmet")
                {
                    helmetSelected = false;
                    credits++;
                }
                else if (protectionType == "vest")
                {
                    vestSelected = false;
                    credits++;
                }
                else throw new Exception("Ntm ptn tu t'es trompé de string u nub");
            }
        }
    }

    public void Save()
    {
        var tempPrimaryJson = JsonConvert.SerializeObject(chosenPrimaryWeapon, Formatting.Indented);
        var tempSecondaryJson = JsonConvert.SerializeObject(chosenSecondaryWeapon, Formatting.Indented);
        var equipmentLines = GetEquipmentTextFileContent();

        File.WriteAllText(primaryWeaponPath, tempPrimaryJson);
        File.WriteAllText(secondaryWeaponPath, tempSecondaryJson);
        File.WriteAllLines(equipmentPath, equipmentLines.ToArray());
    }
    
    private List<string> GetEquipmentTextFileContent()
    {
        var lines = new List<string>();

        lines.Add(string.Format("helmet={0}", helmetSelected.ToString()));
        lines.Add(string.Format("vest={0}", vestSelected.ToString()));
        lines.Add(string.Format("smoke={0}", amountSmokeGrenade.ToString()));
        lines.Add(string.Format("flash={0}", amountFlashGrenade.ToString()));
        lines.Add(string.Format("frag={0}", amountFragGrenade.ToString()));

        return lines;
    }

    #endregion
}
