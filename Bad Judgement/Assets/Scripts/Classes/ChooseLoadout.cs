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

    PrimaryWeapon CZ805 = new PrimaryWeapon(6, 30, 35f, 1f, 600, new Vector3(0.03565392F, -0.40402F, 0.5512002F), "CZ805");

    public ChooseLoadout()
    {
        weaponsDb = new WeaponsDataBase();

        weaponsDb.Save(CZ805);
        allPrimaryWeapons = weaponsDb.LoadPrimary();

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
        if (!File.Exists(primaryWeaponPath)) File.Create(primaryWeaponPath);
        if (!File.Exists(secondaryWeaponPath)) File.Create(secondaryWeaponPath);
        if (File.Exists(equipmentPath)) File.Delete(equipmentPath);
        File.Create(equipmentPath);
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
