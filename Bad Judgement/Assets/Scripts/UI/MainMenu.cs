using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LaunchGame() //Has to be public. Thanks.
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //We launch the next scene in the SceneManager order
    }

    public void Quit() //Has to be public. Thanks.
    {
        Application.Quit(); //Quit application //Need so much detail due to the difficulty of understanding that method call
    }
}
