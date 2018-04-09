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

    //List of difficulties

    #endregion

    #region Properties

    public static List<string> difficultiesList
    {
        get { return listeDifficultes; }
    }
    //It allows me to get the difficulties names for the menu

    public static string difficultyLevelName { get { return difficultiesList[difficultyLevelIndex]; } }
    //For the name, we've a "get" that returns the difficulty's name thanks to the current difficulty index
    public static int difficultyLevelIndex { get; private set; }
    //Contains current difficulty
    public static int maxDifficultyIndex { get { return (difficultiesList.Count - 1); } }
    //Returns maximal difficulty index (int) (on sait jamais que t'en aies besoin @Martin)

    #endregion
    //Allows to change the difficulty
    public static void ChangeDifficulty(int niveau)
    {
        difficultyLevelIndex = niveau;
    }   
}
