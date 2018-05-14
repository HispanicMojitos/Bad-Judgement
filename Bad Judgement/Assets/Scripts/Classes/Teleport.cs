using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Teleport
{

    public static void toVan()
    {
        SceneManager.LoadScene("Van", LoadSceneMode.Single);

    }

    public static void toTuto()
    {
        SceneManager.LoadScene("First scene", LoadSceneMode.Single);
    }

    public static void toMainMenu()
    {
        SceneManager.LoadScene("Main Menu", LoadSceneMode.Single);
    }

    

   
	
	
}
