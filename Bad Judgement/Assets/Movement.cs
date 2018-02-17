using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float forwardSpeed;
    public float backwardSpeed;
    public float sideSpeed; 
    public float verticalSpeed;
    public float strafeSpeed; //We'll be able to strafe fast. => WIP (2.88 KMH).

    private Rigidbody rigidbody = new Rigidbody();
    private CharacterController charCtrl = new CharacterController();

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //These speeds are based on my real tests and are in KMH :

        forwardSpeed = 4.2F;
        backwardSpeed = (0.66F * forwardSpeed); //After real tests, reverse speed is 2/3 times of forward speed.
        sideSpeed = 2.15F; 
        verticalSpeed = 4.5F;

        rigidbody = GetComponent<Rigidbody>();
        charCtrl.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update()
    {
        
        float zAxis = Input.GetAxis("Vertical"); //Z is forward/backward
        float xAxis = Input.GetAxis("Horizontal"); //X is the strafe
        float yAxis = Input.GetAxis("Jump"); //Jump axis (space key)

        this.XZAxisMove(zAxis, xAxis); //Move on the ground
        Jump(yAxis); //Move vertically. You'll notice that the character has to be on the ground to jump.

        if(Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        if (Cursor.lockState == CursorLockMode.None && Input.GetKeyDown(KeyCode.Mouse0)) Cursor.lockState = CursorLockMode.Locked;
    }

    #region MovingMethods

    private void XZAxisMove(float zAxis, float xAxis)
    {
        //The forward and backward spd are !=. So, we have to dissociate backward and forward speeds :
        if (zAxis < 0) zAxis *= backwardSpeed;
        else zAxis *= forwardSpeed;

        //Treating the X-Axis :
        xAxis *= sideSpeed;

        //Creating a Vector3 to contain the x and z axes :
        Vector3 movement = new Vector3(xAxis, 0F, zAxis);
        movement *= Time.deltaTime; //For machine responsiveness.
        
        //Moving the character :
        this.rigidbody.MovePosition(this.rigidbody.position + movement);
    }

    private void Jump(float yAxis)
    {
        yAxis = yAxis * verticalSpeed; //Affecting vertical jump speed and consider machine responsiveness

        Vector3 moveVertical = new Vector3(0F, yAxis, 0F); //Creating the vector that contains the vertical !=ce
        moveVertical *= Time.deltaTime; //Machine responsiveness

        this.rigidbody.MovePosition(this.rigidbody.position + moveVertical); //Making the jump
        //The character will be automatically brought back to the ground due to gravity.
    }

    #endregion
}
