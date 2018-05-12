using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControl : MonoBehaviour
{
    #region Members

    private Camera cam; //Current camera (it's a children component, this script has to be applied to the player !!!!!)
    [SerializeField] private Camera normalCam; //Normal camera which isn't free moveable
    [SerializeField] private Camera freeCam; //Reference to the free cam that hasn't a gun attached on it. Disabled by default

    [SerializeField] private float minVertical = 70.0F;
    [SerializeField] private float maxVertical = -55.0F; //Maximum head angle is 55 degrees. Modifiable here.

    [SerializeField] private bool freeCamByDefault;
    [SerializeField] private bool camChangeAuthorized;

    //[SerializeField] private float maxLeftRightFreeCamAngle = 70.0F; //Not used for now but it is going to be used 

    private Vector3 verticalEulerVector; //Vector to handle vertical rotation of the camera
    private Vector3 horizontalEulerVector; //Vector to handle the horizontal rotation of the camera (IF FREE CAMERA MODE ACTIVE)

    // Recoil
    private float xKick = 0f;
    private float yKick = 0f;
    private float kickForce = 0.8f;
    // Recoil

    #endregion

    #region Properties

    public static bool isVerticalAxisInverted { get; set; } //For you LERUTH

    public bool isFreeCamActive { get; private set; }

    public static float verticalSensitivity { get; set; }
    public static float horizontalSensitivity { get; set; }

    public static bool isMoveable { get; set; }

    #endregion

    #region Start and Update

    // Use this for initialization
    void Start()
    {
        isMoveable = true;
        //cam = normalCam; //Starting with the main cam which is the FPS one
                         //As project I'd like to put a second cam which could be changed to TPS if we press a button

        if (freeCamByDefault)
        {
            cam = freeCam;
            isFreeCamActive = true;
        }
        else
        {
            cam = normalCam;
            isFreeCamActive = false;
        }

        //Initializing properties :
        isVerticalAxisInverted = false;
        horizontalSensitivity = 5.0F;
        verticalSensitivity = 5.0F;

        verticalEulerVector = Vector3.zero;
        horizontalEulerVector = Vector3.zero;
    }


    // Update is called once per frame
    void Update()
    {
        if (GunScript.isShooting)
        {
            if (xKick != 1f)
            {
                xKick += 0.1f;
                yKick += 0.00001f;
            }
            cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(-xKick, Random.Range(-yKick, yKick)), kickForce * Time.deltaTime);
        }
        else cam.transform.localEulerAngles = Vector3.Lerp(cam.transform.localEulerAngles, new Vector3(0, 0), kickForce * Time.deltaTime);

        if (!UIScript.gameIsPaused)
        {
            float yGameAxis = Input.GetAxis("Mouse X"); //The horizontal (X) mouse axis matches with the ingame y rotation axis
            float xGameAxis = Input.GetAxis("Mouse Y"); //The vertical (Y) mouse axis matches with the ingame x rotation axis

            if (isMoveable)
            {
                if (isFreeCamActive) MoveCamHorizIfFreeCam(yGameAxis); //Rotating the camera because of freecam mode
                else MoveCamHoriz(yGameAxis); //Rotating the player on its Y-axis.

                MoveCamVertically(xGameAxis); //Moving the camera vertically (X axis)
            }

            if (Input.GetKeyDown(KeyCode.LeftAlt) && camChangeAuthorized) ChangeFreeCamOrNot();
            //Setting Camera to free if alt is pressed, reverting action by pressing a second time
        }
    }

    #endregion

    #region Cam Methods

    private void MoveCamVertically(float xGameAxis)
    {
        this.cam.transform.localEulerAngles = verticalEulerVector; //Refreshing cam movement every frame

        float verticalRotation = xGameAxis * verticalSensitivity; //Applying sensitivity
        if (isVerticalAxisInverted) verticalRotation *= (-1F);
        verticalEulerVector.x += verticalRotation; //Adding the mouse axis to the actual rotation

        if (verticalEulerVector.x >= minVertical) verticalEulerVector.x = minVertical;
        if (verticalEulerVector.x <= maxVertical) verticalEulerVector.x = maxVertical;
    }

    private void MoveCamHoriz(float yGameAxis)
    {
        yGameAxis *= horizontalSensitivity; //Applying sensitivity

        this.transform.Rotate(0F, yGameAxis, 0F); //Horizontal rotate (applied to the player)
    }

    //WIP :
    private void MoveCamHorizIfFreeCam(float yGameAxis)
    {
        yGameAxis *= horizontalSensitivity;
        Vector3 freeCamHorizontalRotation = new Vector3 (0F, yGameAxis, 0F);
        this.cam.transform.localEulerAngles += freeCamHorizontalRotation;
    }

    private void ChangeFreeCamOrNot()
    {
        if (isFreeCamActive) //If we want to have the normal camera
        {
            this.normalCam.enabled = true;
            this.cam = normalCam;
            this.freeCam.enabled = false;

            this.isFreeCamActive = false;
        }
        else //If we want to have the free camera mode
        {
            this.freeCam.enabled = true;
            this.cam = freeCam;
            this.normalCam.enabled = false;

            this.isFreeCamActive = true;
        }
    }

    #endregion
}
