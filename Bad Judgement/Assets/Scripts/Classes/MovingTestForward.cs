using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTestForward : MonoBehaviour {

    public GameObject moveForward;
    public GameObject moveBacward;
    public GameObject moveLeft;
    public GameObject moveRight;
    public GameObject lookLeft;
    public GameObject lookRight;
    public GameObject lookUp;
    public GameObject lookDown;
    public GameObject continueOne;

    public int collision = 0;                     // will permit to know which command to ask      
    public float vision;
    public float view;

    void Start ()
    {                                                   //set the texts invisible
        moveForward.SetActive(false);
        moveBacward.SetActive(false);
        moveLeft.SetActive(false);
        moveRight.SetActive(false);
        lookDown.SetActive(false);
        lookLeft.SetActive(false);
        lookRight.SetActive(false);
        lookUp.SetActive(false);
        continueOne.SetActive(false);
    }
    void OnTriggerEnter(Collider other)                     // do when entering trigger
    {
        if (other.gameObject.tag == "Player" && collision == 0)
        {
            moveForward.SetActive(true);
            collision = 1;
        }
        if (other.gameObject.tag == "Player" && collision == 1 && Input.GetButton("Backward"))
        {
            moveBacward.SetActive(false);
            moveLeft.SetActive(true);
            collision = 2;
        }
        if (other.gameObject.tag == "Player" && collision == 2 && Input.GetButton("Right"))
        {
            moveRight.SetActive(false);
            lookUp.SetActive(true);
            collision = 3;
        }
       
    }
    void OnTriggerExit(Collider other)                      // do while exiting trigger
    {
        if (Input.GetButton("Forward") && collision == 1 && other.gameObject.tag == "Player")
        {
            moveForward.SetActive(false);
            moveBacward.SetActive(true);
        }
        if (Input.GetButton("Left") && collision == 2 && other.gameObject.tag == "Player")
        {
            moveLeft.SetActive(false);
            moveRight.SetActive(true);
        }
        if (other.gameObject.tag == "Player" && collision == 6)
        {
            continueOne.SetActive(false);
        }
    }
    private void OnTriggerStay(Collider other)          // do while staying in trigger
    {
        vision = Input.GetAxis("Mouse Y");
        view = Input.GetAxis("Mouse X");
        if (other.gameObject.tag == "Player" && collision == 3 && vision < 0)
        {
            lookUp.SetActive(false);
            lookDown.SetActive(true);
            collision = 4;
        }
        if (other.gameObject.tag == "Player" && collision == 4 && vision > 0)
        {
            lookDown.SetActive(false);
            collision = 5;
            lookLeft.SetActive(true);
        }
        if (other.gameObject.tag == "Player" && collision == 5 && view < 0)
        {
            lookLeft.SetActive(false);
            lookRight.SetActive(true);
            collision = 6; ;
        }
        if(other.gameObject.tag == "Player" && collision == 6 && view > 0)
        {
            lookRight.SetActive(false);
            continueOne.SetActive(true);
        }
    }
    void Update () {
		
	}
}
