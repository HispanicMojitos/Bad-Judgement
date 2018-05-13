using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondaryWeapon : MainWeaponsClass
{
    public SecondaryWeapon(int magQty, int bulletsPerMag, float damage, float impactForce, float fireRate, Vector3 spawnPos) :
        base(magQty, bulletsPerMag, damage, impactForce, fireRate, spawnPos) { }
}
