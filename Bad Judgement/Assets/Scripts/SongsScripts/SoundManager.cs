using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour {

    public AudioMixerSnapshot pause; 
    public AudioMixerSnapshot enJeux;



    public void soundPaused()
    {
        pause.TransitionTo(0.1f);
    }

    public void soundEnJeux()
    {
        enJeux.TransitionTo(0.1f);
    }



    void Start ()
    {

	}

	void Update ()
    {
		
	}
}
