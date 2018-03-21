using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class AnimatorHandling
{

    public static List<string> GetParameterNames(Animator anim)
    {
        List<string> animatorParameterList = new List<string>();

        List<AnimatorControllerParameter> animParamList = anim.parameters.ToList();
        for (int i = 0; i < animParamList.Count; i++) animatorParameterList[i] = animParamList[i].name;

        return animatorParameterList;
    }
}
