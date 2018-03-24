using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeScript : MonoBehaviour
{
    [SerializeField] private Light smokeLigth;
    [SerializeField] private AudioSource audioSmoke;
    [SerializeField] private AudioClip soundSmoke;
    [SerializeField] private GameObject effetFumée;
    [SerializeField] private GameObject smokeGrenade;
    
    [SerializeField] [Range(1f, 10f)] private float rayonExplosion = 5f;

    private Target vieGrenade;
    private bool aExplosé = false; 
    private float delai = 5f;
    private float vieDeLaGrenade; 
    
    private void Start()
    {
        vieGrenade = this.GetComponent<Target>(); 
        vieDeLaGrenade = vieGrenade.vie; 
        audioSmoke.clip = soundSmoke;
    }

    void Update()
    {
        delai = delai - Time.deltaTime;
        if (((delai <= 0f) && (aExplosé == false)) || ((vieGrenade.vie != vieDeLaGrenade) && aExplosé == false))    Emmet();
        if (aExplosé == true) effetFumée.transform.rotation = Quaternion.Euler(-90, 0, 0);
    }

    private void Emmet()
    {
        if (audioSmoke.isPlaying == false) audioSmoke.Play();

        Instantiate(effetFumée, transform.position, Quaternion.Euler(-90, 0, 0)); 
        effetFumée.transform.localPosition = smokeGrenade.transform.position;
        effetFumée.transform.rotation = Quaternion.Euler(-90, 0, 0);
        aExplosé = true;
        Destroy(smokeLigth, 15);
      }
}
