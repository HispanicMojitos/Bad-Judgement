using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerInverseKinematics : MonoBehaviour {

    private Animator anim;
    public Transform leftIKHandTarget;
    public Transform rightIKHandTarget;
    public float IKWeight = 1;
    
    // Use this for initialization
    void Start ()
    {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, IKWeight);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, IKWeight);
        //this is the force we want to have

        anim.SetIKPosition(AvatarIKGoal.LeftHand, leftIKHandTarget.position);
        anim.SetIKPosition(AvatarIKGoal.RightHand, rightIKHandTarget.position);
        //this part sets the right and left hand to towards the target
    }
}
