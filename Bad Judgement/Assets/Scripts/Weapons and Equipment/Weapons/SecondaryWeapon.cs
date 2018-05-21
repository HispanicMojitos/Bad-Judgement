using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SecondaryWeapon : MainWeaponsClass
{
    public SecondaryWeapon(int magQty, int bulletsPerMag, float damage, float impactForce, int fireRate, float spread, Vector3 spawnPos, string name) :
        base(magQty, bulletsPerMag, damage, impactForce, fireRate, spread, spawnPos, name) { }
}
