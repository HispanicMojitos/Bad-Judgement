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
    string primaryDb = "PrimaryWeapons.json";
    string secondaryDb = "SecondaryWeapons.json";

    public WeaponsDataBase()
    {
        if (!File.Exists(primaryDb)) File.Create(primaryDb).Dispose();
        if (!File.Exists(secondaryDb)) File.Create(secondaryDb).Dispose();
    }

    public void Save(PrimaryWeapon w)
    {
        if (!File.Exists(primaryDb)) File.Create(primaryDb).Dispose();
        List<PrimaryWeapon> primaries = new List<PrimaryWeapon>();
        string jsonFile = File.ReadAllText(primaryDb); // Read the database
        if (jsonFile == null || jsonFile == "")
        {
            primaries.Add(w);
            var jsonDataArray = primaries.ToArray();
            primaries.ForEach(x => Debug.Log(x.GetType()));
            var tmpJson = JsonConvert.SerializeObject(jsonDataArray, Formatting.Indented); // Serialize into the json file
            File.WriteAllText(primaryDb, tmpJson);
        }
        else
        {
            var jsonDataArray = JsonConvert.DeserializeObject<PrimaryWeapon[]>(jsonFile); // Deserialized the objects and put it in an array
            primaries = jsonDataArray.ToList(); // Put its stuff inside a list for better querying and adding new weapons
            if (weaponsList.Exists(x => x.Name != w.Name))
            {
                primaries.Add(w); // If the weapon doesn't already exist
                jsonDataArray = primaries.ToArray();
                var tmpJson = JsonConvert.SerializeObject(jsonDataArray, Formatting.Indented); // Serialize into the json file
                File.WriteAllText(primaryDb, tmpJson); // Write into the actual file
            }
        }
    }

    public void Save(SecondaryWeapon w)
    {
        if (!File.Exists(secondaryDb)) File.Create(secondaryDb).Dispose();
        List<SecondaryWeapon> secondaries = new List<SecondaryWeapon>();
        string jsonFile = File.ReadAllText(secondaryDb); // Read the database
        if (jsonFile == null || jsonFile == "")
        {
            secondaries.Add(w);
            var jsonDataArray = secondaries.ToArray();
            secondaries.ForEach(x => Debug.Log(x.GetType()));
            var tmpJson = JsonConvert.SerializeObject(jsonDataArray, Formatting.Indented); // Serialize into the json file
            File.WriteAllText(secondaryDb, tmpJson);
        }
        else
        {
            var jsonDataArray = JsonConvert.DeserializeObject<SecondaryWeapon[]>(jsonFile); // Deserialized the objects and put it in an array
            secondaries = jsonDataArray.ToList(); // Put its stuff inside a list for better querying and adding new weapons
            if (weaponsList.Exists(x => x.Name != w.Name))
            {
                secondaries.Add(w); // If the weapon doesn't already exist
                jsonDataArray = secondaries.ToArray();
                var tmpJson = JsonConvert.SerializeObject(jsonDataArray, Formatting.Indented); // Serialize into the json file
                File.WriteAllText(secondaryDb, tmpJson); // Write into the actual file
            }
        }
    }
 
    public List<PrimaryWeapon> LoadPrimary()
    {
        string jsonFile = File.ReadAllText(primaryDb);
        if (jsonFile != null)
        {
            var jsonDataArray = JsonConvert.DeserializeObject<PrimaryWeapon[]>(jsonFile);
            var primaryList = jsonDataArray.ToList();
            primaryList.ForEach(x => Debug.Log(x.GetType()));
            if (primaryList != null) return primaryList;
            else throw new Exception("There are no primary weapons in this database.");
        }
        else throw new Exception("File is empty, please try saving some weapons into the database first.");
    }
    
    public List<SecondaryWeapon> LoadSecondary()
    {
        string jsonFile = File.ReadAllText(secondaryDb);
        if (jsonFile != null)
        {
            var jsonDataArray = JsonConvert.DeserializeObject<SecondaryWeapon[]>(jsonFile);
			var secondaryList = jsonDataArray.ToList();
			if (secondaryList != null) return secondaryList;
            else throw new Exception("There are no secondary weapons in this database.");
        }
        else throw new Exception("File is empty, please try saving some weapons into the database first.");
    }
}