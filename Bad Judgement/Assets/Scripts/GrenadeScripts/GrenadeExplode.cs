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

        foreach(var collider in touchedColliders)
        {
            var colTarget = collider.GetComponent<Target>();

            if(colTarget != null)
            {
                float damage = FragGrenade.CalculateDamage(this.transform.position, colTarget.transform.position);
                colTarget.TakeDamage(damage);
            }
        }

        Destroy(this, 1.5F);
    }
}
