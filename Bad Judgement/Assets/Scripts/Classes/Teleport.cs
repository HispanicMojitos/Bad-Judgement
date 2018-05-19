using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Teleport
{

    public static void ToVan()
    {
        SceneManager.LoadScene("Van", LoadSceneMode.Single);

    }
    public static void ToMissionOne()
    {
        SceneManager.LoadScene("Mission 1", LoadSceneMode.Single);
    }
    public static void ToTuto()
    {
        SceneManager.LoadScene("First scene", LoadSceneMode.Single);
    }

    public static void ToMainMenu()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    

   
	
	
}
