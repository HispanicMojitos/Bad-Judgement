using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PrimaryWeapon : MainWeaponsClass
{
    public PrimaryWeapon(int magQty, int bulletsPerMag, float damage, float impactForce, int fireRate, Vector3 spawnPos, string name) :
        base(magQty, bulletsPerMag, damage, impactForce, fireRate, spawnPos, name) { }
}
