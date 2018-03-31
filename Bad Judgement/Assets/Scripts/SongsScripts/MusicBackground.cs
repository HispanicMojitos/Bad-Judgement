using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class MusicBackground : MonoBehaviour
{

    public AudioClip musicClip;
    public AudioSource musicSource;

	void Update ()
    {
        Sounds.JouerMusique(musicSource,musicClip);
	}
}
