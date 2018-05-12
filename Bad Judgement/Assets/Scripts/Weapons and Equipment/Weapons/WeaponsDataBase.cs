﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;

public class WeaponsDataBase
{
    List<MainWeaponsClass> weaponsList = new List<MainWeaponsClass>();
    string db = "WeaponsDB.json";

    public WeaponsDataBase()
    {
        File.Create(db);
    }

    public void SaveInside(MainWeaponsClass w)
    {
        if (!File.Exists(db)) File.Create(db); 
        string jsonFile = File.ReadAllText(db); // Read the database
        var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile); // Deserialized the objects and put it 
        // in an array

        weaponsList = new List<MainWeaponsClass>(jsonDataArray); // Put its stuff inside a list for better querying 
        // and adding new weapons
        if (!weaponsList.Exists(x => x.name == w.name)) weaponsList.Add(w); // If the weapon doesn't already exist
        jsonDataArray = weaponsList.ToArray();
        var tmpJson = JsonConvert.SerializeObject(jsonDataArray, Formatting.Indented); // Serialize into the json file
        File.WriteAllText(db, tmpJson); // Write into the actual file
    }

    /// <summary>
    /// Returns every weapon inside of the database
    /// </summary>
    /// <returns></returns>
    public List<MainWeaponsClass> Load()
    {
        if (File.Exists(db))
        {
            string jsonFile = File.ReadAllText(db);
            var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile);
            weaponsList = new List<MainWeaponsClass>(jsonDataArray);
            return weaponsList;
        }
        else throw new Exception("Weapons Database does not exist, please try saving something first");
    }
    /// <summary>
    /// Return specific weapon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public MainWeaponsClass LoadSpecific(string name)
    {
        string jsonFile = File.ReadAllText(db);
        var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile);
        weaponsList = new List<MainWeaponsClass>(jsonDataArray);
        var weapon = weaponsList.Where(x => x.name == name).SingleOrDefault();
        return weapon;
    }

    public List<PrimaryWeapon> LoadPrimary()
    {
        string jsonFile = File.ReadAllText(db);
        var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile);
        var primaryList = (jsonDataArray.OfType<PrimaryWeapon>()).ToList();
        return primaryList;
    }

    public List<SecondaryWeapon> LoadSecondary()
    {
        string jsonFile = File.ReadAllText(db);
        var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile);
        var secondaryList = (jsonDataArray.OfType<SecondaryWeapon>()).ToList();
        return secondaryList;
    }

}
