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
    }

    void Update()
    {
        delai = delai - Time.deltaTime;
        if (((delai <= 0f) && (emmetFumee == false)) || ((vieGrenade.vie != vieDeLaGrenade) && emmetFumee == false))    Emmet();
        if (emmetFumee == true) effetFumée.transform.rotation = Quaternion.Euler(-90, 0, 0);

        if(delai < -20 && finFumee == false)
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
            Destroy(this.gameObject.GetComponent<Target>());
            Destroy(this.gameObject.GetComponent<SmokeGrenadeScript>());
        }

        if (delai < tempsRafraichissement && finFumee == false)
        {
            tempsRafraichissement--;
            Collider[] collider = Physics.OverlapSphere(this.transform.position, 50);
            foreach (Collider objetProche in collider)
            {
                if (objetProche.GetComponent<AIscripts>() != null)
                {
                    if (Vector3.Distance(objetProche.GetComponent<AIscripts>().Player.gameObject.transform.position, objetProche.GetComponent<AIscripts>().gameObject.transform.position) < (Vector3.Distance(this.gameObject.transform.position, objetProche.GetComponent<AIscripts>().gameObject.transform.position))  )
                    {
                        objetProche.GetComponent<AIscripts>().canSeePlayer = true;
                    }
                    else objetProche.GetComponent<AIscripts>().canSeePlayer = false;
                }
            }
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
                objetProche.GetComponent<AIscripts>().canSeePlayer = false;
                objetProche.GetComponent<AIscripts>().chercheCouverture = true;
            }
        }
        Destroy(smokeLigth, 15);
    }
}
