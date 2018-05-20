using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Linq.Expressions;

public class WeaponsDataBase
{
	Dictionary<string, List<MainWeaponsClass>> weaponsDic = new Dictionary<string, List<MainWeaponsClass>>();
	string db = "WeaponsDB.json";

    public WeaponsDataBase()
    {
		if (!File.Exists(db)) File.Create(db).Dispose();
		weaponsDic.Add("Primary", new List<MainWeaponsClass>());
		weaponsDic.Add("Secondary", new List<MainWeaponsClass>());
    }

	public void Save(MainWeaponsClass w)
	{
		string jsonFile = File.ReadAllText(db);
		string type = w is PrimaryWeapon ? "Primary"
					: w is SecondaryWeapon ? "Secondary"
					: null;
		if (type == null) throw new Exception("Weapon is not primary nor secondary");
		if (String.IsNullOrEmpty(jsonFile))
		{
			weaponsDic[type].Add(w);
			weaponsDic[type].ForEach(x => Debug.Log(string.Format("Saved type is: {0}", x.GetType())));
			var tmpJson = JsonConvert.SerializeObject(weaponsDic, Formatting.Indented);
			File.WriteAllText(db, tmpJson);
		}
		else
		{
			weaponsDic = JsonConvert.DeserializeObject<Dictionary<string, List<MainWeaponsClass>>>(jsonFile);
			//weaponsDic[type].RemoveAll(x => x.Name == w.Name);
			if (!weaponsDic[type].Exists(x => x.Name == w.Name))
			{
				weaponsDic[type].Add(w);
				var tmpJson = JsonConvert.SerializeObject(weaponsDic, Formatting.Indented);
				File.WriteAllText(db, tmpJson);
			}
		}
	}
 
    public List<MainWeaponsClass> LoadPrimary()
    {
        string jsonFile = File.ReadAllText(db);
        if (jsonFile != null)
        {
			weaponsDic = JsonConvert.DeserializeObject<Dictionary<string, List<MainWeaponsClass>>>(jsonFile);
            if (weaponsDic["Primary"] != null) return weaponsDic["Primary"];
            else throw new Exception("There are no primary weapons in this database.");
        }
        else throw new Exception("File is empty, please try saving some weapons into the database first.");
    }
    
    public List<MainWeaponsClass> LoadSecondary()
    {
        string jsonFile = File.ReadAllText(db);
        if (jsonFile != null)
        {
			weaponsDic = JsonConvert.DeserializeObject<Dictionary<string, List<MainWeaponsClass>>>(jsonFile);
			if (weaponsDic["Secondary"] != null) return weaponsDic["Secondary"];
            else throw new Exception("There are no secondary weapons in this database.");
        }
        else throw new Exception("File is empty, please try saving some weapons into the database first.");
    }
}