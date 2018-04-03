using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarCreator : MonoBehaviour {

    public static GameObject go;
    public static HumanDescription human;
    Avatar avatar;
    Animator anim;
	// Use this for initialization
	void Start () {
        go = this.gameObject;
        avatar = AvatarBuilder.BuildHumanAvatar(go, human);
        avatar.name = "Jeff";
        anim = go.GetComponent<Animator>();
        anim.avatar = avatar;
        }
	
	// Update is called once per frame
	void Update () {
		
	}
}
