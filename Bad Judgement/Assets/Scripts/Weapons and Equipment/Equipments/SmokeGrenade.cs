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
        this.uiSprite = Resources.LoadAll<Sprite>("Grenades/Orange theme spritesheet 1")[17];
        InstanciateGrenades();
    }

    #region Methods

    public override void ThrowGrenade()
    {
        base.ThrowGrenade();
        //HERE WAIT FOR EFFECTS
    }

    #endregion
}
