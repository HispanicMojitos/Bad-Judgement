using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    
    [SerializeField] private Transform bruitExlosion;
    [SerializeField] private float delai = 5f;
    public GameObject joueur;
    [SerializeField] private GameObject effetExplosion;
    [SerializeField] private GameObject Grenade;
    [SerializeField] [Range(1f, 10f)] private float rayonExplosion = 5f;
    private Target vieGrenade;
    [HideInInspector] public bool aExplosé = false;
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
        
        Collider[] collider = Physics.OverlapSphere(this.transform.position, 5f);
        foreach (Collider objetProche in collider)
        {
            if(objetProche.GetComponent<Target>() != null)
            {
                objetProche.GetComponent<Target>().TakeDamage(50);
            }
        }

        Destroy(Grenade);
    }
}
