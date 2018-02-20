using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsOfShoots : MonoBehaviour {
    public AudioClip shootSound;
    public AudioSource machineGun;
    private int indice = 1;
    private float timeSound;
    
    [Range(0f,1f)]
    public float volume = 0.2f;

    [Range(1, 5)]
    public int intensité = 1;

	// Use this for initialization
	void Awake  () {
        machineGun.clip = shootSound;
        machineGun.volume = volume;
        machineGun.pitch = intensité;
        machineGun.spatialBlend = 1f;
        machineGun.spatialize = true;
        timeSound = shootSound.length;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            indice = 0;
            timeSound = shootSound.length;
        }

		if(Input.GetButton("Fire1") && indice == 0)
        {
            timeSound = timeSound - Time.deltaTime;
            if (!machineGun.isPlaying)
            {
                if (timeSound > 0)
                    machineGun.Play();
                else if(timeSound <= 0)
                    indice = 1;
            }
        }
        else
        {
            machineGun.Stop();
        }

	}
}
