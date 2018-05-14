using System.Collections;
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
        if(!File.Exists(db))File.Create(db).Dispose();
    }

    public void SaveInside(MainWeaponsClass w)
    {
        if (!File.Exists(db)) File.Create(db).Dispose();
        string jsonFile = File.ReadAllText(db); // Read the database
        if (jsonFile == null || jsonFile == "")
        {
            weaponsList.Add(w);
            var jsonDataArray = weaponsList.ToArray();
            Debug.Log(jsonDataArray[0]);
            var tmpJson = JsonConvert.SerializeObject(jsonDataArray, Formatting.Indented); // Serialize into the json file
            File.WriteAllText(db, tmpJson);
        }
        else
        {
            var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile); // Deserialized the objects and put it in an array
            weaponsList = jsonDataArray.ToList(); // Put its stuff inside a list for better querying and adding new weapons
            if (!weaponsList.Exists(x => x.Name == w.Name) && (w.Name != null || w.Name != "" || w.Name != string.Empty))
            {
                weaponsList.Add(w); // If the weapon doesn't already exist
                jsonDataArray = weaponsList.ToArray();
                var tmpJson = JsonConvert.SerializeObject(jsonDataArray, Formatting.Indented); // Serialize into the json file
                File.WriteAllText(db, tmpJson); // Write into the actual file
            }
        }
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
            if (jsonFile != null)
            {
                var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile);
                weaponsList = new List<MainWeaponsClass>(jsonDataArray);
                return weaponsList;
            }
            else throw new Exception("Database is empty.");
        }
        else throw new Exception("Weapons Database does not exist, please try saving something first.");
    }
    /// <summary>
    /// Return specific weapon
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public MainWeaponsClass LoadSpecific(string name)
    {
        string jsonFile = File.ReadAllText(db);
        if (jsonFile != null)
        {
            var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile);
            weaponsList = new List<MainWeaponsClass>(jsonDataArray);
            var weapon = weaponsList.Where(x => x.Name == name).SingleOrDefault();
            if (weapon != null) return weapon;
            else throw new Exception("This weapon doesn't exist.");
        }
        else throw new Exception("File is empty, please try saving some weapons into the database first.");
    }
    
    public List<MainWeaponsClass> LoadPrimary()
    {
        string jsonFile = File.ReadAllText(db);
        if (jsonFile != null)
        {
            var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile);
            var primaryList = jsonDataArray.Select(x => x).Where(x => x is PrimaryWeapon).ToList();
            if (primaryList != null) return primaryList;
            else throw new Exception("There are no primary weapons in this database.");
        }
        else throw new Exception("File is empty, please try saving some weapons into the database first.");
    }
    
    public List<MainWeaponsClass> LoadSecondary()
    {
        string jsonFile = File.ReadAllText(db);
        if (jsonFile != null)
        {
            var jsonDataArray = JsonConvert.DeserializeObject<MainWeaponsClass[]>(jsonFile);
			var secondaryList = jsonDataArray.Select(x => x).Where(x => x is SecondaryWeapon).ToList();
			if (secondaryList != null) return secondaryList;
            else throw new Exception("There are no secondary weapons in this database.");
        }
        else throw new Exception("File is empty, please try saving some weapons into the database first.");
    }
}