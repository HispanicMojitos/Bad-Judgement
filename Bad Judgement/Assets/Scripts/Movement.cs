using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Members

    private float forwardSpeed;
    private float backwardSpeed;
    private float sideSpeed;
    private float verticalSpeed;
    private float strafeSpeed; //We'll be able to strafe fast. => WIP (2.88 KMH).

    protected new Rigidbody rigidbody = new Rigidbody();
    private CharacterController charCtrl = new CharacterController();

    #endregion

    #region Properties

    public bool characterIsMoving { get; private set; }
    public bool characterIsJumping { get; private set; }

    #endregion

    #region Start and Update

    // Use this for initialization
    void Start()
    {
        //These speeds are based on my real tests and are in KMH :
        forwardSpeed = 4.2F;
        backwardSpeed = (0.66F * forwardSpeed); //After real tests, reverse speed is 2/3 times of forward speed.
        sideSpeed = 2.15F;
        verticalSpeed = 4.5F;

        //Getting components :
        rigidbody = GetComponent<Rigidbody>();
        charCtrl.GetComponent<CharacterController>(); //We search the rigidbody and the charController in the player

        //To be moved later :
        Cursor.lockState = CursorLockMode.Locked;
    }


    // Update is called once per frame
    void Update()
    {
        //Checking for Ground Moving :

        float xAxis = Input.GetAxis("Horizontal") * sideSpeed * Time.deltaTime;
        float zAxis = Input.GetAxis("Vertical") * Time.deltaTime;

        if (zAxis != 0 || xAxis != 0) //If the player is moving :
        {
            Move(zAxis, xAxis); //We're moving on X and Z
            characterIsMoving = true;
        }
        else characterIsMoving = false; 

        //========================================================================================

        //Checking for Jump :

        float yAxis = Input.GetAxis("Jump"); //Jump axis (space key)

        if (yAxis != 0)
        {
            Jump(yAxis);
            characterIsJumping = true;
        }
        else characterIsJumping = false;

        //========================================================================================

        //To be moved :
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        if (Cursor.lockState == CursorLockMode.None && Input.GetKey(KeyCode.Mouse0)) Cursor.lockState = CursorLockMode.Locked;
    }

    #endregion

    #region Moving Methods

    private void Jump(float yAxis)
    {
        yAxis = yAxis * verticalSpeed; //Affecting vertical jump speed and consider machine responsiveness
        Vector3 moveVertical = new Vector3(0F, yAxis, 0F); //Creating the vector that contains the vertical !=ce
        moveVertical *= Time.deltaTime; //Machine responsiveness

        this.transform.Translate(moveVertical); //Making the jump
        //The character will be automatically brought back to the ground due to gravity.
    }

    private void Move(float zAxis, float xAxis)
    {
        if (zAxis < 0) zAxis *= backwardSpeed;
        else zAxis *= forwardSpeed; //Forward speed != than backward speed

        Vector3 movement = new Vector3(xAxis, 0F, zAxis);
        //X is the strafe and Z is forward/backward

        this.transform.Translate(movement); //Making the move
    }

    #endregion

}
