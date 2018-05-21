using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void Start()
    {
        Time.timeScale = 1F;
        MissionSave.CreateSaveFolder();
    }

    public void LaunchGame() //Has to be public. Thanks.
    {
        Teleport.ToVan();
        //We launch the next scene in the SceneManager order
    }

    public void Quit() //Has to be public. Thanks.
    {
        Application.Quit(); //Quit application //Need so much detail due to the difficulty of understanding that method call
    }
}
