using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ProtectionEquipment : Equipment
{
    protected float equipmentDuration { get; private set; }
    protected float protectionCoefficient { get; set; }
    protected float maxDuration { get; private set; }

    public bool isDestroyed
    {
        get { return (equipmentDuration == 0) ; }
    }

    public ProtectionEquipment(string name, Sprite uiSprite, float protectionCoefficient, float maxDuration) : base(name, uiSprite)
    {
        maxDuration = Mathf.Clamp(maxDuration, 0F, 100F); //Checking if duration is not out of limits

        this.maxDuration = maxDuration;
        this.equipmentDuration = maxDuration; //At the beginning, our equipment is full durability

        this.protectionCoefficient = protectionCoefficient;
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

        if (equipmentDuration != 0) equipDamage = ((protectionCoefficient/ 100F) * rawDamage);
        else return rawDamage; 
        //Calculating damage applied TO THE EQUIPMENT

        this.equipmentDuration -= equipDamage; //Applying that damage TO THE EQUIPMENT

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
