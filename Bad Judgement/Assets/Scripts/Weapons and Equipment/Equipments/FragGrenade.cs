using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FragGrenade : Grenade
{
    public FragGrenade(string name, int amount, Transform playerPos) : base(name, amount, playerPos)
    {
        this.grenade = Resources.Load("Grenades/Frag", typeof(GameObject)) as GameObject;
        this.uiSprite = Resources.LoadAll<Sprite>("Grenades/Orange theme spritesheet 1")[5];
    }

    #region Surcharges lancers

    public override void ThrowGrenade()
    {
        base.ThrowGrenade();
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