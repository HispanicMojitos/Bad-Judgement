using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlosionScrit : MonoBehaviour { // CE SCRIPT PERMET DE JOUER LE SON DE L'EXPLOSION EN L'UTILISANT SU UN GAMOBJECT VIDE QUE L'ON DETACHE DE LA GRENADE AU MOMENT OU ELLE EXPLISE
    
    [SerializeField] private AudioSource grenadeAudioSource;
    [SerializeField] private AudioClip grenadeExlosion; 
    [SerializeField] private GameObject grenade; 
    GrenadeScript gre;

    void Awake ()
    {
        grenadeAudioSource.clip = grenadeExlosion; // On initialise le son de la grenade
        gre = grenade.GetComponent<GrenadeScript>(); // On récupere l'etat de la grenade pour l'utiliser ensuite
    }
	
	void FixedUpdate ()
    {
        if (grenadeAudioSource.isPlaying == false && gre.aExplosé == true) // Si l'etat de la grenade est qu'elle a explosé, on joue le son
        {
            grenadeAudioSource.Play(); // Permet de jouer le son de l'explosion
            Destroy(gameObject, 3.5f); // On detruit l'objet
        }
	}
}
