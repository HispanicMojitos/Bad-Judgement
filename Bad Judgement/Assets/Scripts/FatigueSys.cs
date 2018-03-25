using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class FatigueSys
{
    

    public float fatigue = 0.0f;
    public float maxFatigue = 1000.0f;

    public bool running;
    public bool jumping;
    public bool idle;

    private const float fatigueIncrease = 1.0f;
    private const float fatigueDecrease = 0.5f;

    public void Running()
    {
        if (running)
        {
            fatigue = Mathf.Clamp(fatigue + (fatigueIncrease * Time.deltaTime), 0.0f, maxFatigue); //Rajoute de la fatigue en l'empechant de descendre en dessous de 0 et au dessus de 1000
        }
    }
    public void Jumping()             
    {
        if ( jumping)
        {
            fatigue += 50.0f;
        }
    }
    public void Idle()
    {
        if (idle && fatigue >= 100.0f)
        {
            fatigue = Mathf.Clamp(fatigue - (fatigueDecrease * Time.deltaTime), 0.0f, maxFatigue); //Réduit de la fatigue en l'empechant de descendre en dessous de 0 et au dessus de 1000
        }
    }
    

    

}
