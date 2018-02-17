using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicBackground : MonoBehaviour {

    public AudioClip musicClip;
    public AudioSource musicSource;
    
	void Start () {
        musicSource = GetComponent<AudioSource>(); // Initialise le son avec l'emmeteur respectif 
        musicSource.volume = 0.05f;
	}
	
	void Update () {
        if (!musicSource.isPlaying) // Joue le son et le repete 
        {
            musicSource.clip = musicClip;

            musicSource.Play();
        }
	}
}
