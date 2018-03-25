using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour {

    [SerializeField] private GameObject Player;
    [SerializeField] private GameObject grenade;
    private float tempsDelai;

    // Update is called once per frame
    void Update()
    {
        tempsDelai += Time.deltaTime; // Permet d'être "synchro" avec l'animation de lancer de grenade
        if (tempsDelai > 1.2)
        {
            tempsDelai = 0;
            GameObject clone = Instantiate(grenade,this.transform); // On instancie l'objet de la grenade a lancer
            Vector3 distance = this.transform.position - Player.transform.position; // On calcule la distance entre l'IA et le joueur
            distance.y -= 60; // Permet de regler un bon angle de lancer pour la grenade + LA VALEUR EST DIMINUE, PLUS LANGLE EST PETIT !!!
            if(distance.magnitude < 20) clone.GetComponent<Rigidbody>().AddForce(-(distance) * (distance.magnitude/28) , ForceMode.Impulse);
            else if(distance.magnitude < 30) clone.GetComponent<Rigidbody>().AddForce(-(distance) * (distance.magnitude / 27), ForceMode.Impulse);
            else if (distance.magnitude >= 30) clone.GetComponent<Rigidbody>().AddForce(-(distance) * (distance.magnitude / 29), ForceMode.Impulse);
        } // Ici on ajuste la puissance de lancer de l'IA en fonction d'ou se trouve le joueur
    }
}
