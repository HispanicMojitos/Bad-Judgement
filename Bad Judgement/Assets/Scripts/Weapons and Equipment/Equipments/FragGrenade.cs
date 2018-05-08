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

    protected override void ThrowGrenade(Transform startPos)
    {
        base.ThrowGrenade(startPos);
    }

    public static float CalculateDamage(Vector3 grenadePos, Vector3 targetPos)
    {
        float damage = 0F;
        float distGrenadeTarget = Vector3.Distance(grenadePos, targetPos);

        //INSERT MAGICAL EASY PEASY MATH FORMULA HERE

        return damage;
    }

    #endregion
}