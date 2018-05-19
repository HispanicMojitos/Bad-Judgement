using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Movement : MonoBehaviour
{
     #region Members

    #region Sounds members
    [SerializeField] private AudioSource[] pieds = new AudioSource[2]; // Source pour les pruits de sauts
    [SerializeField] private AudioSource personnage;
    #endregion Sounds members

    private float forwardSpeed = 4.2F;
    private float backwardSpeed = 2.8F;
    private float sideSpeed = 2.15F; //Speeds
    //private float strafeSpeed; //We'll be able to strafe fast. => WIP (2.88 KMH).
    private float runMultiplier = 1.6F; //If the player wants to run, his forward speed will be multiplicated by 1.6

    private float jumpForce = 4.5F; //Force of the jump that the character will have
    private float groundGravity = 0.1F;
    private float defaultGravity = 12F;

    private float normalCrouchDeltaH = 0.6F;
    private float onTheKneesCrouchDeltaH = 0.35F; //The height the character will lose while crouching

    private bool characterCanJump; //Useful for the jump move
    private static float leanAngle = 7F;

    [SerializeField] private Rigidbody playerRigidbody;
    [SerializeField] private CapsuleCollider playerCollider; //Getting thos components via editor
    [SerializeField] private GameObject groundDetection;
    //List<string> animParametersList; //New list w/ names of booleans to handle animations

    FatigueSys fatigue;
    RaycastHit groundRay;
    
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
    /// <summary>Was character on the ground during last frame ?</summary>
    public bool characterIsGrounded { get; private set; } //Working

    //Those allows to get player's exhaust and allows creating percentage w/ Max. exhaust (designed for UI) :

    public float playerExhaust { get { return this.fatigue.fatigue; } }
    public float playerMaxExhaust { get { return this.fatigue.maxFatigue; } }

    public bool characterMovingFwd { get; private set; }
    public bool characterMovingBwd { get; private set; }
    public bool charactermovingLeft { get; private set; }
    public bool characterMovingRight { get; private set; }

    public bool wantsToRun { get; private set; } //To know is the character wants to run

    #endregion

    #region Start and Update

    // Use this for initialization
    void Start()
    { 
        this.fatigue = new FatigueSys(); //Instanciating new exhaust system (Class by JS)
        this.groundRay = new RaycastHit();

        this.InitState(); //Initializing character state (Setting booleans correclty before starting playing)

        //this.animParametersList = AnimatorHandling.GetParameterNames(this.anim);
        //We send our animator to get a whole list of the animator's parameters. This will allow us to disable all the bools we don't need in only one line !
    }

    private void Update()
    {
        #region Jump

        //Checking for Jump :
        //If player wants to jump AND that character can jump AND that character isn't crouched :
        if (Input.GetButtonDown("Jump") && this.characterIsGrounded && !(characterIsCrouched))
        {
            Jump(); //Makes the character jump
            characterIsJumping = true; //Setting the property for other scripts
        }
        else characterIsJumping = false; //Setting property for other scripts

        #endregion
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!UIScript.gameIsPaused)
        {
            #region Ground Move

            if (characterIsGrounded) this.Move();

            #endregion

            #region Crouch

            if (Input.GetKeyDown(KeyCode.C)) Crouch(this.normalCrouchDeltaH);
            //Doing two != types of crouching depending on the key pressed
            if (Input.GetKeyDown(KeyCode.B)) Crouch(this.onTheKneesCrouchDeltaH);

            #endregion

            #region Ground Detection

            this.characterIsGrounded = this.GetIfCharacterIsGrounded();


            #endregion

            #region Exhaust

            DetermineExhaust();
            //That method I created will call the good method in FatigueSys class to in/de-crease exhaust in function of what the character is doing

            #endregion

            #region Leaning

            //Leaning();

            #endregion

            Gravity();
        }
    }

    #endregion

    #region Moving Methods

    private void Gravity()
    {
        float gravity = 0F;

        if (characterIsGrounded) gravity = groundGravity;
        else gravity = defaultGravity;

        Vector3 forceToApply = new Vector3(0F, -gravity, 0F);
        playerRigidbody.AddRelativeForce(forceToApply, ForceMode.Acceleration);
    }

    private void Jump()
    {
        #region sound
        Sounds.Jump(personnage);
        #endregion sound

        Vector3 jump = new Vector3(0F, jumpForce, 0F); //Making the jump by setting the velocity to the jump force
        //playerRigidbody.velocity += jump;
        playerRigidbody.AddForce(jump, ForceMode.Impulse);
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
            wantsToRun = Input.GetKey(KeyCode.LeftShift); //Checking if player pressed Lshift (means that he wants to run)

            if (targetSpeed.z > 0)
            {
                targetSpeed.z *= forwardSpeed;
                this.characterMovingFwd = true;
                this.characterMovingBwd = false;

            }//If going forward multiplying by forward speed
            else
            {
                targetSpeed.z *= backwardSpeed;
                this.characterMovingBwd = true;
                this.characterMovingFwd = false;
            }//If going backwards, multiplying by backwards speed that is lower than forward one

            targetSpeed.x *= sideSpeed; //Assigning speeds to each component of the moving Vector
            if(targetSpeed.x > 0)
            {
                this.characterMovingRight = true;
                this.charactermovingLeft = false;
            }
            else
            {
                this.charactermovingLeft = true;
                this.characterMovingRight = false;
            }

            targetSpeed = transform.TransformDirection(targetSpeed); //Doing a transformDirection to be able to turn the axes
            //Have to keep this after applying speeds so that we don't have speeds/direction problems

            if (wantsToRun && this.fatigue.isAbleToRun()) //If player wants to run, we increase movement speed by a number that'll change depending on exhaust
            {
                targetSpeed.x *= runMultiplier;
                targetSpeed.z *= runMultiplier;


                this.SetStateRun();

                #region sound
                Sounds.Marche(pieds, !this.characterCanJump, true);
                #endregion sound
            }
            else if (wantsToRun) this.SetStateWalk(true);
            else this.SetStateWalk(false);

            Vector3 deltaMove = targetSpeed - currentVelocity;

            //Not doing the difference between actual velocity and new one would provoke a kind of acceleration which we don't want

            playerRigidbody.AddForce(deltaMove, ForceMode.VelocityChange); //Applying that force to the player. Multiplying by 50 (float) to get something strong enough.

            #region sound
            if (!wantsToRun) Sounds.Marche(pieds, !this.characterCanJump, false);
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

    private void Leaning()
    {
        Vector3 actualRot = playerRigidbody.transform.localEulerAngles;

        if (Input.GetKey(KeyCode.E)) actualRot.z = -leanAngle;
        else if (Input.GetKey(KeyCode.A)) actualRot.z = leanAngle;
        else actualRot.z = 0F;

        playerRigidbody.transform.localEulerAngles = actualRot;
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

    private bool GetIfCharacterIsGrounded()
    {
        //If the player is on the ground 
        if (Physics.Raycast(this.groundDetection.transform.position, this.groundDetection.transform.forward, out this.groundRay, .5F))
        {
            if (this.groundRay.collider.tag == "Ground") return true;
        }

        return false;
    }

    private void SetStateRun()
    {
        this.characterIsMoving = true;
        this.characterIsRunning = true;

        this.characterIsWalking = false;
    }

    private void SetStateWalk(bool triesToRun)
    {
        this.characterIsMoving = true;
        this.characterIsWalking = true;

        this.characterIsRunning = false;
        
        if (triesToRun) wantsToRun = true;
        else wantsToRun = false;
    }

    private void SetStateIdle()
    {
        this.characterIsRunning = false;
        this.characterIsWalking = false;
        this.characterIsMoving = false;

        this.characterMovingBwd = false;
        this.characterMovingFwd = false;
        this.characterMovingRight = false;
        this.charactermovingLeft = false;
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

    private void DetermineExhaust()
    {
        if (this.characterIsJumping) fatigue.Jumping();

        if (characterIsCrouched)
        {
            if (characterIsMoving) fatigue.crouchWalking();
            else fatigue.Crouched();
        }
        else
        {
            if (characterIsRunning) fatigue.Running();
            else if (characterIsWalking && wantsToRun) fatigue.exhaustRun();
            else if (characterIsWalking) fatigue.Walking();
            else fatigue.Idle();
        }
    }

    #endregion

}
