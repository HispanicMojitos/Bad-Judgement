using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour {

    [SerializeField] private GameObject Grenada;

    private MovingTestForward Grenade;

   
	
	void Start ()
    {
        Grenade = Grenada.GetComponent<MovingTestForward>();
	}
	
	
	void Update ()
    {
        teleportToVan();
	}
    void teleportToVan()
    {
        if (Grenade.leanLeft.activeInHierarchy == false && Grenade.collision == 20)
        {
            SceneManager.LoadScene("Van",LoadSceneMode.Single);
        }
    }

    void teleportToMissionOne()
    {
        SceneManager.LoadScene("Mission 1", LoadSceneMode.Single);
    }
}
