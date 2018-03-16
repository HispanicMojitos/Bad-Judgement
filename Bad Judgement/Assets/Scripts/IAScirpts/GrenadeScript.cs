using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    
    [SerializeField] private Transform bruitExlosion;
    [SerializeField] private float delai = 3f;
    [SerializeField] private GameObject joueur;
    [SerializeField] private GameObject effetExplosion;
    [SerializeField] private GameObject Grenade;
    [SerializeField] [Range(1f, 10f)] private float rayonExplosion = 5f;
    private Target vieGrenade;
    public bool aExplosé = false;
    private float vieDeLaGrenade;

    private void Awake()
    {
        vieGrenade = this.GetComponent<Target>();
        vieDeLaGrenade = vieGrenade.vie;
    }

    void Update ()
    {
        delai = delai - Time.deltaTime;
        if ( ((delai <= 0f) && (aExplosé == false)) || vieGrenade.vie != vieDeLaGrenade)  // On explose la grenade QU'UNE SEULE FOIS, et ce apres 3sec
        {
            bruitExlosion.transform.parent = null;
            aExplosé = true;
            Explode();
        }
	}

    private void Explode()
    {
        Instantiate(effetExplosion, transform.position, transform.rotation);

        if(Vector3.Distance(this.transform.position,joueur.transform.position) < 5)
        {
            Target Player = joueur.GetComponent<Target>(); // On récupere le script TARGET du joueur pour pouvoir lui enlever de la vie
            Player.TakeDamage(50);
        }

        Destroy(Grenade);
    }
}
