using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

  
    [SerializeField] private float delai = 3f;

    private bool aExplosé = false;
    private float rayonExplosion = 5f;
    private float explosionForce = 50f;

    public ParticleSystem effetExplosion;
    
	
	void Update ()
    {
        delai = delai - Time.deltaTime;
        if ( (delai <= 0f) && (aExplosé == false) )  // On explose la grenade QU'UNE SEULE FOIS, et ce apres 3sec
        {
            Explode();
            aExplosé = true;
        }
	}

    private void Explode()
    {

        Instantiate(effetExplosion, transform.position, transform.rotation); // Permet d'appeler l'effet de particule souhaite => EXPLOSIOOOONNN !!!!
        Collider Player = new Collider();

        Collider[] colliders = Physics.OverlapSphere(transform.position, rayonExplosion); // On initialise le rayon d'action sur laquelle la grenade aura effet
        foreach (Collider objectProche in colliders)
        {
            Rigidbody rb = objectProche.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(explosionForce, transform.position, rayonExplosion); // Permet d'ajouter une force aux objets proche ayant un rigibody
            }
            Player = objectProche;
           
        }

        Target Pla = Player.GetComponent<Target>(); // On récupere le script TARGET du joueur pour pouvoir lui enlever de la vie
        Pla.TakeDamage(50);

        Destroy(gameObject);
    }
}
