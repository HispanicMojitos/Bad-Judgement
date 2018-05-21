using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTestForward : MonoBehaviour {
    #region Members

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject IA;

    private Movement playerMove;
    private TestInteraction playerInteract;
    private Target enemyKill;
    private AIally Ally;
    private TakeMoney moneyCount;

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
    public GameObject weaponSelection;
    public GameObject Interaction;
    public GameObject Kill;
    public GameObject weaponTake;
    public GameObject Run;
    public GameObject Jump;
    public GameObject AllyHeal;
    public GameObject AllyOrder;
    public GameObject ExecAlly;
    public GameObject Grenada;
    public GameObject leanLeft;
    public GameObject leanRight;
    public GameObject continueTwo;
    public GameObject onRight;
    public GameObject Aim;


    public int collision = 0;                     // will permit to know which command to ask      
    public float vision;
    public float view;
    public float scroll;
    #endregion

    #region Start&Update
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
        Run.SetActive(false);
        Jump.SetActive(false);
        AllyHeal.SetActive(false);
        AllyOrder.SetActive(false);
        ExecAlly.SetActive(false);
        Grenada.SetActive(false);
        leanLeft.SetActive(false);
        leanRight.SetActive(false);
        weaponSelection.SetActive(false);
        weaponTake.SetActive(false);
        continueTwo.SetActive(false);
        onRight.SetActive(false);
        Aim.SetActive(false);

        playerMove = player.GetComponent<Movement>();
        playerInteract = player.GetComponentInChildren<TestInteraction>();
        moneyCount = player.GetComponentInChildren<TakeMoney>();
        enemyKill = enemy.GetComponent<Target>();
        Ally = IA.GetComponent<AIally>();
       
    }
    void Update()
    {
        TestOutdoor();
        Indoor();
    }
#endregion

    #region outdoor

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
            collision ++;
        }
        if (playerMove.characterMovingRight == true && collision == 2)
        {
            moveRight.SetActive(false);
            moveLeft.SetActive(true);
            collision++;
        }
        if (playerMove.charactermovingLeft == true && collision == 3)
        {
            moveLeft.SetActive(false);
            lookUp.SetActive(true);
            collision++;
        }
        if (collision == 4 && vision < 0)
        {
            lookUp.SetActive(false);
            lookDown.SetActive(true);
            collision++;
        }
        if (collision == 5 && vision > 0)
        {
            lookDown.SetActive(false);
            lookRight.SetActive(true);
            collision++;
        }
        if (collision == 6 && view > 0)
        {
            lookRight.SetActive(false);
            lookLeft.SetActive(true);
            collision++;
        }
        if (collision == 7 && view < 0)
        {
            lookLeft.SetActive(false);
            Run.SetActive(true);
            collision++;
        }
        if (collision == 8 && Input.GetKey(KeyCode.LeftShift))
        {
            Run.SetActive(false);
            Jump.SetActive(true);
            collision++;
        }
        if (collision == 9 && Input.GetKey(KeyCode.Space))
        {
            Jump.SetActive(false);
            fireTest.SetActive(true);
            collision++;
        }
        if (collision == 10 && Input.GetButton("Fire1"))
        {
            fireTest.SetActive(false);
            Aim.SetActive(true);
            collision++;
        }
        if (collision == 11 && Input.GetButton("Fire2"))
        {
            Aim.SetActive(false);
            reloadTest.SetActive(true);
            collision++;
        }
        if (collision == 12 && Input.GetKey(KeyCode.R))
        {
            reloadTest.SetActive(false);
            weaponSelection.SetActive(true);
            collision++;
        }
        if (collision == 13 && scroll < 0)
        {
            weaponSelection.SetActive(false);
            AllyOrder.SetActive(true);
            collision++;
        }
        if (Ally.ordreDeplacement == true && Input.GetKey(KeyCode.T) && collision == 14)
        {
            AllyOrder.SetActive(false);
            ExecAlly.SetActive(true);
            collision++;
        }
        if (Ally.estHS == true && collision == 15)
        {
            ExecAlly.SetActive(false);
            AllyHeal.SetActive(true);
            collision++;
        }
        if (Input.GetKeyUp(KeyCode.H) && collision == 16)
        {
            AllyHeal.SetActive(false);
            continueOne.SetActive(true);
            collision++;
        }

    }
    #endregion

    #region Indoor
    void Indoor()
    {
        scroll = Input.GetAxis("Mouse ScrollWheell");
        if (playerInteract.isInteractionImageOn == true && collision == 17)
        {
            continueOne.SetActive(false);
            Interaction.SetActive(true);
            collision ++;
        }
        
        if (collision == 18 && Input.GetKey(KeyCode.F) && playerInteract.decisionPorte == true)
        {
            Interaction.SetActive(false);
            Kill.SetActive(true);
            collision++;
        }
        if (enemyKill.vie <=0 && collision == 19)
        {
            Kill.SetActive(false);
            weaponTake.SetActive(true);
            collision++;
        }
        if (collision == 20 && Input.GetKey(KeyCode.F))
        {
            weaponTake.SetActive(false);
            Grenada.SetActive(true);
            collision++;
        }
        
        if (collision == 21 )
        {
            Grenada.SetActive(false);
            leanRight.SetActive(true);
            collision++;
        }
        if (collision == 22 && Input.GetKey(KeyCode.E))
        {
            leanRight.SetActive(false);
            leanLeft.SetActive(true);
            collision++;
        }
        if (collision == 23 && Input.GetKey(KeyCode.A))
        {
            leanLeft.SetActive(false);
            continueTwo.SetActive(true);
            collision++;
        }
        if (collision == 24 && playerInteract.isInteractionImageOn == true)
        {
            continueTwo.SetActive(false);
            onRight.SetActive(true);
            collision++;
        }
        if (collision == 25 && moneyCount.moneyText == true)
        {
            onRight.SetActive(false);
            if(moneyCount.moneyCount == 3)
            {
                collision++;
            }
        }
        if (collision == 26)
        {
            moneyCount.moneyCount = 0;
            MissionSave.NextMission();
            Teleport.ToVan();
        }
        
        
    }
    #endregion
}
