using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public interface IDamage
{
    float damage { get; set; }
    float range { get; set; }
    GameObject prefab { get; set; }

    float CalculateDamageKnife(float rawDamage, float protection, float distanceToPlayer);
    float CalculateDamageGrenade(float rawDamage, float protection, float distanceToGrenade);
}

public class Damage : IDamage
{
    public float damage { get; set; }
    public float range { get; set; }
    public GameObject prefab { get; set; }

    public Damage(float damage, float range)
    {
        if (damage > 100F) damage = 100F;
        if (damage < 0F) damage = 0F;
        this.damage = damage;

        this.range = range;
    }

    #region Methods

    public float CalculateDamageKnife(float rawDamage, float protection, float distanceToPlayer)
    {
        if (distanceToPlayer > this.range) return 0F;

        if (protection > 70F) protection = 70F;
        if (protection < 0F) protection = 0F;
        protection /= 100F; //Number between 0 & 1F needed for formula
        //However, here we want to have a protection of 70% maximum so that protection doesn't block every damage

        float realDamage;
        if (protection == 0F) realDamage = rawDamage;
        else realDamage = rawDamage - (protection * rawDamage);

        return realDamage;
    }

    public float CalculateDamageGrenade(float rawDamage, float protection, float distanceToGrenade)
    {
        bool inGrenadeActionRay = (distanceToGrenade < this.range);
        //If our grenade is at less than 7F distance, we'll take damage
        if (!inGrenadeActionRay) return 0F; //If we're not in the action ray of grenade, we don't do damage, so we return

        float realDamage;
        if (protection == 0) realDamage = rawDamage;
        else realDamage = rawDamage - ((protection * rawDamage) + (distanceToGrenade / 10F));

        return realDamage;
    }

    #endregion
}