using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Equipment : MonoBehaviour
{
    protected new string name { get; private set; }
    public Sprite uiSprite { get; set; }
    
    public Equipment(string name)
    {
        this.name = name;
        this.uiSprite = uiSprite;
    }
}
