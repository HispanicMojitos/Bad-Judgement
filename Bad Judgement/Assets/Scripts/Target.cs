using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Now we need a target script to recieve damage and other stuff
    [SerializeField]private float health = 50f;
    private float healthMax;
    public float vie { get { return health; } }
    public float vieMax { get { return healthMax; } }


    private void Awake()
    {
        healthMax = health;
    }
    

    /// <summary> Permet d'enlever de la vie aux gameObject attaché aux script Target ( "amount" etant le nombre de point de vie enlevé lors de ce frame)</summary>
    public void TakeDamage(float amount)
    {
        if (health <= 0f) Die();
        else health -= amount;
    }

    /// <summary> Permet de faire regagner de la vie au gameObjec ("HP" etant le nombre de vie que regagnera cet objet lors de ce frame, ne peut pas depasser la vieMax initialisée)</summary>
    public void GainHealth(float HP)
    {
        if (health < healthMax) health += HP;
    }

    /// <summary> Permet de faire Mourir/Detruire les Objetc attaché à ce script</summary>
    void Die()
    {
        if (gameObject != null && gameObject.GetComponent<SmokeGrenadeScript>() == null && gameObject.CompareTag("Player") == false && gameObject.CompareTag("Ally") == false) Destroy(gameObject, 10);
        else if (gameObject.CompareTag("Player") == true) gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None; // AJouter la méthode pour faire tomber l'IA, fait toi plaisir Thomas hahaha ;D
        else if (gameObject.CompareTag("Ally") == true) gameObject.GetComponent<AIally>().estHS = true;/*Ajouter ici une methode pour la mort de l'IA alliée*/;
    }

}
