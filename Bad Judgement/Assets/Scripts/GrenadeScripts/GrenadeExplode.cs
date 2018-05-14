using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class GrenadeExplode : MonoBehaviour
{
    private Timer grenadeTimer;

    private static float explosionRadius = 12F;

    private void Start()
    {
        grenadeTimer = new Timer(5000); //Needs 5 seconds before exploding
        grenadeTimer.Elapsed += OnExplodeTime;
        grenadeTimer.Start();
    }

    private void OnExplodeTime(object sender, ElapsedEventArgs e)
    {
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

        Destroy(this, 1.5F); //Destroying grenade 
    }
}
