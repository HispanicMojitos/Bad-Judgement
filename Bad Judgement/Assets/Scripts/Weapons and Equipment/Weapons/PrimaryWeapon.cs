using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryWeapon : MainWeaponsClass
{
    public PrimaryWeapon(int magQty, int bulletsPerMag, float damage, float impactForce, float fireRate, Transform spawnPos) :
        base(magQty, bulletsPerMag, damage, impactForce, fireRate, spawnPos) { }
}
