using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    // Now we need a target script to recieve damage and other stuff
    [SerializeField]private float health = 50f;
    public float vie{ get { return health; } }
    

    public void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f) Die();
        // we create a script that destroys an object when its health is equal or lower than 0
    }

    void Die()
    {
        if(gameObject != null && gameObject.GetComponent<SmokeGrenadeScript>() == null) Destroy(gameObject,10);
    }

}
