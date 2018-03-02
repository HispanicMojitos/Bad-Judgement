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
    private char tag = '\0'; // donne la valeur null du char, cette variable permet de faire en sorte dans l'algorithme de continuer un son de pas même lorsqu'on entre en collision avec un autre objet non tagué (exemple : un mur)

    #endregion Sounds members

    //Speeds :
    private float forwardSpeed = 4.2F;
    private float backwardSpeed;
    private float sideSpeed = 2.15F; 
    //private float strafeSpeed; //We'll be able to strafe fast. => WIP (2.88 KMH).

    //Jump :
    private float jumpForce = 4.5F; //Force of the jump that the character will have

    //Crouch :
    private float normalCrouchDeltaH = 0.6F;
    private float onKneesCrouchDeltaH = 0.32F; //The height 

    private bool characterCanJump; //Useful for the jump move

    //GameObjects :
    [SerializeField]private Rigidbody playerRigidbody;
    [SerializeField] private CapsuleCollider playerCollider; //We set them via edior.

    #endregion

    #region Properties

    public bool characterIsMoving { get; private set; }
    public bool characterIsJumping { get; private set; } //Properties accessible in readonly in other scripts

    public bool characterIsCrouched { get; private set; }

    #endregion

    #region Start and Update

    // Use this for initialization
    void Start()
    {
        backwardSpeed = (0.66F * forwardSpeed); //After real tests, reverse speed is 2/3 times of forward speed.

        this.characterIsCrouched = false;

        //To be moved later :
        Cursor.lockState = CursorLockMode.Locked;

        #region sounds
        personnage.volume = volumeDesSonsDePas; // Permet de reglez les sons de pas
        piedjumpPersonnage.volume = volumeDesSonsDePas; // permet de regler les son de jumps
        #endregion sounds
    }

    // Update is called once per frame
    void Update()
    {
        //Cursor will be moved later
        #region Cursor

        //To be moved :
        if (Input.GetKeyDown(KeyCode.Escape)) Cursor.lockState = CursorLockMode.None;
        if (Cursor.lockState == CursorLockMode.None && Input.GetKey(KeyCode.Mouse0)) Cursor.lockState = CursorLockMode.Locked;
        //Locks the cursor on the window

        #endregion

        #region Ground Moving

        //Checking for Ground Moving :
        float xAxis = Input.GetAxis("Horizontal") * sideSpeed * Time.deltaTime;
        float zAxis = Input.GetAxis("Vertical") * Time.deltaTime;

        if (zAxis != 0 || xAxis != 0) //If the player is moving :
        {
            Move(zAxis, xAxis); //We're moving on X and Z
            characterIsMoving = true; //If the player moves, then we say to the property to be true
        }
        else characterIsMoving = false; //If the player moves, then we say to the property to be true
                                        //Other ppl might want to use that property in other scripts

        #endregion

        #region Jump

        //Checking for Jump :

        if (Input.GetButtonDown("Jump") && this.characterCanJump) //If player wants to jump & that character can jump
        {
            Jump(); //Makes the character jump
            this.characterIsJumping = true; //Setting the property for other scripts
        }
        else this.characterIsJumping = false; //Setting property for other scripts

        #endregion

        #region Crouching

        if (Input.GetKeyDown(KeyCode.C)) Crouch(this.normalCrouchDeltaH);
        if (Input.GetKeyDown(KeyCode.B)) Crouch(this.onKneesCrouchDeltaH); 
        //Character crouches with a != height depending on the key pressed.
        //Animation will handle the 

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

    private void Move(float zAxis, float xAxis)
    {
        #region sound
        Sounds.FootSteepsSound(personnage); // Permet de jouer les sons de pas
        #endregion sound

        if (zAxis < 0) zAxis *= backwardSpeed;
        else zAxis *= forwardSpeed; //Forward speed != than backward speed

        Vector3 movement = new Vector3(xAxis, 0F, zAxis);
        //X is the strafe and Z is forward/backward

        this.transform.Translate(movement); //Making the move
    }

    private void Crouch(float deltaHeight)
    {
        if (!characterIsCrouched) deltaHeight *= -1.0F;
        //If the player is crouched, we will go higher, but if not, we'll descend by transforming
        //The + sign in a - by just putting "* (-1)" instead of "* 1"

        this.playerCollider.height += deltaHeight;
        this.playerCollider.center += new Vector3(0F, (deltaHeight / 2), 0F);

        this.characterIsCrouched = InvertBool(this.characterIsCrouched);
    }

    private bool InvertBool(bool toInvert) //Only for class members (no return)
    {
        if (toInvert) toInvert = false;
        else toInvert = true;

        return toInvert;
    }

    #endregion

    #region Sounds detecion area

    private void OnCollisionEnter(Collision collision) // Permet d'evaluer le son a jouer en fonction du type de sol rencontré /!\ On a besoin d'un rigibody et d'une box Colider /!\
    {
        this.characterCanJump = true;

        if ((collision.collider.CompareTag("Wood") || (tag == 'w' && !collision.collider.CompareTag("Wood"))) && !collision.collider.CompareTag("Street") && !collision.collider.CompareTag("Grass")) // OPTIMISATION : tag == "something"  allocates memory, CompareTag does not, i've changes this , sources : https://forum.unity.com/threads/making-stepssounds-by-using-oncollisionenter-or-raycasts-optimizations-question.518865/#post-3402025 ,https://answers.unity.com/questions/200820/is-comparetag-better-than-gameobjecttag-performanc.html
        {
            Sounds.DeclareSonDemarche(personnage, sonDePasSurBois, piedjumpPersonnage, jumpOnWood, tag, 'w');
        }
        else if ((collision.collider.CompareTag("Street") || (tag == 's' && !collision.collider.CompareTag("Street"))) && !collision.collider.CompareTag("Wood") && !collision.collider.CompareTag("Grass")) // Toute ces conditions permette de jouer le sons même en etant en colision avec d'autres objet non tagué (exemple : un mur, un ventilateur, ect...) 
        {
            Sounds.DeclareSonDemarche(personnage, sonDePasSurRue, piedjumpPersonnage, jumpOnStreet, tag, 's');
        }
        else if ((collision.collider.CompareTag("Grass") || (tag == 'g' && !collision.collider.CompareTag("Wood"))) && !collision.collider.CompareTag("Street") && !collision.collider.CompareTag("Wood"))
        {
            Sounds.DeclareSonDemarche(personnage, sonDePasSurterre, piedjumpPersonnage, jumpOnGrass, tag, 'g');
        }
    }

    #endregion Sounds detecion area

}
