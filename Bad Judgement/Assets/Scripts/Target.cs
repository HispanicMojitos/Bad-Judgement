using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Now we need a target script to recieve damage and other stuff
    public float health = 50f;

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f) Die(); // we create a script that destroys an object when its health is equal or lower than 
    }

    void Die()
    {
        Destroy(gameObject);
    }

}
