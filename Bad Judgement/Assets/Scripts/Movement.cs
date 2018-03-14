using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    #region Members

    #region Sounds members

    [SerializeField] private AudioClip sonDePasSurRue; //Initialisation des sons de pas 
    [SerializeField] private AudioClip sonDePasSurBois;
    [SerializeField] private AudioClip sonDePasSurterre;
    [SerializeField] private AudioClip jumpOnGrass; // Initialisation de sons de Jump
    [SerializeField] private AudioClip jumpOnStreet;
    [SerializeField] private AudioClip jumpOnWood;
    [SerializeField] private AudioSource personnage; // Source pour les bruits de pas normaux
    [SerializeField] private AudioSource piedjumpPersonnage; // Source pour les pruits de sauts
    [Range(0f, 1f)] // Permet de regler le volume via un bouton de reglage dans Unity
    private float volumeDesSonsDePas = 0.2F;
    private char tagNew = '\0'; // donne la valeur null du char, cette variable permet de faire en sorte dans l'algorithme de continuer un son de pas même lorsqu'on entre en collision avec un autre objet non tagué (exemple : un mur)

    #endregion Sounds members

    private float forwardSpeed = 4.2F;
    private float backwardSpeed;
    private float sideSpeed = 2.15F; //Speeds
    //private float strafeSpeed; //We'll be able to strafe fast. => WIP (2.88 KMH).
    private float runMultiplier = 1.6F; //If the player wants to run, his forward speed will be multiplicated by 2

    private float jumpForce = 4.5F; //Force of the jump that the character will have

    private float normalCrouchDeltaH = 0.6F;
    private float onTheKneesCrouchDeltaH = 0.35F; //The height the character will lose while crouching

    private bool characterCanJump; //Useful for the jump move

    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private CapsuleCollider playerCollider; //Getting thos components via editor

    [SerializeField] private Animator anim;

    #endregion

    #region Properties & readonly

    public bool characterIsMoving { get; private set; }
    public bool characterIsJumping { get; private set; } //Properties accessible in readonly in other scripts
    public bool characterIsCrouched { get; private set; }

    public bool characterIsWalkingFwd { get; private set; }
    public bool characterIsIdle { get; private set; }

    #endregion

    #region Start and Update

    // Use this for initialization
    void Start()
    {
        backwardSpeed = (0.66F * forwardSpeed); //After real tests, reverse speed is 2/3 times of forward speed.
        this.characterIsCrouched = false;

        #region sounds
        personnage.volume = volumeDesSonsDePas; // Permet de reglez les sons de pas
        piedjumpPersonnage.volume = volumeDesSonsDePas; // permet de regler les son de jumps
        #endregion 

        //To be moved later :
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; //Setting the cursor to a locked position


        this.characterIsIdle = true;
        this.characterIsWalkingFwd = false;
    }

    // Update is called once per frame
    void Update()
    {
        #region Cursor

        //To be moved :
        if (Input.GetKeyDown(KeyCode.Escape)) this.CursorUnlock();
        if (Cursor.lockState == CursorLockMode.None && Input.GetKey(KeyCode.Mouse0)) this.CursorLock();
        //Locks the cursor on the window

        #endregion

        #region Jump

        //Checking for Jump :
        //If player wants to jump AND that character can jump AND that character isn't crouched :
        if (Input.GetButtonDown("Jump") && this.characterCanJump && !(this.characterIsCrouched))
        {
            Jump(); //Makes the character jump
            this.characterIsJumping = true; //Setting the property for other scripts
        }
        else this.characterIsJumping = false; //Setting property for other scripts

        #endregion

        #region Ground Move

        //Checking for Ground Moving :
        float xAxis = Input.GetAxis("Horizontal") * sideSpeed * Time.deltaTime;
        float zAxis = Input.GetAxis("Vertical") * Time.deltaTime;
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift);

        if (wantsToRun && !this.characterCanJump) wantsToRun = this.InvertBool(wantsToRun);
        this.Move(zAxis, xAxis, wantsToRun);

        #endregion

        #region Crouch

        if (Input.GetKeyDown(KeyCode.C)) Crouch(this.normalCrouchDeltaH);

        if (Input.GetKeyDown(KeyCode.B)) Crouch(this.onTheKneesCrouchDeltaH);

        #endregion
    }

    #endregion

    #region Moving Methods

    private void Jump()
    {
        #region sound
        Sounds.JumpSound(piedjumpPersonnage); // Permet de jouer les sons de saut
        #endregion sound

        Vector3 jump = new Vector3(0F, jumpForce, 0F); //Making the jump by setting the velocity to the jump force
        playerRigidbody.velocity += jump;

        this.characterCanJump = false; //Telling that the player may not jump

        //In the sounds part, there's the method that handles OnCollisionEnter event. I just added this.characterCanJump = true
        //So that when the player touches the ground again, he can jump.
    }

    private void Move(float zAxis, float xAxis, bool wantsToRun)
    {
        if (zAxis != 0 || xAxis != 0) //If player moving
        {
            if (zAxis < 0) //We know that the character is going backwards
            {
                zAxis *= backwardSpeed;
                //PLAY WALK BACKWARD
            }
            else
            {
                zAxis *= forwardSpeed;

                if (wantsToRun)
                {
                    zAxis *= runMultiplier;
                    //PLAY RUN FORWARD
                }
                else if (xAxis == 0) this.PlayWalkForward();
            }

            Vector3 movement = new Vector3(xAxis, 0F, zAxis);
            //X is the strafe and Z is forward/backward

            this.transform.Translate(movement); //Making the move

            #region sound
            Sounds.FootSteepsSound(personnage); // Permet de jouer les sons de pas
            #endregion
        }
        else this.PlayIdle();
    }

    private void Crouch(float deltaHeight)
    {
        if (this.characterIsCrouched) deltaHeight *= -(1.0F);
        //If the player is crouched then we make the height higher and not lower

        this.playerCollider.height -= deltaHeight;
        this.playerCollider.center -= new Vector3(0F, (deltaHeight / 2), 0F);

        this.characterIsCrouched = InvertBool(this.characterIsCrouched);
    }

    #endregion

    #region Animation Methods

    private void PlayWalkForward()
    {
        //anim.SetTrigger("walkForward");
        anim.SetBool("testBoolIdle", false);
        anim.SetBool("testBoolWalk", true);
    }

    private void PlayIdle()
    {
        //anim.SetTrigger("idle");
        anim.SetBool("testBoolWalk", false);
        anim.SetBool("testBoolIdle", true);
    }

    #endregion

    #region Other Methods 

    private bool InvertBool(bool toInvert)
    {
        if (toInvert) toInvert = false;
        else toInvert = true;

        return toInvert;
    }

    private void CursorLock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void CursorUnlock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    #endregion

    #region Sounds detection area

    private void OnCollisionEnter(Collision collision) // Permet d'evaluer le son a jouer en fonction du type de sol rencontré /!\ On a besoin d'un rigibody et d'une box Colider /!\
    {
        this.characterCanJump = true;

        if ((collision.collider.CompareTag("Wood") || (tagNew == 'w' && !collision.collider.CompareTag("Wood"))) && !collision.collider.CompareTag("Street") && !collision.collider.CompareTag("Grass")) // OPTIMISATION : tag == "something"  allocates memory, CompareTag does not, i've changes this , sources : https://forum.unity.com/threads/making-stepssounds-by-using-oncollisionenter-or-raycasts-optimizations-question.518865/#post-3402025 ,https://answers.unity.com/questions/200820/is-comparetag-better-than-gameobjecttag-performanc.html
        {
            Sounds.DeclareSonDemarche(personnage, sonDePasSurBois, piedjumpPersonnage, jumpOnWood, tagNew, 'w');
        }
        else if ((collision.collider.CompareTag("Street") || (tagNew == 's' && !collision.collider.CompareTag("Street"))) && !collision.collider.CompareTag("Wood") && !collision.collider.CompareTag("Grass")) // Toute ces conditions permette de jouer le sons même en etant en colision avec d'autres objet non tagué (exemple : un mur, un ventilateur, ect...) 
        {
            Sounds.DeclareSonDemarche(personnage, sonDePasSurRue, piedjumpPersonnage, jumpOnStreet, tagNew, 's');
        }
        else if ((collision.collider.CompareTag("Grass") || (tagNew == 'g' && !collision.collider.CompareTag("Wood"))) && !collision.collider.CompareTag("Street") && !collision.collider.CompareTag("Wood"))
        {
            Sounds.DeclareSonDemarche(personnage, sonDePasSurterre, piedjumpPersonnage, jumpOnGrass, tagNew, 'g');
        }
    }

    #endregion

}
