using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrenadeExplode : MonoBehaviour
{
    private static float explosionRadius = 18F;

    private void Start()
    {
        StartCoroutine(ExplodeTime());
    }

    IEnumerator ExplodeTime()
    {
        yield return new WaitForSeconds(5);

        var touchedColliders = Physics.OverlapSphere(this.transform.position, explosionRadius);
        //Getting all colliders hit by the grenade

        //HAVE TO ACTIVATE PARTICLES HERE 

        foreach(var collider in touchedColliders)
        {
            var colTarget = collider.GetComponent<Target>(); 
            //If the touched collider has is a Target
            if(colTarget != null)
            {
                float damage = FragGrenade.CalculateDamage(this.transform.position, colTarget.transform.position);
                colTarget.TakeDamage(damage);
                //Sending damage in function of the distance (calculated in FragGrenade class) to the collider
                if (collider.tag == "Player")
                {
                    var playerCol = collider.GetComponent<PlayerLoadout>();
                    foreach (var protection in playerCol.protection) protection.GrenadeHit(damage);
                }
                //If it is the player, his protection will be damaged by a grenade
            }
        }

        Destroy(this.gameObject, 4F); //Destroying grenade 
    }
}
