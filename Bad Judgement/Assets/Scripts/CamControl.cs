using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    #region Members

    private Camera cam; //Camera (it's a children component, this script has to be applied to the player !!!!!)

    private float minVertical = 60.0F;
    private float maxVertical = -55.0F;//Maximum head angle is 55 degrees. Modifiable here.

    private Vector3 verticalEulerVector; //Création d'un vecteur pour la rotation verticale de la caméra

    #endregion
    
    #region Properties

    public bool isVerticalAxisInverted { get; private set; } //For you LERUTH // MERCI!!

    public float horizontalSensitivity { get; set; }
    public float verticalSensitivity { get; set; }

    #endregion

    #region Start and Update

    // Use this for initialization
    void Start()
    {
        cam = Camera.main; //Starting with the main cam which is the FPS one
                           //As project I'd like to put a second cam which could be changed to TPS if we press a button
       
        //Initializing properties :
        this.isVerticalAxisInverted = false;
        this.horizontalSensitivity = 6.0F;
        this.verticalSensitivity = 6.0F;

        verticalEulerVector = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        float yGameAxis = Input.GetAxis("Mouse X"); //The horizontal (X) mouse axis matches with the ingame y rotation axis
        float xGameAxis = Input.GetAxis("Mouse Y"); //The vertical (Y) mouse axis matches with the ingame x rotation axis

        if (yGameAxis != 0) MoveCamHoriz(yGameAxis); //Rotating the player on Y axis (horizontally)
        MoveCamVertically(xGameAxis); //Moving the camera vertically (X axis)
        
        
        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Mouse0)) this.InvertAxis();
        //If LeftAlt + LeftClick => Inverting Y axis
    }

    #endregion

    #region Cam Methods

    private void InvertAxis()
    {
        if (this.isVerticalAxisInverted) this.isVerticalAxisInverted = false;
        else this.isVerticalAxisInverted = true;
    }

    private void MoveCamVertically(float xGameAxis)
    {
        this.cam.transform.localEulerAngles = verticalEulerVector; //Refreshing cam movement every frame

        float verticalRotation = xGameAxis * verticalSensitivity;
        verticalEulerVector.x += verticalRotation; //Adding the mouse axis to the actual

        if (verticalEulerVector.x >= minVertical) verticalEulerVector.x = minVertical;
        if (verticalEulerVector.x <= maxVertical) verticalEulerVector.x = maxVertical;
    }

    private void MoveCamHoriz(float yGameAxis)
    {
        yGameAxis *= horizontalSensitivity; //Applying vertical sensitivity

        this.transform.Rotate(0F, yGameAxis, 0F); //Horizontal rotate (applied to the player)
    }

    #endregion

}
