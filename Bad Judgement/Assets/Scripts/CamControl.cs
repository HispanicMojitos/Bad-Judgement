using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    private Camera cam; //Camera (it's a children component, this script has to be applied to the player !!!!!)
    public Transform target;

    //private float minVertical = -55.0F;
    //private float maxVertical = 55.0F; //Maximum head angle is 55 degrees. Modifiable here.

    private float horizontalSensitivity = 6.0F;
    private float verticalSensitivity = 10.0F;

    public bool isVerticalAxisInverted = false; //FOR YOU LERUTH  //WIP

    // Use this for initialization
    void Start ()
    {
        cam = Camera.main; //Starting with the main cam which is the FPS one
                           //As project I'd like to put a second cam which could be changed to TPS if we press a button

        //Has to implement invert Y-axis
	}
	

	// Update is called once per frame
	void Update ()
    {
        float yGameAxis = Input.GetAxis("Mouse X"); //The horizontal (X) mouse axis matches with the ingame y rotation axis
        float xGameAxis = Input.GetAxis("Mouse Y"); //The vertical (Y) mouse axis matches with the ingame x rotation axis

        yGameAxis *= verticalSensitivity;
        xGameAxis *= horizontalSensitivity; //Appliying sensitivity to the camera axes.

        if(isVerticalAxisInverted) this.cam.transform.Rotate(-xGameAxis, 0F, 0F); 
        else this.cam.transform.Rotate(xGameAxis, 0F, 0F); //Vertically rotating (applied to the cam)

        this.transform.Rotate(0F, yGameAxis, 0F); //Horizontal rotate (applied to the player)

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Mouse0)) this.InvertAxis();
        //If LeftAlt + LeftClick => Inverting Y axis
    }

    public void InvertAxis()
    {
        if(this.isVerticalAxisInverted) this.isVerticalAxisInverted = false;
        else this.isVerticalAxisInverted = true;
    }

}
