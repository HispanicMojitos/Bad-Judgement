using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTestForward : MonoBehaviour {
    #region Members

    [SerializeField] private GameObject player;
    private Movement playerMove;
   
    public GameObject moveForward;
    public GameObject moveBacward;
    public GameObject moveLeft;
    public GameObject moveRight;
    public GameObject lookLeft;
    public GameObject lookRight;
    public GameObject lookUp;
    public GameObject lookDown;
    public GameObject continueOne;
    public GameObject fireTest;
    public GameObject reloadTest;
    public GameObject Interaction;
    public GameObject Kill;

    public int collision = 0;                     // will permit to know which command to ask      
    public float vision;
    public float view;
    #endregion
    void Start ()
    {                                                   //set the texts invisible
        
        moveBacward.SetActive(false);
        moveLeft.SetActive(false);
        moveRight.SetActive(false);
        lookDown.SetActive(false);
        lookLeft.SetActive(false);
        lookRight.SetActive(false);
        lookUp.SetActive(false);
        continueOne.SetActive(false);
        fireTest.SetActive(false);
        reloadTest.SetActive(false);
        Interaction.SetActive(false);
        Kill.SetActive(false);


        playerMove = player.GetComponent<Movement>();
        
    }
    void TestOutdoor()
    {
        vision = Input.GetAxis("Mouse Y");
        view = Input.GetAxis("Mouse X");
        if (playerMove.characterMovingFwd == true && moveForward.activeInHierarchy == true)
        {
            moveForward.SetActive(false);
            moveBacward.SetActive(true);
            collision = 1;
        }
        if (playerMove.characterMovingBwd == true && collision == 1)
        {
            moveBacward.SetActive(false);
            moveRight.SetActive(true); ;
            collision = 2;
        }
        if (playerMove.characterMovingRight == true && collision == 2)
        {
            moveRight.SetActive(false);
            moveLeft.SetActive(true);
            collision = 3;
        }
        if (playerMove.charactermovingLeft == true && collision == 3)
        {
            moveLeft.SetActive(false);
            lookUp.SetActive(true);
            collision = 4;
        }
        if (collision == 4 && vision < 0)
        {
            lookUp.SetActive(false);
            lookDown.SetActive(true);
            collision = 5;
        }
        if (collision == 5 && vision > 0)
        {
            lookDown.SetActive(false);
            lookRight.SetActive(true);
            collision = 6;
        }
        if (collision == 6 && view > 0)
        {
            lookRight.SetActive(false);
            lookLeft.SetActive(true);
            collision = 7;
        }
        if (collision == 7 && view < 0)
        {
            lookLeft.SetActive(false);
            continueOne.SetActive(true);
        }

    }
    void Update()
    {
        TestOutdoor();
        Indoor();
    }
    
    void Indoor()
    {
        


    }
}
