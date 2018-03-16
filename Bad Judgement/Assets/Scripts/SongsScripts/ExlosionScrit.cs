using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExlosionScrit : MonoBehaviour {
    
    [SerializeField] private AudioSource grenadeAudioSource;
    [SerializeField] private AudioClip grenadeExlosion;
    [SerializeField] private GameObject grenade;
    private bool exlose = false;
    GrenadeScript gre;
    // Use this for initialization
    void Start ()
    {
        grenadeAudioSource.clip = grenadeExlosion;
        gre = grenade.GetComponent<GrenadeScript>();
    }
	
	// Update is called once per frame
	void Update ()
    {

        if (grenadeAudioSource.isPlaying == false && gre.aExplosé == true)
        {
            grenadeAudioSource.Play();
            Destroy(gameObject, 4.520f);
        }
	}
}
