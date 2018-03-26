using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FatigueSys
{
    public float fatigue = 0.0f;
    public float maxFatigue = 1000.0f;

    private const float fatigueIncrease = 1.0f;
    private const float fatigueDecrease = 0.5f;

    #region Méthodes de fatigue

    public void Running()
    {
        fatigue = Mathf.Clamp(fatigue + (fatigueIncrease), 0.0f, maxFatigue); //Rajoute de la fatigue en l'empechant de descendre en dessous de 0 et au dessus de 1000        
    }

    public void Walking()
    {
        fatigue = Mathf.Clamp((fatigue + (fatigueIncrease)) / 5f, 0.0f, maxFatigue);
    }

    public void Jumping()
    {
        fatigue += 50.0f;
    }

    public void crouchWalking()                                                        //Augmente la fatigue quand il avance accroupi
    {
        fatigue = Mathf.Clamp((fatigue + (fatigueIncrease)) * 1.5f, 0.0f, maxFatigue);
    }

    public void Throw()                                                                //Fatigue sur le lancer de grenade
    {
        fatigue += 25.0f;
    }

    public void Crouched()                                                             //Diminue la fatigue quand accroupi
    {
        if (fatigue >= 100.0f)
        {
            fatigue = Mathf.Clamp((fatigue - (fatigueIncrease)) / 1.5f, 0.0f, maxFatigue);
        }
    }

    public void Idle()
    {
        if (fatigue >= 100.0f)
        {
            fatigue = Mathf.Clamp(fatigue - (fatigueDecrease), 0.0f, maxFatigue); //Réduit la fatigue en l'empechant de descendre en dessous de 0 et au dessus de 1000
        }
    }

    #endregion

    #region Méthodes booléennes

    public bool isAbleToJump()
    {
        if (this.fatigue < 500.0f) return true;
        else return false;
    }
    public bool isAbleToRun()
    {
        if (this.fatigue < 750.0F) return true;
        else return false;
    }

    public bool isAbleToHeal()                                  //Empeche de regen de la vie si trop fatigué
    {
        if (this.fatigue < 750.0F) return true;
        else return false;
    }

    public bool isAbleToThrow()                                 //Empeche de lancer des grenades
    {
        if (this.fatigue < 800.0f) return true;
        else return false;
    }

    public bool isAbleToAim()                                   //Empeche de viser en 1ere personne
    {
        if (this.fatigue < 900.0f) return true;
        else return false;
    }

    public bool gunSway()                                      //Pour les problemes de visees
    {
        if (this.fatigue > 350.0f) return true;
        else return false;
    } 

    #endregion
}
