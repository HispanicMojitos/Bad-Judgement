using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Movement : MonoBehaviour
{
    #region Members

    #region Sounds members
    [SerializeField] private AudioClip sonCourse;
    [SerializeField] private AudioClip sonDePas;
    [SerializeField] private AudioClip tombe;
    [SerializeField] private AudioClip saute;
    [SerializeField] private AudioSource[] pieds = new AudioSource[2]; // Source pour les pruits de sauts
    [SerializeField] private AudioSource personnage;
    [Range(0f, 1f)] // Permet de regler le volume via un bouton de reglage dans Unity
    private float volumeDesSonsDePas = 0.2F;
    private char tagNew = '\0'; // donne la valeur null du char, cette variable permet de faire en sorte dans l'algorithme de continuer un son de pas même lorsqu'on entre en collision avec un autre objet non tagué (exemple : un mur)

    #endregion Sounds members

    private float forwardSpeed = 4.2F;
    private float backwardSpeed = 2.8F;
    private float sideSpeed = 2.15F; //Speeds
    //private float strafeSpeed; //We'll be able to strafe fast. => WIP (2.88 KMH).
    private float runMultiplier = 1.6F; //If the player wants to run, his forward speed will be multiplicated by 1.6

    private float jumpForce = 4.5F; //Force of the jump that the character will have

    private float normalCrouchDeltaH = 0.6F;
    private float onTheKneesCrouchDeltaH = 0.35F; //The height the character will lose while crouching

    private bool wantsToRun; //To know is the character wants to run
    private bool characterCanJump; //Useful for the jump move

    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private CapsuleCollider playerCollider; //Getting thos components via editor

    //List<string> animParametersList; //New list w/ names of booleans to handle animations

    FatigueSys fatigue;
    
    #endregion

    #region Properties & readonly

    //Properties accessible in readonly in other scripts :

    public bool characterIsMoving { get; private set; }  //Working
    /// <summary>True if character is walking BUT false if running !!!!</summary>
    public bool characterIsWalking { get; private set; } //Working
    public bool characterIsRunning { get; private set; } //Working
    /// <summary>[NOT WORKING/WIP]Get if the character has been jumping during previous frame</summary>
    public bool characterIsJumping { get; private set; } //Working
    public bool characterIsCrouched { get; private set; } //Working
    /// <summary>NOT WORKING => WIP</summary>
    public bool characterIsGrounded { get; private set; } //NOT WORKING => WIP

    //Those allows to get player's exhaust and allows creating percentage w/ Max. exhaust (designed for UI) :

    public float playerExhaust { get { return this.fatigue.fatigue; } }
    public float playerMaxExhaust { get { return this.fatigue.maxFatigue; } }

    #endregion

    #region Start and Update

    // Use this for initialization
    void Start()
    {
        this.fatigue = new FatigueSys(); //Instanciating new exhaust system (Class by JS)

        this.InitState(); //Initializing character state (Setting booleans correclty before starting playing)

        //this.animParametersList = AnimatorHandling.GetParameterNames(this.anim);
        //We send our animator to get a whole list of the animator's parameters. This will allow us to disable all the bools we don't need in only one line !
    }

    // Update is called once per frame
    void Update()
    {
        #region Ground Move

        if (characterIsGrounded) this.Move();

        #endregion

        #region Jump

        //Checking for Jump :
        //If player wants to jump AND that character can jump AND that character isn't crouched :
        if (Input.GetButtonDown("Jump") && this.characterCanJump && !(characterIsCrouched))
        {
            Jump(); //Makes the character jump
            characterIsJumping = true; //Setting the property for other scripts
        }
        else characterIsJumping = false; //Setting property for other scripts

        #endregion

        #region Crouch

        if (Input.GetKeyDown(KeyCode.C)) Crouch(this.normalCrouchDeltaH);
        //Doing two != types of crouching depending on the key pressed
        if (Input.GetKeyDown(KeyCode.B)) Crouch(this.onTheKneesCrouchDeltaH);

        #endregion

        Debug.Log(this.characterIsCrouched);
    }

    #endregion

    #region Moving Methods

    private void Jump()
    {
        #region sound
        Sounds.Jump(personnage, saute);
        #endregion sound

        Vector3 jump = new Vector3(0F, jumpForce, 0F); //Making the jump by setting the velocity to the jump force
        //playerRigidbody.velocity += jump;
        playerRigidbody.AddForce(jump, ForceMode.VelocityChange);
        this.characterCanJump = false; //Telling that the player may not jump

        //In the sounds part, there's the method that handles OnCollisionEnter event. I just added this.characterCanJump = true
        //So that when the player touches the ground again, he can jump.
    }

    private void Move() //Fully reworked to correct collision bugs
    {
        Vector3 currentVelocity = playerRigidbody.velocity; //Getting current velocity
        Vector3 targetSpeed = new Vector3(Input.GetAxis("Horizontal"), currentVelocity.y, Input.GetAxis("Vertical")); //Calculating new velocity


        if (targetSpeed.x != 0 || targetSpeed.z != 0)
        {
            bool wantsToRun = Input.GetKey(KeyCode.LeftShift); //Checking if player pressed Lshift (means that he wants to run)

            if (targetSpeed.z > 0) targetSpeed.z *= forwardSpeed; //If going forward multiplying by forward speed
            else targetSpeed.z *= backwardSpeed; //If going backwards, multiplying by backwards speed that is lower than forward one
            targetSpeed.x *= sideSpeed; //Assigning speeds to each component of the moving Vector

            targetSpeed = transform.TransformDirection(targetSpeed); //Doing a transformDirection to be able to turn the axes
            //Have to keep this after applying speeds so that we don't have speeds/direction problems

            if (wantsToRun && this.fatigue.isAbleToRun()) //If player wants to run, we increase movement speed by a number that'll change depending on exhaust
            {
                targetSpeed.x *= runMultiplier;
                targetSpeed.z *= runMultiplier;

                this.SetStateRun();

                #region sound
                Sounds.Marche(pieds, sonCourse, this.characterCanJump);
                #endregion sound
            }
            else this.SetStateWalk();

            Vector3 deltaMove = targetSpeed - currentVelocity;

            //Not doing the difference between actual velocity and new one would provoke a kind of acceleration which we don't want

            playerRigidbody.AddForce(deltaMove, ForceMode.VelocityChange); //Applying that force to the player. Multiplying by 50 (float) to get something strong enough.

            #region sound
            if (!wantsToRun) Sounds.Marche(pieds, sonDePas, this.characterCanJump);
            #endregion  
        }
        else this.SetStateIdle();
    }

    private void Crouch(float deltaHeight)
    {
        if (characterIsCrouched) deltaHeight *= -(1.0F);
        //If the player is crouched then we make the height higher and not lower

        this.playerCollider.height -= deltaHeight;
        this.playerCollider.center -= new Vector3(0F, (deltaHeight / 2), 0F);

        this.characterIsCrouched = InvertBool(characterIsCrouched);
    }

    #endregion

    #region Animation Methods

    public void Dying()
    {

    }

    #endregion

    #region Other Methods 

    private bool InvertBool(bool toInvert)
    {
        if (toInvert) toInvert = false;
        else toInvert = true;

        return toInvert;
    }

    private void SetStateRun()
    {
        this.characterIsMoving = true;
        this.characterIsRunning = true;

        this.characterIsWalking = false;
    }

    private void SetStateWalk()
    {
        this.characterIsMoving = true;
        this.characterIsWalking = true;

        this.characterIsRunning = false;
    }

    private void SetStateIdle()
    {
        this.characterIsRunning = false;
        this.characterIsWalking = false;
        this.characterIsMoving = false;
    }

    private void InitState()
    {
        this.characterIsMoving = false;
        this.characterIsRunning = false;
        this.characterIsWalking = false;
        this.characterIsCrouched = false;
        this.characterIsJumping = false;

        this.characterIsGrounded = true;
    }

    #endregion

    #region jump detection area

    private void OnCollisionEnter(Collision collision) { this.characterCanJump = true; }

    #endregion

}
