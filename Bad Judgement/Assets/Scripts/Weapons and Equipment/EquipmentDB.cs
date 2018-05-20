using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using Newtonsoft.Json;
using System;

[Serializable]
public static class EquipmentDB
{
    #region File Paths

    public static readonly string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
    public static readonly string directoryPath = string.Format(@"{0}\BadJudgement", documentsPath);
    public static readonly string loadoutPath = string.Format(@"{0}\Loadout", directoryPath);
    public static readonly string primaryWeaponPath = string.Format(@"{0}\primaryWp.json", loadoutPath);
    public static readonly string secondaryWeaponPath = string.Format(@"{0}\secondaryWp.json", loadoutPath);
    public static readonly string equipmentPath = string.Format(@"{0}\equipment.bjg", loadoutPath);

    public static List<string> equipmentList;

    #endregion

    static EquipmentDB()
    {
        GetEquipment(); //EquipmentList Init
    }

    #region Private Methods

    private static void GetEquipment()
    {
        equipmentList = File.ReadAllLines(equipmentPath).ToList();
    }

    private static string GetValueOfItem(string line)
    {
        int indexOfEqualSign = line.IndexOf('=');
        var value = line.Substring(indexOfEqualSign + 1);

        return value;
    }

    #endregion

    #region Public methods

    public static int GetGrdNumber(string grdType)
    {
        string line = equipmentList.SingleOrDefault(x => x.ToLower().Contains(grdType.ToLower()));
        int nb = 0;

        int.TryParse(GetValueOfItem(line), out nb);

        return nb;
    }
    
    public static bool HasVest()
    {
        string line = equipmentList.SingleOrDefault(x => x.ToLower().Contains("vest"));
        bool hasVest = false;

        bool.TryParse(GetValueOfItem(line), out hasVest);

        return hasVest;
    }

    public static bool HasHelmet()
    {
        string line = equipmentList.SingleOrDefault(x => x.ToLower().Contains("helmet"));
        bool hasHelmet = false;

        bool.TryParse(GetValueOfItem(line), out hasHelmet);

        return hasHelmet;
    }

    public static MainWeaponsClass GetPrimaryWp()
    {
        string wpJson = File.ReadAllText(primaryWeaponPath);
        var primaryWp = JsonConvert.DeserializeObject<MainWeaponsClass>(wpJson);

        return primaryWp;
    }

    public static MainWeaponsClass GetSecondaryWp()
    {
        string wpJson = File.ReadAllText(secondaryWeaponPath);
        var secondaryWp = JsonConvert.DeserializeObject<MainWeaponsClass>(wpJson);

        return secondaryWp;
    }

    #endregion
}
