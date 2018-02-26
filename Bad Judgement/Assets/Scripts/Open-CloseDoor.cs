using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoor : MonoBehaviour {

    public Animator door;  //Call the animator

	// Use this for initialization
	void Start () {

        door = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("E")) door.Play("DoorOpen");

	}
}
