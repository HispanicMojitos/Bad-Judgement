using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIscripts : MonoBehaviour
{

    #region membres
    [SerializeField] private CapsuleCollider capsColKnneel;
    [SerializeField] private CapsuleCollider capsColStand;
    [SerializeField] private GameObject lanceurGrenad;
    [SerializeField] private GameObject M4A8; // prend la position du M4A8
    [SerializeField] private GameObject grenade;
    [SerializeField] private GameObject boucheCanon;
    [SerializeField] private GameObject[] pointDeCouverture;
    [SerializeField] private GameObject[] pointDePatrouille; // Recupere l'ensemble des points de patrouille que l'on veuille mettre a l'IA
    [SerializeField] private AudioClip[] soundDeath;
    [SerializeField] private AudioSource mouthHead;
    [SerializeField] private AudioSource M4A8Source; // Recupere la source des son du M4A8
    [SerializeField] private AudioClip M4A8shoot;// Recupere le son du M4A8
    [SerializeField] private AudioClip grenadScream;
    [SerializeField] private Transform chercheurCouverture;
    [SerializeField] private Transform hand;
    public Transform Player; // Nous permet de comparer le joueur a l'intéligence artificelle
    [SerializeField] private Transform head; // Permet de regler les angles de vue par rapport a la tête
    [SerializeField] private Rigidbody rbPlayer;
    [SerializeField] private static Animator anim; // Récupere les animations de l'IA, on met en static, cela permet de dupliquer l'IA avec ctr+D dans l'editeur de scene 
    private Target IA; // permet de récuperer le script Target attaché a l'IA auquel on attache ce script
    [SerializeField] private Transform[] alliés;


    [SerializeField] [Range(0f, 1f)] private float cadenceDetir = 0.1f; // plus cadenceDetir est faible, plus l'IA va tirer rapidement
    private float degats = 1f; // Permet de regler les degats de l'IA
    [SerializeField] private float vitesseRotation = 0.2f; // Vitesse de rotation de l'IA
    [SerializeField] [Range(0f, 10f)] private float vitesseMarche = 1.5f; // Vitesse de marche
    [SerializeField] [Range(0f, 10f)] private float vitesseCourse = 4f; // Vitesse de marche
    private float tailleZonePointDepatrouille = 1f; // Taille des points de patrouille par lesquelle l'IA va prendre la route du prochain point de patrouille
    private float vie = 0f; //On initialise la vie de l'IA
    private float vieMax; // Recupere la vie max de l'IA pour des comparaison

    private float tempsPause = 5f; // Durée de la pause
    private float tempsDeTir = 0f; // TOUCHE PAS A CA PTIT CON = la valeur 0 doit etre absolument initialisée pour permettre a l'IA de tirer
    private float tempsNouvelleDecision = 0f;
    private float tempsDebutAttaque = 0f;
    private float tempsFinAttaque = 0f;
    private float tempsKneelDecision = 0f;
    private float tempsAvantArreterPoursuite = 0f;
    private float tempsAvantAttaque = 0f;
    private float tempsAvantSeredresser = 0f;
    private float tempsAvantDelayCoupDeCrosse = 0f;
    private float tempsActionGrenade = 0f;
    private float tempsScreamgrenade = 0f;
    private float tempsAvantSeCouvrir = 0f;
    private float tempsAvantAcroupir = 0f;
    private float difficulteTempsReprendRonde = 10;
    private float delayAvntSeCouvrir = 1f;

    private int actuelleCible;
    private int nbreAlliés;
    private int[] PdPprocheDePdC; // Valeur entre [] => POINT DE CONTROLEE, valeur tout cours : POINT DE PATRUILLE le plus proche au point de controlle correspondant
    private int actuelPointDePatrouille = 0; // retourne le point actuel de patrouille
    private int angleDevueMax = 20; // Angle de vue maximum de l'IA
    private float distanceDeVueMax = 100f; // Distance entre l'IA et le joueur a partir de laquelle l'IA va commencer a suivre le joueur
    // A METTRE EN MODE FACILE private int distanceAttaque = 30;// Distance entre l'IA et le joueur a partir de laquelle l'IA va commencer a attaquer
    private int tempsGrenadeChoix = 4;

    [HideInInspector] public bool canSeePlayer = true;
    private bool estAGenoux = false;
    [HideInInspector] public bool estMort = false;
    private bool aJeteGrenade = false;
    private bool isThrowingGrenade = false;
    private bool isblocking = false;
    private bool playSoundOnce = false;
    private bool[] volonté;
    private bool changeDirection = false;
    private bool saitOuEstLeJoueur = false;
    private bool isAimingPlayer = false;
    [HideInInspector] public bool chercheCouverture = false;
    private bool wantToAttack = false;
    private bool searchCover = false;
    [HideInInspector] public bool estCouvert = false;
    private bool reversePatrouille = false; // Permet de savoir dans quel sens de la patrouille l'IA est
    private bool IsPausing = false; // reflete si l'IA doit prendre une pause
    [HideInInspector]  public bool IsPatrolling = true; // Permet de savoir quand l'enemi poursuit l'iA 

    [HideInInspector]public bool reprendLaRonde = false;
    #endregion membres

    #region membres pour difficultes

    int nbrDePause = 3; // Plus on augmonte ce nombre, plus l'IA fera de pause

    #endregion membres pour difficultes

    #region Awake Start & Update

    #region Awake & Start
    void Awake()
    {
        M4A8.transform.SetParent(hand); // On positionne l'arme dans la main de L'IA et on la maintient dans des rotation est position convenable des le début
        M4A8.transform.localPosition = new Vector3(0.287f, -0.046f, 0.008f);
        M4A8.transform.localRotation = Quaternion.Euler(-18.928f, -95.132f, 85.29f);
    }

    void Start()
    {
        actuelleCible = 0;
        nbreAlliés = alliés.Length;
        anim = GetComponent<Animator>(); // On récupere les animations dés que le jeux commence
        IA = GetComponent<Target>(); // On récupere les donnée du script Target attaché a la même IA que Ce script-ci
        vie = IA.vie; // On recupere la vie de l'IA via le script Target 
        vieMax = IA.vie; // On recupere la vie dans une valeur tempons de comparaison

        tempsDebutAttaque = UnityEngine.Random.Range(1f, 0.5f);
        tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
        tempsAvantSeredresser = UnityEngine.Random.Range(2f, 8f);

        PdPprocheDePdC = new int[pointDeCouverture.Length];
        volonté = new bool[5]; // On initialise la volonté de l'IA

        mouthHead.clip = soundDeath[0];

        DeterminePointDePatrouilleProchePointDeCouverture(); // Permet de faire prendre connaissance de lIA des point de couverture les plus proches en fonction des points de patrouille
        VolonteEtat(); // On initialise la vonlonté de l'IA
    }
# endregion Awake & Start

    void Update()
    {
        if (OptionsMenu.changeDifficultée == true)
            ChangementDeDifficulté();

        if (IA.vie > 0)
        {
            Vector3 direction = Player.position - this.transform.position; // Ici on retourne le rapport de la direction du joueur par rapport a l' IA au niveau de la position de ceux ci dans l'espace virtuel du jeux
            VolonteEtat();
            direction.y = 0; // evite que l'IA marche dans le vide lorsqu'on saute
            float angle = Vector3.Angle(direction, head.forward); // Permet de retourner un angle en comparant la position de la tête de l'IA avec celle du joueur

            Debug.DrawLine(transform.position, direction * 100, Color.blue);

            if ( (IsPatrolling && pointDePatrouille.Length > 0 || chercheCouverture == true) || reprendLaRonde == true) // Si l'IA patrouille ET qu'il existe des point de patrouille pou lui patrouiller, alors  son algorithme se met en place (évite les erreurs)
            {
                if (chercheCouverture == true)
                {
                    RaycastHit k; // On utilise un raycast pour voir si l'IA voit le joueurs
                    if (Physics.Raycast(chercheurCouverture.transform.position, (pointDeCouverture[CherchePointDeCouvertureProche()].transform.position - chercheurCouverture.transform.position), out k) && k.transform.CompareTag("cover"))
                    {
                        searchCover = true;
                    }
                    Debug.DrawLine(chercheurCouverture.transform.position, (pointDeCouverture[CherchePointDeCouvertureProche()].transform.position - chercheurCouverture.transform.position) * 50, Color.green);
                }

                if (IA.vie == vie ) // Si l'IA n'est pas attaquée
                {
                    if ((!IsPausing) && chercheCouverture == false) SetAnimation(isWalking:true);// si il n'a pas a faire de pause, il continue son bonhome de chemin
                    else if (chercheCouverture == true && estCouvert == false) SetAnimation(isRunning:true);
                    else if (estCouvert == true) SetAnimation(isKneel: true);

                    if (((actuelPointDePatrouille % nbrDePause == 0) && tempsPause >= 0) && chercheCouverture == false) // Permet de ne faire la pause qu'a un point de patrouille donné
                    {
                        SetAnimation(isIdle:true);
                        IsPausing = true; // l'IA prens sa pause
                        tempsPause = tempsPause - Time.deltaTime; // Malheuresement le temps d'une pause ne dure jamais longtemps !! (Bref le temps de la pause diminue)
                    }
                    else IsPausing = false;

                    if (!(actuelPointDePatrouille % nbrDePause == 0)) tempsPause = UnityEngine.Random.Range(5f, 15f); // Permet de remettre la pause a son stade initial 

                    if ((Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, transform.position) < tailleZonePointDepatrouille && !IsPausing) && chercheCouverture == false) // On verifie la distance entre le point de patrouille actuel et l'IA
                    {
                        if (reversePatrouille == false) actuelPointDePatrouille++; // On a atteint notre point de patrouille, on passe au suivant !
                        else actuelPointDePatrouille--; // On a atteint notre point de patrouille, on passe au suivant ! sens inverse de la patrouille

                        if (actuelPointDePatrouille >= pointDePatrouille.Length) // permet de faire la boucle de pause lorsqu'on arrive au point 5
                        {
                            actuelPointDePatrouille -= 2;
                            reversePatrouille = true;
                        }
                        else if (actuelPointDePatrouille < 0) // Permet de faire la boucle de pause lorsqu'on arrive au point 0
                        {
                            reversePatrouille = false;
                            actuelPointDePatrouille += 2;
                        }
                    }
                    else if (((Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, this.transform.position) < tailleZonePointDepatrouille) && chercheCouverture == true && searchCover == false) || changeDirection == true && estCouvert == false)
                    {
                        if (actuelPointDePatrouille == PdPprocheDePdC[CherchePointDeCouvertureProche()] && Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, this.transform.position) < tailleZonePointDepatrouille) searchCover = true;
                        else if (actuelPointDePatrouille > PdPprocheDePdC[CherchePointDeCouvertureProche()]) { if (!searchCover) actuelPointDePatrouille--; }
                        else if (actuelPointDePatrouille < PdPprocheDePdC[CherchePointDeCouvertureProche()]) { if (!searchCover) actuelPointDePatrouille++; }
                        changeDirection = false;
                    }// Ici on rafraichi les points de patrouille vers lequel doit se diriger l'IA, ou si il doit aller se couvrir


                    if (!IsPausing && chercheCouverture == false && searchCover == false)
                    {
                        direction = pointDePatrouille[actuelPointDePatrouille].transform.position - transform.position; // Permet d'ajuster la direction que doit prendre l'IA a chaque frame, Notemment ici l'IA prend la direction du point actuel de patrouille qu'elle doit rejoindre
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime);// L'ia tourne en direction du point de patrouille actuel pour pouvoir se diriger ver celui ci
                        this.transform.Translate(0, 0, Time.deltaTime * vitesseMarche); // Donne une certaine vitesse a l'IA lorsqu'il marche
                    }
                    else if (IsPausing == false && chercheCouverture == true && estCouvert == false && Vector3.Distance(pointDeCouverture[CherchePointDeCouvertureProche()].transform.position, this.transform.position) > 0.5f)
                    {
                        if (!searchCover) direction = pointDePatrouille[actuelPointDePatrouille].transform.position - transform.position; // Permet d'ajuster la direction que doit prendre l'IA a chaque frame, Notemment ici l'IA prend la direction du point actuel de patrouille qu'elle doit rejoindre
                        else if (searchCover) direction = pointDeCouverture[CherchePointDeCouvertureProche()].transform.position - transform.position;

                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime); // L'ia tourne en direction du point de patrouille actuel pour pouvoir se diriger ver celui ci
                        this.transform.Translate(0, 0, Time.deltaTime * vitesseCourse); // Donne une certaine vitesse a l'IA lorsqu'il marche
                    }
                    if (Vector3.Distance(pointDeCouverture[CherchePointDeCouvertureProche()].transform.position, this.transform.position) <= 0.5f)
                    {
                        chercheCouverture = false;
                        if (wantToAttack == false) isAimingPlayer = false;
                        estCouvert = true;
                        SetAnimation(isKneel: true);
                    }
                }
                else
                {
                    IsPatrolling = false; // Si le joueur attaque l'IA, l'IA va venir l'attaquer
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                    if (Vector3.Distance(pointDeCouverture[CherchePointDeCouvertureProche()].transform.position, this.transform.position) <= 0.5f)
                    {
                        chercheCouverture = false;
                        if (wantToAttack == false) isAimingPlayer = false;
                        estCouvert = true;
                        SetAnimation(isKneel: true);
                    }
                    else if (Vector3.Distance(pointDeCouverture[CherchePointDeCouvertureProche()].transform.position, this.transform.position) <= 1f)
                    {
                        chercheCouverture = false;
                        if (wantToAttack == false) isAimingPlayer = false;
                        estCouvert = true;
                        SetAnimation(isKneel: true);
                    }

                    RaycastHit h;
                    if (Physics.Raycast(head.transform.position, alliés[0].position - this.transform.position, out h) && h.transform.position != alliés[0].position) // Si le joueur ne peut pas etre dans la ligne de mire de l'IA lorsqu'elle est attaquée
                    {
                        foreach (Transform a in alliés)
                        {
                            if (a.GetComponent<AIally>() != null && a.GetComponent<AIally>().estHS == false)
                            {
                                Player = a;
                                if (Physics.Raycast(head.transform.position, Player.position - this.transform.position, out h) && h.transform.position == Player.position) break;
                            }
                        }
                    }
                    else Player = alliés[0];
                }
            }

            Debug.DrawLine(this.transform.position, direction * 100, Color.yellow);

            Debug.DrawLine(head.transform.position, (Player.position - this.transform.position)*100, Color.magenta);

            if(alliés[0] != Player) // Si l'IA a tué l'un des alliés du joueur, elle va directement se concentrer sur le joueur si possible
            {
                if (Player.GetComponent<AIally>().estHS == true) Player = alliés[0]; // alliés[0] reresente la valeur du comosant Transform du joueur
            }

            if (Vector3.Distance(Player.position, this.transform.position) <= 1.5f && tempsAvantDelayCoupDeCrosse < 0.5f) // Si le joueur se trouve trop pres de l'IA il va l'attaquer au corp a corps !! (DU CATCH !!)
            {
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f); // On garde le fait que l'IA regarde vers le joueur
                SetAnimation(isAttackingCloser:true);
                isblocking = true;
                if (tempsAvantDelayCoupDeCrosse > 0.1f)
                {
                    Target joueur = Player.transform.GetComponent<Target>();
                    joueur.TakeDamage(3);
                    rbPlayer.AddForce(direction * 2.5f, ForceMode.Impulse); // Permet de faire reculer le joueur
                    tempsAvantDelayCoupDeCrosse = 0;
                }
                if (tempsAvantDelayCoupDeCrosse == 0)
                {
                    M4A8.transform.localPosition = new Vector3(0.023f, -0.04f, -0.365f);
                    M4A8.transform.localRotation = Quaternion.Euler(-169.489f, 176.729f, 80.002f); // Permet de changer la position de l'arme le temps de l'animation
                }
                tempsAvantDelayCoupDeCrosse += Time.deltaTime;
            }
            else if (isblocking == true) // Si l'IA arrete de bloquer, la position de l'arme va changer
            {
                M4A8.transform.localPosition = new Vector3(0.287f, -0.046f, 0.008f);
                M4A8.transform.localRotation = Quaternion.Euler(-18.928f, -95.132f, 85.29f);
                tempsAvantDelayCoupDeCrosse = 0;
                isblocking = false;
            }
            else if ((((Vector3.Distance(Player.position, this.transform.position) < 100 ) && (angle < angleDevueMax || IsPatrolling == false)) || saitOuEstLeJoueur) && chercheCouverture == false && estCouvert == false)
            {// Si la distance entre le joueur  ET l'IA auquel on attache ce script est inférieur à la distance de vue max, ET que le joueur se trouve dans la région de l'espace situé dans l'angle de vue défini de l'IAalors on va faire quelquechose

                EtatCiblage();

                tempsNouvelleDecision += Time.deltaTime; // Le temps avant une nouvelle décision de l'IA augmonte
                RaycastHit h;
                if (Physics.Raycast(head.transform.position, Player.position - this.transform.position, out h) && h.transform.position == Player.position)
                { // Si l4IA voit bien le joueur dans sa ligne de mire
                    if (saitOuEstLeJoueur == false)
                    {
                        saitOuEstLeJoueur = true;
                        alliés[0].GetComponent<PlayerHealth>().estRepere = true; // alliés[0] etant le Joueur principal
                    }
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f); // L'IA se tourne ver le joueur
                    IsPatrolling = false;

                    tempsAvantSeCouvrir += Time.deltaTime;
                    if (tempsAvantSeCouvrir > UnityEngine.Random.Range(4, 10))
                    {
                        chercheCouverture = true;
                        changeDirection = true;
                    }

                    isAimingPlayer = true;
                    SetAnimation(isAiming:true);
                    if ((tempsAvantAttaque > tempsDebutAttaque) && (tempsAvantAttaque <= tempsFinAttaque)) //Permet de faire tirer des rafales a l'IA
                    {
                        if (tempsDeTir > cadenceDetir) // permet de cadancer les tirs de l'IA
                        {
                            SetAnimation(isAttack:true);
                            AttackShoot(direction); // Permet de faire attaquer l'IA
                            tempsDeTir = 0;
                        }
                        else tempsDeTir += Time.deltaTime; // le temps a attendre pour que l'IA pousse effectuer un autre tir augmonte
                    }
                    else if (tempsAvantAttaque > 1.2f)
                    {
                        tempsAvantAttaque = 0;
                        tempsDebutAttaque = UnityEngine.Random.Range(0.3f, 0.7f);
                        tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
                    }
                    tempsAvantAttaque += Time.deltaTime; // Incréméente le temps avant la prochaine rafale de balle
                }
                else if (isAimingPlayer == true)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                    SetAnimation(isAiming: true);
                    if (tempsNouvelleDecision > UnityEngine.Random.Range(2f, 10f)) // Permet de que l'IA prennent une nouvelle décision lorsque le temps de celui ci est dépassé
                    {
                        chercheCouverture = volonté[UnityEngine.Random.Range(0, 4)]; // ici l'IA va décider de chercher une couverture ou non en fonction de sa volonté
                        tempsNouvelleDecision = 0; //
                        changeDirection = true;
                    }
                    if (tempsAvantArreterPoursuite > difficulteTempsReprendRonde)
                    {
                        StopPoursuite();
                        Player.GetComponent<PlayerHealth>().estRepere = false;
                    } // arrete la poursuite de l'IA envers le joueur lorsque celui ci ne le voit plus pendant tout un temps
                    else tempsAvantArreterPoursuite += Time.deltaTime;
                }
            }
            else if (IsPatrolling == false && estCouvert == false)
            {
                IsPatrolling = true;
                vie = IA.vie;
                saitOuEstLeJoueur = false;
                estCouvert = false;
            }
            else if (estCouvert == true) // Ici on a les differentes actions pour lesquelle l'IA est couvert
            {
                EtatCiblage();

                tempsNouvelleDecision += Time.deltaTime;

                if (isAimingPlayer == false) SetAnimation(isKneel: true);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                float random = UnityEngine.Random.Range(5f, 15f);
                if (tempsNouvelleDecision > random)
                {
                    tempsNouvelleDecision = 0;
                    int r = UnityEngine.Random.Range(0, 4);
                    isAimingPlayer = volonté[r];
                    wantToAttack = true;
                }
                if (vie == IA.vie)
                {
                    tempsAvantAcroupir = 0;
                    RaycastHit h; // On utilise un raycast pour voir si l'IA voit le joueurs

                    if (Physics.Raycast(head.transform.position - new Vector3(0f, 0.75f, 0f), Player.position - this.transform.position, out h) && h.transform.position == Player.position && isThrowingGrenade == false )
                    {
                        SetAnimation(isAimKneel:true);
                        if ((tempsAvantAttaque > tempsDebutAttaque) && (tempsAvantAttaque <= tempsFinAttaque)) //Permet de faire tirer des rafales a l'IA
                        {
                            if (tempsDeTir > cadenceDetir) // permet de cadancer les tirs de l'IA
                            {
                                AttackShoot(direction); // Permet de faire attaquer l'IA
                                tempsDeTir = 0;
                            }
                            else tempsDeTir += Time.deltaTime; // le temps a attendre pour que l'IA pousse effectuer un autre tir augmonte
                        }
                        else if (tempsAvantAttaque > 1.2f)
                        {
                            tempsAvantAttaque = 0;
                            tempsDebutAttaque = UnityEngine.Random.Range(0.3f, 0.7f);
                            tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
                        }
                    }
                    else if (isAimingPlayer == true && isThrowingGrenade == false)
                    {
                        if (Physics.Raycast(head.transform.position, alliés[0].position - this.transform.position, out h) && h.transform.position == alliés[0].position) Player = alliés[0]; // ermet de changer de cible vers le joueur principal
                        SetAnimation(isAiming: true);
                        if (Physics.Raycast(head.transform.position, Player.position - this.transform.position, out h) && h.transform.position == Player.position)
                        {
                            if ((tempsAvantAttaque > tempsDebutAttaque) && (tempsAvantAttaque <= tempsFinAttaque))
                            {
                                if (tempsDeTir > cadenceDetir) // permet de cadancer les tirs de l'IA
                                {
                                    AttackShoot(direction); // Permet de faire attaquer l'IA
                                    tempsDeTir = 0;
                                }
                                else tempsDeTir += Time.deltaTime; // le temps a attendre pour que l'IA pousse effectuer un autre tir augmonte
                            }
                            else if (tempsAvantAttaque > 1.2f)
                            {
                                tempsAvantAttaque = 0;
                                tempsDebutAttaque = UnityEngine.Random.Range(0.3f, 0.7f);
                                tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
                            }
                        }
                        if (tempsAvantArreterPoursuite > difficulteTempsReprendRonde)
                        {
                            StopPoursuite();
                            Player.GetComponent<PlayerHealth>().estRepere = false;
                        }
                        else tempsAvantArreterPoursuite += Time.deltaTime;
                    }
                    else isAimingPlayer = false;
                }
                else if (vie != IA.vie && isThrowingGrenade == false) // Joue l'animation est la reflexions lorsque l'IA est a genoux
                {
                    RaycastHit h;
                    if (Physics.Raycast(head.transform.position, Player.position - this.transform.position, out h) && h.transform.position == Player.position && isAimingPlayer == true)
                    {
                        SetAnimation(isAiming: true);
                        if ((tempsAvantAttaque > tempsDebutAttaque) && (tempsAvantAttaque <= tempsFinAttaque))
                        {
                            if (tempsDeTir > cadenceDetir) // permet de cadancer les tirs de l'IA
                            {
                                AttackShoot(direction); // Permet de faire attaquer l'IA
                                tempsDeTir = 0;
                            }
                            else tempsDeTir += Time.deltaTime; // le temps a attendre pour que l'IA pousse effectuer un autre tir augmonte
                        }
                        else if (tempsAvantAttaque > 1.2f)
                        {
                            tempsAvantAttaque = 0;
                            tempsDebutAttaque = UnityEngine.Random.Range(0.3f, 0.7f);
                            tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
                        }
                    }
                    tempsAvantAcroupir += Time.deltaTime;
                    if (tempsAvantAcroupir > delayAvntSeCouvrir)
                    {
                        if (Physics.Raycast(head.transform.position - new Vector3(0f, 0.75f, 0f), Player.position - this.transform.position, out h) && h.transform.position == Player.position)
                        {
                            SetAnimation(isAimKneel:true);
                            vie = IA.vie; // Si l'enemi est attaqué lorsqu'il est acroupi, il peut attaquer en même temps
                            if (Physics.Raycast(head.transform.position - new Vector3(0f, 0.75f, 0f), Player.position - this.transform.position, out h) && h.transform.position == Player.position && isThrowingGrenade == false & isAimingPlayer == false)
                            {
                                SetAnimation(isAimKneel: true);
                                if ((tempsAvantAttaque > tempsDebutAttaque) && (tempsAvantAttaque <= tempsFinAttaque)) //Permet de faire tirer des rafales a l'IA
                                {
                                    if (tempsDeTir > cadenceDetir) // permet de cadancer les tirs de l'IA
                                    {
                                        AttackShoot(direction); // Permet de faire attaquer l'IA
                                        tempsDeTir = 0;
                                    }
                                    else tempsDeTir += Time.deltaTime; // le temps a attendre pour que l'IA pousse effectuer un autre tir augmonte
                                }
                                else if (tempsAvantAttaque > 1.2f)
                                {
                                    tempsAvantAttaque = 0;
                                    tempsDebutAttaque = UnityEngine.Random.Range(0.3f, 0.7f);
                                    tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
                                }
                            }
                        }
                        else
                        {
                            if (tempsAvantArreterPoursuite > difficulteTempsReprendRonde)
                            {
                                StopPoursuite();
                                Player.GetComponent<PlayerHealth>().estRepere = false;
                            }
                            else tempsAvantArreterPoursuite += Time.deltaTime;
                            SetAnimation(isKneel: true);
                            isAimingPlayer = false;
                            tempsKneelDecision += Time.deltaTime;
                            if (tempsKneelDecision > tempsAvantSeredresser)
                            {
                                tempsAvantSeredresser = UnityEngine.Random.Range(2f, 8f);
                                vie = IA.vie;
                                isAimingPlayer = true;
                                tempsNouvelleDecision = 0;
                                tempsKneelDecision = 0f;
                            }
                        }
                    }
                }
                tempsAvantAttaque += Time.deltaTime; // Incréméente le temps avant la prochaine rafale de balle

                if (aJeteGrenade == false)//  Partie dans laquelle l'IA se décide de jeter une grenade ounon 
                {
                    tempsKneelDecision += Time.deltaTime;
                    if (tempsKneelDecision > 5f)
                    {
                        tempsGrenadeChoix = UnityEngine.Random.Range(0, 4);
                        tempsKneelDecision = 0;
                    }
                    if (tempsGrenadeChoix == 1)
                    {
                        isThrowingGrenade = true;
                        SetAnimation(kneeGrenad: true);
                        tempsKneelDecision = 0;
                        tempsScreamgrenade += Time.deltaTime;
                        if (!mouthHead.isPlaying && tempsScreamgrenade < 1.3f)
                        {
                            mouthHead.clip = grenadScream;
                            mouthHead.Play();
                        }

                        if (aJeteGrenade == false) LanceGrenade(Vector3.Distance(Player.position, this.transform.position));
                    }
                }
                else isThrowingGrenade = false;
            }
        }
        else if (estMort == false) // Si l'IA meurt il faut jouer sa mort, faire en sorte que l'arme se perde, jouer le bruit de la mrt, etc...
        {
            SetAnimation(isDead: true);
            M4A8.GetComponent<Rigidbody>().isKinematic = false;
            M4A8.GetComponent<Rigidbody>().useGravity = true;
            M4A8.GetComponent<Rigidbody>().AddForce(Vector3.right * 0.2f, ForceMode.Impulse);
            M4A8.GetComponent<BoxCollider>().enabled = true;
            M4A8.transform.parent = null;
            capsColStand.enabled = false;
            capsColKnneel.enabled = false;
            Sounds.Death(mouthHead, soundDeath[0],playSoundOnce);
            estMort = true; // Permet d'éviter de se retrouver dans une boucle inutile
            
            Destroy(this.GetComponent<Target>());
            Destroy(this.GetComponent<AIscripts>());
        }
    }
    #endregion start & update

    

    #region method
    private void SetAnimation(bool isAimKneel = false  ,bool isKneel = false  ,bool kneeGrenad = false  ,bool isIdle = false  ,bool isAiming = false  ,bool isAttack = false  ,bool isWalking = false  ,bool isRunning = false  ,bool isAttackingCloser = false  ,bool isDead = false) // Utilisation de prametre nomé, a récuperer en argument nommer pour décider quelle animation sera jouée
    {
        if(isAimKneel == true || isKneel == true || kneeGrenad == true) etatDeboutOuGenoux(false);
        else etatDeboutOuGenoux(true);
        anim.SetBool("IsAimKneel", isAimKneel);
        anim.SetBool("IsKneel", isKneel);
        anim.SetBool("IsGrenade", kneeGrenad);
        anim.SetBool("IsIdle", isIdle);
        anim.SetBool("IsAiming", isAiming);
        anim.SetBool("IsAttacking", isAttack);
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("IsAttackingCloser", isAttackingCloser);
        anim.SetBool("IsDead", isDead);
        anim.SetBool("IsGettingUp", false);

    } // Cette methode permet ainsi de mettre en action l'animation souhaitée
    
    /// <summary> Permet, ar un jeux de raycast, de voir sir l'IA enemie l'enemie, et si as de viser un allié en etat d'attaque</summary>
    private void EtatCiblage()
    {
        RaycastHit h;
        if (Physics.Raycast(head.transform.position, alliés[0].position - this.transform.position, out h) && h.transform.position == alliés[0].position) Player = alliés[0]; // permet de faire en sorte que l'IA enemie se focus sur le joueur des qu'elle eut tirer dessus
        else if (Player == alliés[0])
        {
            foreach (Transform a in alliés) // Ici on va chercher un allié a ciblé pour l'IA enemie si elle ne peut pas voir le joueur
            {
                if (a.GetComponent<AIally>() != null && a.GetComponent<AIally>().estHS == false)
                {
                    if (Physics.Raycast(head.transform.position, a.position - this.transform.position, out h) && h.transform.position == a.position)
                    {
                        Player = a;
                        break;
                    }
                }
            }
        }
    }


    private void etatDeboutOuGenoux(bool doisEtreDebout) // Permet de regler l'etat de la box collider de l'énemy en fonction qu'il soit debout ou acroupi
    {
        if (estAGenoux == false && doisEtreDebout == false)
        {
            capsColStand.enabled = false;
            capsColKnneel.enabled = true;
            estAGenoux = true;
        }
        else if (estAGenoux == true && doisEtreDebout == true)
        {
            capsColKnneel.enabled = false;
            capsColStand.enabled = true;
            estAGenoux = false;
        }
    }

    private int précision = 5; // Plus cette valeur est grande, plus l'IA est précise dans ses tirs
    private void AttackShoot(Vector3 direction)
    {
        if (canSeePlayer == true)
        {
            RaycastHit hit; // permet de savoir si le raycaste se confronte a quelque chose

            if (Physics.Raycast(boucheCanon.transform.position, direction, out hit)) // si le raycast est bien en direction du joueur et qu'il le touche bien
            {
                float reelDegats;
                int i = UnityEngine.Random.Range(0,précision); // Perme

                if (i == 0) reelDegats = 0; // Permet de choisir le fait si ce tir la fera des degats ou pas au joueurs
                else reelDegats = degats;

                Target joueur = hit.transform.GetComponent<Target>(); // permet de recuperer le script de l'entité avec laquelle le 'hit' s'est rencontré

                if (joueur != null) // Si la cible du raycast a bien le script Target attaché
                    joueur.TakeDamage(reelDegats); // On fait subir des dommages au joueurs qui a le script Target attaché

                Sounds.AK47shoot(M4A8Source, M4A8shoot); // permet de jouer le son de tir 
            }
        }
    }
    

    private void LanceGrenade(float distanceBetween) // Permet de faire lancer une grenade a l'IA
    {
        SetAnimation(kneeGrenad:true);
        tempsActionGrenade += Time.deltaTime;
        float Force = 0f;
        if (tempsActionGrenade > 1.35f)
        {
            if (distanceBetween < 20) Force = 20;
            else if (distanceBetween < 30) Force = 28;
            else if (distanceBetween >= 30) Force = 30;

            tempsActionGrenade = 0;
            aJeteGrenade = true;
            GameObject clone = Instantiate(grenade, this.transform);
            Vector3 distance = this.transform.position - Player.transform.position;
            distance.y -= 15;
            clone.GetComponent<Rigidbody>().AddForce(-(distance) * (distance.magnitude / Force), ForceMode.Impulse);
            clone.transform.parent = null;
        }

    }

    private void StopPoursuite() // Permet d'arreter la pousuite de l'IA
    {
        IsPatrolling = true;
        vie = IA.vie;
        saitOuEstLeJoueur = false;
        estCouvert = false;
        chercheCouverture = false;
        reprendLaRonde = true;
        IsPausing = false;
        searchCover = false;
        isAimingPlayer = false;
        canSeePlayer = true;
        estAGenoux = false;
        changeDirection = false;
        wantToAttack = false;
        
        tempsAvantArreterPoursuite = 0;
    }
   
    private void DeterminePointDePatrouilleProchePointDeCouverture() // Permet de savoir quel point de patrouille est le plus proche de tel point de couverture
    {
        float pluspetiteDistance;
        for(int i = 0; i < pointDeCouverture.Length; i++) // Permet de verifier la distance de tout les point de patrouille pour chaque point de couverture
        {
            pluspetiteDistance = 1000000f;
            for (int j = 0; j < pointDePatrouille.Length; j++) // Permet de verifier la ditance de tout les points de patrouille existant
            {
                for(int k = 0; k < pointDePatrouille.Length; k++) // Permet de verifier la distance du point de patrouille séléctioneé
                {
                    if(   (Vector3.Distance(pointDeCouverture[i].transform.position, pointDePatrouille[j].transform.position) < Vector3.Distance(pointDeCouverture[i].transform.position, pointDePatrouille[k].transform.position))   && (Vector3.Distance(pointDeCouverture[i].transform.position, pointDePatrouille[j].transform.position) < pluspetiteDistance)  )
                    {
                        PdPprocheDePdC[i] = j;
                        pluspetiteDistance = Vector3.Distance(pointDeCouverture[i].transform.position, pointDePatrouille[j].transform.position);
                    }
                }
            }
        }
    }
    private int CherchePointDeCouvertureProche() // Permet de savoir quel point de couverture est le plus proche du joueur
    {
        int poinDeCouvertureProche = 0;
        for(int i = 0; i < PdPprocheDePdC.Length; i++)
        {
            for (int j = 0; j < PdPprocheDePdC.Length; j++)
            {
                if (Vector3.Distance(this.transform.position, pointDeCouverture[j].transform.position) < Vector3.Distance(this.transform.position, pointDeCouverture[i].transform.position) )
                {
                    poinDeCouvertureProche = j;
                }
            }
        }
        return poinDeCouvertureProche;
    }

    private void VolonteEtat()
    {
        byte choix = 0; // En fonction de la vie de l'IA, sa volonté changera et l'algorithme  dans la methode Update décidera des actions plus sécuritaire pour l'IA plus sa vie est basse

        if (IA.vie >= (vieMax * 80 / 100)) choix = 1;
        else if (IA.vie >= (vieMax * 50 / 100)) choix = 2;
        else if (IA.vie >= (vieMax * 35 / 100)) choix = 3;
        else if (IA.vie >= (vieMax * 20 / 100)) choix = 4;
        else choix = 5;

        Action<int> Volonte = new Action<int>((decision) => // PErmet d'avoir cette méthode rapidement dans cette autre méthoden en modifiant les valeurs en cas de besoin
      {
          switch (decision)
          {
              case 1: volonté[0] = true;  volonté[1] = true;  volonté[2] = true;  volonté[3] = true; volonté[4] = true; break;
              case 2: volonté[0] = false; volonté[1] = true;  volonté[2] = true;  volonté[3] = true; volonté[4] = true; break;
              case 3: volonté[0] = false; volonté[1] = false; volonté[2] = true;  volonté[3] = true; volonté[4] = true; break;
              case 4: volonté[0] = false; volonté[1] = false; volonté[2] = true;  volonté[3] = true; volonté[4] = true; break;
              case 5: volonté[0] = false; volonté[1] = false; volonté[2] = false; volonté[3] = true; volonté[4] = true; break;
              default:  break;
          }
      });

        Volonte(choix);
    }

    private void ChangementDeDifficulté()
    {
        switch(Difficulté.difficultyLevelIndex)
        {
            case 0: // BABY
                paramètreDifficultéIA(true,25,10,100,20,3,1,10);
                break;

            case 1: // EASY
                paramètreDifficultéIA(true,40,20,100,15,6,1,8f);
                break;

            case 2: // NORMAL
                paramètreDifficultéIA(false,60,30,100,10,8,1,5f);
                break;

            case 3: // Hard
                paramètreDifficultéIA(false, 90,40,100,7.5f,10,2,4f);
                break;

            case 4: // INFAMY
                paramètreDifficultéIA(false,100,50,100,5,15,3,2f);
                break;

            default: // Par defaut la difficulté sera mise sur Normal
                paramètreDifficultéIA(false, 60, 30, 100, 10, 8, 1, 1f);
                break;
        }
        OptionsMenu.changeDifficultée = false;
    }

    private void paramètreDifficultéIA(bool grenade, int angleVue, int distanceVue, float t_avantReprendreRonde, float t_pause, int precis, int dmg, float delayGenoux)
    {
        distanceDeVueMax = distanceVue;
        degats = dmg;
        précision = precis;
        aJeteGrenade = grenade;
        angleDevueMax = angleVue;
        difficulteTempsReprendRonde = t_avantReprendreRonde;
        tempsPause = t_pause;
        delayAvntSeCouvrir = delayGenoux;
    }
    #endregion method
}
