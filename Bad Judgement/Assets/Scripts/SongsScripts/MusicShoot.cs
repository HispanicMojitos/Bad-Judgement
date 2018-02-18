using UnityEngine;
using System.Collections;

public class MusicShoot : MonoBehaviour
{
    public AudioClip shootMachineGun;
    public AudioSource machineGun;

    [Range(0f, 1)] // Permet de regler le volume via un bouton de reglage dans Unity
    public float volume = 0.2f;
    [Range(1, 3)] // Permet de regler l'intensité du son via un bouton de reglage dans Unity
    public int intensité = 1;
    
	void Start ()
    {
        machineGun.clip = shootMachineGun; // Initialise le son avec l'emmeteur
        machineGun.volume = volume;
        machineGun.pitch = intensité;
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
