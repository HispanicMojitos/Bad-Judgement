using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSmoke;
    [SerializeField] private AudioClip soundSmoke;
    [SerializeField] private GameObject effetFumée;
    [SerializeField] private GameObject smokeGrenade;
    private Target vieGrenade; // On récupere l'etat de vie de la grenade de cette manière
    [HideInInspector] public bool aExplosé = false; // Permet de récuperer l'etat de l'explosion de la grenade dans d'autre scripts
    [SerializeField] [Range(1f, 7f)] private float delai = 5f;
    [SerializeField] [Range(1f, 10f)] private float rayonExplosion = 5f;
    private float vieDeLaGrenade; // Valeur tempon pour récuperer la valeur de la vie de la grenade

    private void Awake()
    {
        this.GetComponent<SmokeGrenadeScript>().enabled = true;
    }

    private void Start()
    {
        this.GetComponent<SmokeGrenadeScript>().enabled = true;
        vieGrenade = this.GetComponent<Target>(); // On récupere le composant du script Target attaché à la grenade
        vieDeLaGrenade = vieGrenade.vie; // On récupere la vie de la grenade en valeur tempon
        audioSmoke.clip = soundSmoke;
    }

    void Update()
    {
        delai = delai - Time.deltaTime;
        if (((delai <= 0f) && (aExplosé == false)) || ((vieGrenade.vie != vieDeLaGrenade) && aExplosé == false))  // On explose la grenade QU'UNE SEULE FOIS, et ce apres tatata secondes
        {
            if (audioSmoke.isPlaying == false) audioSmoke.Play();
            Emmet();// Permet de faire en sorte que la grenade joue un son, et ce apres qu'elle soit détruite
            effetFumée.transform.parent = smokeGrenade.transform;
            effetFumée.transform.localPosition = smokeGrenade.transform.position;
            effetFumée.transform.localRotation = Quaternion.Euler(-90, 0, 0);
            aExplosé = true;
        }
        if (aExplosé == true)
        {
            effetFumée.transform.localPosition = smokeGrenade.transform.position;
            effetFumée.transform.localRotation = Quaternion.Euler(-90, 0, 0);
        }
    }

    private void Emmet()
    {
        Instantiate(effetFumée, transform.position, Quaternion.Euler(-90, 0, 0)); // On créé l'effet de particule d'explosion
        effetFumée.transform.parent = smokeGrenade.transform;
        //Collider[] collider = Physics.OverlapSphere(this.transform.position, rayonExplosion); // permet de recuperer tout les objet dans un rayon determiné

        //foreach (Collider objetProche in collider) { if (objetProche.GetComponent<Target>() != null) objetProche.GetComponent<Target>().TakeDamage(1); } // On enleve de la vie a tout les objets en ayant, dans le rayon de l'explosion
    }
}
