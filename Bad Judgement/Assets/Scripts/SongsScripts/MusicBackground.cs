using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicBackground : MonoBehaviour
{

    public AudioClip musicClip;
    public AudioSource musicSource;
    [Range(1, 3)] //Permet de regler le volume via un bouton de reglage dans Unity
    public float volume = 0.05f;
    [Range(1, 3)] // Permet de regler l'intensité  via un bouton de reglage dans Unity
    public int pitch = 1;
    
	void Start ()
    {   // Initialise le son avec l'emmeteur respectif 
        musicSource.clip = musicClip;
        musicSource.volume = volume;
        musicSource.pitch = pitch;
	}
	
	void Update ()
    {
        if (!musicSource.isPlaying) // Joue le son et le repete 
        {
            musicSource.Play();
        }
	}
}
