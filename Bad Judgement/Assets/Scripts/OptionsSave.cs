using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Threading;

public class OptionsSave
{
    #region Members

    List<string> optionsNames = new List<string>()
    {
        "fullscreen", "resolution", "quality",
        "effects sound", "music sound",
        "difficulty", "invert mouse y axis", "vertical sensitivity", "horizontal sensitivity"
    };
    //Reference list to know if we have a complete .config file
    List<string> defaultOptionsValues = new List<string>()
    {
        "true", "1920x1080", "2",
        "100", "100",
        "2", "false", "5", "5"
    };
    //Default options values so that we can create a new .config file if needed

    private static string directoryPath = "UserData";
    private static string path = "UserData\\settings.config";
    private static string tempFilePath = "UserData\\tempConfig.config";
    private static string backupFilePath = "UserData\\configBackup.config";
    List<string> userOptions = new List<string>();

    #endregion

    #region Properties

    public static bool configFileExists { get { return File.Exists(path); } }

    #endregion

    #region Méthodes

    #region Init

    private void CreateFile(bool deleteBeforeCreate)
    {
        if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

        if (deleteBeforeCreate) File.Delete(path); //If the file wasn't valid, we delete it and then create another

        for(int i = 0; i < optionsNames.Count; i++)
        {
            string toAdd = optionsNames[i] + "=" + defaultOptionsValues[i];
            userOptions.Add(toAdd);
        }

        File.WriteAllLines(path, userOptions.ToArray()); //Creating the file and writing into it
    }

    public void Init()
    {
        if (configFileExists)
        {
            userOptions = File.ReadAllLines(path).ToList();
            VerifyFile();
        }
        else CreateFile(false);
    }

    private void VerifyFile()
    {
        bool fileIsValid = true;

        for (int i = 0; i < userOptions.Count; i++)
        {
            if (!userOptions[i].Contains(optionsNames[i]))
            {
                fileIsValid = false;
                break;
            }
        }

        if (!fileIsValid) CreateFile(true);
    }

    #endregion

    #region Read

    public Resolution GetResolution() //Special one for resolution
    {
        Resolution resolution = new Resolution();

        var resol = (from line in userOptions where line.ToLower().Contains("resolution") select line); //String of option where resolution
        string strResolution = resol.ToArray()[0];

        int indexEqualSign = strResolution.IndexOf('='); //Getting the index of the character '='
        strResolution = strResolution.Substring(indexEqualSign + 1); //Getting a new string with only the value of the setting

        string[] res = strResolution.Split('x'); //Resolution width and height as strings

        resolution.width = int.Parse(res[0]);
        resolution.height = int.Parse(res[1]); //Assigning our values to the resolution we will return

        return resolution; //Returning resolution
    }

    public string GetValueOfSetting(string settingName)
    {
        var settingList = from line in userOptions where line.ToLower().Contains(settingName.ToLower()) select line;
        string optionLine = settingList.ToArray()[0];

        int valueStringStartPos = (optionLine.IndexOf('=') + 1);
        string value = optionLine.Substring(valueStringStartPos);

        return value;
    }

    #endregion

    #region Write & modify 

    public void ModifySetting(string optionName, string value)
    {
        int lineToModify = 0;

        for (int i = 0; i < userOptions.Count; i++) 
        {
            if (userOptions[i].Contains(optionName))
            {
                lineToModify = i;
                break;
            }
        }

        userOptions[lineToModify] = optionName + "=" + value;
    }

    public void Save()
    {
        File.WriteAllLines(tempFilePath, userOptions.ToArray());
        File.Replace(tempFilePath, path, backupFilePath);
    }

    #endregion

    #endregion
}
