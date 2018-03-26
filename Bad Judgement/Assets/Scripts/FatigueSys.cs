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

    public void Running()
    {
        fatigue = Mathf.Clamp(fatigue + (fatigueIncrease), 0.0f, maxFatigue); //Rajoute de la fatigue en l'empechant de descendre en dessous de 0 et au dessus de 1000        
    }
    public void Jumping()             
    {
        fatigue += 50.0f;
    }

    public void Idle()
    {
        if (fatigue >= 100.0f)
        {
            fatigue = Mathf.Clamp(fatigue - (fatigueDecrease), 0.0f, maxFatigue); //Réduit de la fatigue en l'empechant de descendre en dessous de 0 et au dessus de 1000
        }
    }
    
    public bool isAbleToRun()
    {
        if (this.fatigue < 750.0F) return true;
        else return false;
    }

}
