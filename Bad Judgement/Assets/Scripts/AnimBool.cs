using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimBool : MonoBehaviour
{
    private string animName;
    private bool animState;

    #region Ctor

    public AnimBool(string name, bool state)
    {
        this.animName = name;
        this.animState = state;
    }

    #endregion
    
    public void SetStateToFalse() { this.animState = false; }

    public void SetName(string name) { this.animName = name; }
}
