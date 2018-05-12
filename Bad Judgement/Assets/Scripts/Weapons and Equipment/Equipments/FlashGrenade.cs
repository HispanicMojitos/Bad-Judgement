using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class FlashGrenade : Grenade
{
    public FlashGrenade(string name, int amount, Transform playerPos) : base(name, amount, playerPos)
    {
        this.grenade = Resources.Load("Grenades/Flash", typeof(GameObject)) as GameObject;
        this.uiSprite = Resources.LoadAll<Sprite>("Grenades/Orange theme spritesheet 1")[14];
    }

    #region Methods

    public override void ThrowGrenade()
    {
        base.ThrowGrenade();
        //HERE WAIT FOR EFFECTS
    }

    #endregion
}
