using UnityEngine;
using System.Collections;

public class MusicShoot : MonoBehaviour {
    public AudioClip shootMachineGun;
    public AudioSource machineGun;
    
	void Start () {
        machineGun.clip = shootMachineGun; // Initialise le son avec l'emmeteur
        machineGun.volume = 0.3f;
	}
	
	void Update ()
    {
        if(Input.GetKey(KeyCode.Mouse0)) // Joue le son si un clique gauche de la souris est repéré
        {
            if(!machineGun.isPlaying)
            {
                machineGun.Play();
            }
        }
        else
        {
            machineGun.Stop();
        }
	}
}
