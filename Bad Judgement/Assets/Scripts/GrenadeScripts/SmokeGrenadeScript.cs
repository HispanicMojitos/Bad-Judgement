using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeGrenadeScript : MonoBehaviour
{
    [SerializeField] private AudioSource audioSmoke;
    [SerializeField] private AudioClip soundSmoke;
    [SerializeField] private GameObject effetFumée;
    [SerializeField] private GameObject smokeGrenade;
    [SerializeField] private SphereCollider espaceFumée;

    private Target vieGrenade;
    private bool emmetFumee = false;
    private bool finFumee = false;
    private float delai = 5f;
    private float tempsRafraichissement = - 5;
    private float vieDeLaGrenade; 
    
    private void Start()
    {
        vieGrenade = this.GetComponent<Target>(); 
        vieDeLaGrenade = vieGrenade.vie; 
        audioSmoke.clip = soundSmoke;
        espaceFumée.enabled = true;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && espaceFumée.enabled == true) espaceFumée.enabled = false;
        else if (espaceFumée.enabled == false && delai < 0f && !Input.GetButton("Fire1")) espaceFumée.enabled = true;

        delai = delai - Time.deltaTime;
        if (((delai <= 0f) && (emmetFumee == false)) || ((vieGrenade.vie != vieDeLaGrenade) && emmetFumee == false))    Emmet();
        if (emmetFumee == true) effetFumée.transform.rotation = Quaternion.Euler(-90, 0, 0);

        if(delai < -40 && finFumee == false)
        {
            finFumee = true;
            Collider[] collider = Physics.OverlapSphere(this.transform.position, 50);
            foreach (Collider objetProche in collider)
            {
                if (objetProche.GetComponent<AIscripts>() != null)
                {
                    objetProche.GetComponent<AIscripts>().canSeePlayer = true;
                }
            }
            espaceFumée.enabled = false;
            Destroy(this.gameObject.GetComponent<Target>());
            Destroy(this.gameObject.GetComponent<SmokeGrenadeScript>());
            Destroy(espaceFumée);
        }


    }

    private void Emmet()
    {
        if (audioSmoke.isPlaying == false) audioSmoke.Play();
        effetFumée.SetActive(true);
        effetFumée.transform.rotation = Quaternion.Euler(-90, 0, 0);
        emmetFumee = true;
        
        Collider[] collider = Physics.OverlapSphere(this.transform.position, 50); // permet de recuperer tout les objet dans un rayon determiné

        foreach (Collider objetProche in collider)
        {
            if (objetProche.GetComponent<AIscripts>() != null)
            {
                objetProche.GetComponent<AIscripts>().chercheCouverture = true;
            }
        }
    }
}
