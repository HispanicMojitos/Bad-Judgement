using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Knife : Equipment
{
    public Knife(string name) : base(name)
    {

    }

    #region Methods

    public void Cut()
    {
        RaycastHit cutHit;

        //Play anim 

        if(Physics.Raycast(this.transform.position, transform.TransformDirection(Vector3.forward), out cutHit, 2.5F))
        {
            var cutTarget = cutHit.collider.GetComponent<Target>();
            if (cutTarget != null) cutTarget.TakeDamage(cutTarget.vieMax);
            //Knife oneshots
        }
    }

    #endregion
}
