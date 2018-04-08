using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Difficulté : MonoBehaviour
{
    #region Members

    private static List<string> listeDifficultes = new List<string>()
    {
        "Baby", "Easy", "Medium", "Hard", "Infamy"
    };


    #endregion

    #region Properties

    public static List<string> difficultiesList
    {
        get { return listeDifficultes; }
    }

    #endregion

    public static void ChangeDifficulty(int niveau)
    {
        string difficultyLevel = listeDifficultes[niveau];

        switch (difficultyLevel)
        {
            case "Baby": break;
            case "facile": break;
            case "Normal": break;
            case "Difficile": break;
            case "Infamy": break;
        }
    }   
}
