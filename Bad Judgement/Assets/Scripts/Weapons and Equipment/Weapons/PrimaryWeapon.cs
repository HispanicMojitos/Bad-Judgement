﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrimaryWeapon : MainWeaponsClass
{
    public PrimaryWeapon(int magQty, int bulletsPerMag, float damage, float impactForce, float fireRate, Vector3 spawnPos, string name) :
        base(magQty, bulletsPerMag, damage, impactForce, fireRate, spawnPos, name) { }
}
