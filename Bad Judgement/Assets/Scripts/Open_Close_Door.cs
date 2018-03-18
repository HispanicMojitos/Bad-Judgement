using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Open_Close_Door : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}
    #region Variables
    bool open;
    bool entry;
    #endregion

    #region DistanceMethods
    public void OnTriggerEnter(Collider other)//To activate opening when player is near
    {
        if (other.gameObject.tag == "Player") entry = true;
    }
    public void OnTriggerExit(Collider other) //To desactivate when he's far away
    {
        if (other.gameObject.tag == "Player") entry = !entry;
    }
    #endregion

    // Update is called once per frame
    void Update () {
        var speed = 2.0F;

        if (entry == true) { if (Input.GetKeyDown("e")) open = true; };

        if (open==true)
        {
            var door = Quaternion.Euler(0, 90, 0); //To rotate the object with 90°
            transform.localRotation = Quaternion.Slerp(transform.localRotation, door, Time.deltaTime * speed); //Rotation with time decided
        }

        if(open==false)
        {
            var door = Quaternion.Euler(0, 0, 0); //To close it
            transform.localRotation = Quaternion.Slerp(transform.localRotation, door, Time.deltaTime * speed);
        }
    }

}
