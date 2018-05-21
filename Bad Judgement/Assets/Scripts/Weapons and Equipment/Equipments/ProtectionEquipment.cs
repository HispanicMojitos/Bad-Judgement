﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ProtectionEquipment : Equipment
{
    public float equipmentDuration { get; private set; }
    public float protectionCoefficient { get; set; }
    public float maxDuration { get; private set; }

    public bool isDestroyed
    {
        get { return (equipmentDuration == 0) ; }
    }

    public ProtectionEquipment(string name, float protectionCoefficient, float maxDuration) : base(name)
    {
        maxDuration = Mathf.Clamp(maxDuration, 0F, 100F); //Checking if duration is not out of limits

        this.maxDuration = maxDuration;
        this.equipmentDuration = maxDuration; //At the beginning, our equipment is full durability

        this.protectionCoefficient = protectionCoefficient;

        this.uiSprite = null;
    }

    #region Methods
    
    /// <summary>
    /// ONLY FOR GUNSHOTS
    /// Applies damage to equipment and returns the damage that should be applied TO THE CHARACTER
    /// </summary>
    /// <param name="rawDamage">This is the RAW damage that has been received</param>
    /// <returns></returns>
    public float GunHit(float rawDamage)
    {
        float equipDamage = 0F;
        //If our equipment has durability, we apply damage to equipment. If not, there will be no damage

        if (Mathf.RoundToInt(equipmentDuration) != 0) equipDamage = ((protectionCoefficient/ 100F) * rawDamage);
        else return rawDamage; 
        //Calculating damage applied TO THE EQUIPMENT

        this.equipmentDuration -= equipDamage; //Applying that damage TO THE EQUIPMENT
        if (equipmentDuration < 0f) equipmentDuration = 0f;

        return CharacterGunDamage(rawDamage, equipDamage); //Returning the damage applied ON THE PLAYER
    }

    private float CharacterGunDamage(float rawDamage, float equipmentDamage)
    {
        float realDamage = rawDamage - equipmentDamage;
        return realDamage;
    }

    public void GrenadeHit(float damage)
    {
        float equipDamage = damage / 2.5F;
        equipmentDuration -= equipDamage;
        //Equipment takes damage from grenade but DOES NOT PROTECT from explosion
    }

    #endregion
}
