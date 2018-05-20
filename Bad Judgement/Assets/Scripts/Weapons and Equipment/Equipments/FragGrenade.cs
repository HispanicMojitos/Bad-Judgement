using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class FragGrenade : Grenade
{
    public FragGrenade(string name, Transform playerPos) : base(name, playerPos)
    {
        this.grenadePrefab = Resources.Load("Grenades/Frag", typeof(GameObject)) as GameObject;
        this.uiSprite = Resources.Load<Sprite>("Grenades/fragUI");
        InstanciateGrenades();
        this.grdRb = realGrenade.GetComponent<Rigidbody>();
        grdRb.useGravity = false;
        grdRb.constraints = RigidbodyConstraints.FreezePosition;
    }

    #region Surcharges lancers

    public override void ThrowGrenade()
    {
        base.ThrowGrenade();
        this.realGrenade.AddComponent<GrenadeExplode>();
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