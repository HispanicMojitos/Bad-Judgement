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

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
        // we create a script that destroys an object when its health is equal or lower than 0
    }

    public void GainHealth(float HP)
    {
        if (health < healthMax) health += HP;
    }

    void Die()
    {
        if (gameObject != null && gameObject.GetComponent<SmokeGrenadeScript>() == null && gameObject.CompareTag("Player") == false) Destroy(gameObject, 10);
        else if (gameObject.CompareTag("Player") == true) Destroy(gameObject);  // AJouter la méthode pour faire tomber l'IA, fait toi plaisir Thomas hahaha ;D
    }

}
