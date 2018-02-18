using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectionActiveSon : MonoBehaviour
{
    public AudioClip sonAlerte;
    public AudioSource Zone;
    
    private int incrementation = 0;
    [Range(1, 3)] //Permet de regler l'intensité via un bouton de reglage dans Unity
    public float intensité = 1;
    [Range(0f, 1F)] // Permet de regler le volume via un bouton de reglage dans Unity
    public float volume = 1f;

    private void Start()
    {
        Zone.clip = sonAlerte;
        Zone.volume = volume;  // fixe les initialisation 
        Zone.pitch = intensité;
        Zone.spatialize = true; // Permet d'avoir des sons realiste sur la propagation
    }

    private void OnTriggerEnter() // permet de detecter une colision    /!\ Is Trigger doit etre coché /!\
    {
        if (incrementation == 0) //permet de jouer le son qu'une fois
        {
            Zone.clip = sonAlerte;
            Zone.Play();
            incrementation++;
        }
    }
}
