using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour
{
    private Collider[] collider;
    [SerializeField] private Transform bruitExlosion;
    [SerializeField] private GameObject effetExplosion;
    [SerializeField] private GameObject Grenade;
    private Target vieGrenade; // On récupere l'etat de vie de la grenade de cette manière
    [SerializeField] private CapsuleCollider coll;
    [HideInInspector] public bool aExplosé = false; // Permet de récuperer l'etat de l'explosion de la grenade dans d'autre scripts
    [SerializeField] [Range(1f, 7f)] private float delai = 5f;
    [SerializeField] [Range(1f, 10f)] private float rayonExplosion = 8f;
    private float vieDeLaGrenade; // Valeur tempon pour récuperer la valeur de la vie de la grenade

    void Awake()
    {
        vieGrenade = this.GetComponent<Target>(); // On récupere le composant du script Target attaché à la grenade
        vieDeLaGrenade = vieGrenade.vie; // On récupere la vie de la grenade en valeur tempon
        coll = GetComponent<CapsuleCollider>();
    }

    void Update ()
    {
        delai = delai - Time.deltaTime;
        if (delai < 3.7f) coll.isTrigger = false;
        if ( ((delai <= 0f) && (aExplosé == false)) || vieGrenade.vie != vieDeLaGrenade)  // On explose la grenade QU'UNE SEULE FOIS, et ce apres tatata secondes
        {
            aExplosé = true;
            bruitExlosion.transform.parent = null; // Permet de faire en sorte que la grenade joue un son, et ce apres qu'elle soit détruite
            Explode();
        }
	}

    private void Explode()
    {
        GameObject fumeeExplosion = Instantiate(effetExplosion, transform.position, transform.rotation); // On créé l'effet de particule d'explosion
        Destroy(fumeeExplosion, 4f);
        collider = Physics.OverlapSphere(this.transform.position, rayonExplosion); // permet de recuperer tout les objet dans un rayon determiné

        foreach (Collider objetProche in collider)
        {
            if (objetProche.GetComponent<Target>() != null)
            {
               if(Vector3.Distance(objetProche.GetComponent<Target>().transform.position,this.gameObject.transform.position) < 3 )  objetProche.GetComponent<Target>().TakeDamage(50);
               else if (Vector3.Distance(objetProche.GetComponent<Target>().transform.position, this.gameObject.transform.position) < 5) objetProche.GetComponent<Target>().TakeDamage(30);
               else if (Vector3.Distance(objetProche.GetComponent<Target>().transform.position, this.gameObject.transform.position) <= 8) objetProche.GetComponent<Target>().TakeDamage(15);
            }
        } // On enleve de la vie a tout les objets en ayant, dans le rayon de l'explosion
        Destroy(Grenade);// On detruit en fin la grenade
    }

    
}
