using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SmokeGrenade : Grenade
{
    public SmokeGrenade(string name, Transform playerPos) : base(name, playerPos)
    {
        this.grenadePrefab = Resources.Load("Grenades/Smoke", typeof(GameObject)) as GameObject;
        this.uiSprite = Resources.Load<Sprite>("Grenades/smokeUI");
        InstanciateGrenades();
        this.grdRb = realGrenade.GetComponent<Rigidbody>();
        grdRb.useGravity = false;
        grdRb.constraints = RigidbodyConstraints.FreezeAll;
    }

    #region Methods

    public override void ThrowGrenade()
    {
        base.ThrowGrenade();
        //HERE WAIT FOR EFFECTS
    }

    #endregion
}
