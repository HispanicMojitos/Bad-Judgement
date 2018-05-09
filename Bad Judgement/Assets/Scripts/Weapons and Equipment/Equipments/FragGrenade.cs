using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FragGrenade : Grenade
{
    public FragGrenade(string name, Sprite uiSprite, int amount, GameObject prefab, Transform playerPos) : base(name, uiSprite, amount, prefab, playerPos)
    {
    }

    #region Surcharges lancers

    public override void ThrowGrenade(Transform startPos)
    {
        base.ThrowGrenade(startPos);
        this.grenade.AddComponent<GrenadeExplode>();
    }

    public static float CalculateDamage(Vector3 grenadePos, Vector3 targetPos)
    {
        float damage = 0F;
        float distGrenadeTarget = Vector3.Distance(grenadePos, targetPos);

        if (distGrenadeTarget <= 4F) damage = 100F;
        else damage = 100F - (distGrenadeTarget * 7F);
        //Formula to calculate damage of a grenade

        return damage;
    }

    #endregion
}