using UnityEngine;
using System.Collections;

public class CaracterController : MonoBehaviour {

    public AudioClip sonDePas;
    public AudioSource personnage;
	void Start () {
        personnage.clip = sonDePas;
        personnage.volume = 0.3F;
	}
	
	void Update ()
    {
        if (Input.anyKey) // Optimisation : Si aucune commande est détécté, les calculs des if suivants ne se feront pas
        {
            if (Input.GetKey(KeyCode.Z)) // Appuyer sur Z avancer tout droit
                transform.Translate(0F, 0F, 0.1F);
            else if (Input.GetKey(KeyCode.S)) // Appuyer sur S pour reculer
                transform.Translate(0F, 0F, -0.1F);
            else if (Input.GetKey(KeyCode.Q)) // Appuyer sur Q pour aller vers la gauche
                transform.Translate(-0.1F, 0F, 0F);
            else if (Input.GetKey(KeyCode.D)) // Appuyer sur D pour aller vers la droite
                transform.Translate(0.1F, 0F, 0F);
            else if (Input.GetKey(KeyCode.E)) // appuyer sur E pour descendre verticallement
                transform.Translate(0F, -0.1F, 0F);
            else if (Input.GetKey(KeyCode.Space)) // appuyer sur Espace pour monter verticallement
                transform.Translate(0F, 0.1F, 0F);

            if (!personnage.isPlaying) //Permet de jouer les bruits de pas
                personnage.Play();
            else
                personnage.Pause();
        }
    }
}
