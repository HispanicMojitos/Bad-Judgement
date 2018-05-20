using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FlashGrenade : Grenade
{
    public FlashGrenade(string name, Transform playerPos) : base(name, playerPos)
    {
        this.grenadePrefab = Resources.Load("Grenades/Flash", typeof(GameObject)) as GameObject;
        this.uiSprite = Resources.Load<Sprite>("Grenades/flashUI");
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
