using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIscripts : MonoBehaviour
{

    #region membres

    [SerializeField] private AudioClip[] soundDeath;
    [SerializeField] private AudioSource mouthHead;
    [SerializeField] private AudioSource M4A8Source; // Recupere la source des son du M4A8
    [SerializeField] private AudioClip M4A8shoot;// Recupere le son du M4A8
    [SerializeField] private Transform chercheurCouverture;
    [SerializeField] private Transform hand;
    [SerializeField] private Transform M4A8; // prend la position du M4A8
    [SerializeField] private Transform Player; // Nous permet de comparer le joueur a l'intélgience artificelle
    [SerializeField] private Transform head; // Permet de regler les angles de vue par rapport a la tête
    [SerializeField] private GameObject Projectile; // Recupere la forme des projéctile envoyé par le M4A8
    [SerializeField] private GameObject boucheCanon;
    [SerializeField] private GameObject[] pointDeCouverture;
    [SerializeField] private GameObject[] pointDePatrouille; // Recupere l'ensemble des points de patrouille que l'on veuille mettre a l'IA
    [SerializeField] private Rigidbody rbM4A8;
    [SerializeField] private BoxCollider bxM4A8;
    [SerializeField] private static Animator anim; // Récupere les animations de l'IA, on met en static, cela permet de dupliquer l'IA avec ctr+D dans l'editeur de scene 
    private Target IA; // permet de récuperer le script Target attaché a l'IA auquel on attache ce script

    [SerializeField] [Range(0f, 1f)] private float cadenceDetir = 0.1f; // plus cadenceDetir est faible, plus l'IA va tirer rapidement
    [SerializeField] [Range(0f, 10f)] private float degats = 1f; // Permet de regler les degats de l'IA
    [SerializeField] private float vitesseRotation = 0.2f; // Vitesse de rotation de l'IA
    [SerializeField] private float vitesse = 1.5f; // Vitesse de marche
    private float tailleZonePointDepatrouille = 1f; // Taille des points de patrouille par lesquelle l'IA va prendre la route du prochain point de patrouille
    private float vie = 0f; //On initialise la vie de l'IA
    private float vieMax; // Recupere la vie max de l'IA pour des comparaison
    private float tempsDeTir = 0f; // TOUCHE PAS A CA PTIT CON = la valeur 0 doit etre absolument initialisée pour permettre a l'IA de tirer
    private float tempsNouvelleDecision = 0f;
    private float tempsDebutAttaque = 0f;
    private float tempsFinAttaque = 0f;
    private float tempsPause = 5f; // Durée de la pause
    private float tempsKneelDecision = 0f;
    private float tempsAvantArreterPoursuite = 0f;
    private float tempsAvantAttaque = 0f;
    private float tempsAvantSeredresser = 0f;

    private int[] PdPprocheDePdC; // Valeur entre [] => POINT DE CONTROLEE, valeur tout cours : POINT DE PATRUILLE le plus proche au point de controlle correspondant
    private int actuelPointDePatrouille = 0; // retourne le point actuel de patrouille
    private int angleDevueMax = 60; // Angle de vue maximum de l'IA
    private int distanceDeVueMax = 50; // Distance entre l'IA et le joueur a partir de laquelle l'IA va commencer a suivre le joueur
    private int distanceAttaque = 30;// Distance entre l'IA et le joueur a partir de laquelle l'IA va commencer a attaquer


    private bool playSoundOnce = false;
    private bool[] volonté;
    private bool changeDirection = false;
    private bool saitOuEstLeJoueur = false;
    private bool isAimingPlayer = false;
    private bool chercheCouverture = false;
    private bool wantToAttack = false;
    private bool searchCover = false;
    private bool estCouvert = false;
    private bool reversePatrouille = false; // Permet de savoir dans quel sens de la patrouille l'IA est
    private bool IsPausing = false; // reflete si l'IA doit prendre une pause
    private bool IsPatrolling = true; // Permet de savoir quand l'enemi poursuit l'iA 
    #endregion membres

    #region Awake Start & Update

    #region Awake & Start
    void Awake()
    {
        M4A8.transform.SetParent(hand);
        M4A8.transform.localPosition = new Vector3(0.287f, -0.046f, 0.008f);
        M4A8.transform.localRotation = Quaternion.Euler(-18.928f, -95.132f, 85.29f);
    }

    void Start()
    {
        anim = GetComponent<Animator>(); // On récupere les animations dés que le jeux commence
        IA = GetComponent<Target>(); // On récupere les donnée du script Target attaché a la même IA que Ce script-ci
        vie = IA.vie; // On recupere la vie de l'IA via le script Target 
        vieMax = IA.vie;

        volonté = new bool[5];
        tempsDebutAttaque = UnityEngine.Random.Range(1f, 0.5f);
        tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
        PdPprocheDePdC = new int[pointDeCouverture.Length];
        tempsAvantSeredresser = UnityEngine.Random.Range(2f, 8f);

        mouthHead.clip = soundDeath[0];

        DeterminePointDePatrouilleProchePointDeCouverture();
        VolonteEtat();
    }
# endregion Awake & Start

    void Update()
    {

        if (IA.vie > 0)
        {
            Vector3 direction = Player.position - this.transform.position; // Ici on retourne le rapport de la direction du joueur par rapport a l' IA au niveau de la position de ceux ci dans l'espace virtuel du jeux
            //direction.y = 0; // evite que l'IA marche dans le vide lorsqu'on saute
            float angle = Vector3.Angle(direction, head.forward); // Permet de retourner un angle en comparant la position de la tête de l'IA avec celle du joueur

            Debug.DrawLine(transform.position, direction * 100, Color.blue);
            if (IsPatrolling && pointDePatrouille.Length > 0 || chercheCouverture == true) // Si l'IA patrouille ET qu'il existe des point de patrouille pou lui patrouiller, alors  son algorithme se met en place (évite les erreurs)
            {
                if (IA.vie == vie) // Si l'IA n'est pas attaquée
                {
                    if ((!IsPausing) && chercheCouverture == false) AnimWalk();// si il n'a pas a faire de pause, il continue son bonhome de chemin
                    else if (chercheCouverture == true && estCouvert == false) AnimRun();
                    else if (estCouvert == true) AnimKneel();

                    if (((actuelPointDePatrouille % 3 == 0) && tempsPause >= 0) && chercheCouverture == false) // Permet de ne faire la pause qu'a un point de patrouille donné
                    {
                        AnimIdle();
                        IsPausing = true; // l'IA prens sa pause
                        tempsPause = tempsPause - Time.deltaTime; // Malheuresement le temps d'une pause ne dure jamais longtemps !! (Bref le temps de la pause diminue)
                    }
                    else  IsPausing = false;

                    if (!(actuelPointDePatrouille %3 == 0)) tempsPause = UnityEngine.Random.Range(5f, 15f); // Permet de remettre la pause a son stade initial 

                    if ( (Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, transform.position) < tailleZonePointDepatrouille && !IsPausing) && chercheCouverture == false) // On verifie la distance entre le point de patrouille actuel et l'IA
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
                    else if (((Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, this.transform.position) < tailleZonePointDepatrouille) && chercheCouverture == true && searchCover == false) || changeDirection == true)
                    {
                        if (actuelPointDePatrouille == PdPprocheDePdC[CherchePointDeCouvertureProche()] && Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, this.transform.position) < tailleZonePointDepatrouille) searchCover = true;
                        else if (actuelPointDePatrouille > PdPprocheDePdC[CherchePointDeCouvertureProche()]) { if(!searchCover) actuelPointDePatrouille--; }
                        else if (actuelPointDePatrouille < PdPprocheDePdC[CherchePointDeCouvertureProche()]) { if(!searchCover) actuelPointDePatrouille++; }
                        changeDirection = false;
                    }
                    

                    if (!IsPausing && chercheCouverture == false && searchCover == false)
                    {
                        direction = pointDePatrouille[actuelPointDePatrouille].transform.position - transform.position; // Permet d'ajuster la direction que doit prendre l'IA a chaque frame, Notemment ici l'IA prend la direction du point actuel de patrouille qu'elle doit rejoindre
                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime);// L'ia tourne en direction du point de patrouille actuel pour pouvoir se diriger ver celui ci
                        this.transform.Translate(0, 0, Time.deltaTime * vitesse); // Donne une certaine vitesse a l'IA lorsqu'il marche
                    }
                    else if(IsPausing == false && chercheCouverture == true)
                    {
                        if (!searchCover)
                        {
                            direction = pointDePatrouille[actuelPointDePatrouille].transform.position - transform.position; // Permet d'ajuster la direction que doit prendre l'IA a chaque frame, Notemment ici l'IA prend la direction du point actuel de patrouille qu'elle doit rejoindre
                            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime); // L'ia tourne en direction du point de patrouille actuel pour pouvoir se diriger ver celui ci
                            this.transform.Translate(0, 0, Time.deltaTime * vitesse); // Donne une certaine vitesse a l'IA lorsqu'il marche
                        }
                        else if(searchCover)
                        {
                            direction = pointDeCouverture[CherchePointDeCouvertureProche()].transform.position - transform.position;
                            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime);
                            this.transform.Translate(0, 0, Time.deltaTime * vitesse);
                        }
                    }
                    if (Vector3.Distance(pointDeCouverture[CherchePointDeCouvertureProche()].transform.position, transform.position) < 0.3f)
                    {
                        chercheCouverture = false;
                       if(wantToAttack == false) isAimingPlayer = false;
                        estCouvert = true;
                    }
                }
                else
                {
                    IsPatrolling = false; // Si le joueur attaque l'IA, l'IA va venir l'attaquer
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }
            }

            Debug.DrawLine(this.transform.position, direction * 100, Color.yellow);

            Debug.DrawLine(head.transform.position - new Vector3(0f, 0.75f,0f), direction * 100, Color.gray);

            if (  (((Vector3.Distance(Player.position, this.transform.position) < distanceDeVueMax) && (angle < angleDevueMax || IsPatrolling == false))  || saitOuEstLeJoueur) && chercheCouverture == false && estCouvert == false)
            {// Si la distance entre le joueur  ET l'IA auquel on attache ce script est inférieur à la distance de vue max, ET que le joueur se trouve dans la région de l'espace situé dans l'angle de vue défini de l'IAalors on va faire quelquechose
                tempsNouvelleDecision += Time.deltaTime; // Le temps avant une nouvelle décision de l'IA augmonte
                RaycastHit h; // On utilise un raycast pour voir si l'IA voit le joueurs
                if (Physics.Raycast(head.transform.position, Player.position - this.transform.position, out h) && h.transform.position == Player.position)
                { // Si l4IA voit bien le joueur dans sa ligne de mire
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f); // L'IA se tourne ver le joueur
                    IsPatrolling = false;

                    ///////////////////////////////////// A METTRE SI DIFFICULTE FACILE
                    //if (direction.magnitude > distanceAttaque  && estCouvert == false) // Direction.magnitude represente la distance mathémathique entre le joueur et l'IA
                    //{
                    //    this.transform.Translate(0, 0, Time.deltaTime * vitesse); // Permet de faire avancer l'IA sur son axe des Z
                    //    AnimRun();
                    //}
                    ///////////////////////////////////// A METTRE SI DIFFICULTE FACILE

                    isAimingPlayer = true;
                        AnimAim();
                        if ((tempsAvantAttaque > tempsDebutAttaque) && (tempsAvantAttaque <= tempsFinAttaque)) //Permet de faire tirer des rafales a l'IA
                        {
                            if (tempsDeTir > cadenceDetir) // permet de cadancer les tirs de l'IA
                            {
                                Animattack();
                                AttackShoot(direction); // Permet de faire attaquer l'IA
                                tempsDeTir = 0;  
                            }
                            else tempsDeTir += Time.deltaTime; // le temps a attendre pour que l'IA pousse effectuer un autre tir augmonte
                        }
                        else if (tempsAvantAttaque > 1.2f)
                        {
                            tempsAvantAttaque = 0;
                            tempsDebutAttaque = UnityEngine.Random.Range(1f, 0.5f);
                            tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
                        }
                        tempsAvantAttaque += Time.deltaTime; // Incréméente le temps avant la prochaine rafale de balle
                }
                else if (isAimingPlayer == true)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                    AnimAim();
                    tempsAvantArreterPoursuite += Time.deltaTime; 
                    if (tempsNouvelleDecision > UnityEngine.Random.Range(2f, 10f)) // Permet de que l'IA prennent une nouvelle décision lorsque le temps de celui ci est dépassé
                    {
                        chercheCouverture = volonté[UnityEngine.Random.Range(0, 4)]; // ici l'IA va décider de chercher une couverture ou non en fonction de sa volonté
                        tempsNouvelleDecision = 0; //
                        changeDirection = true;
                    }
                    if (tempsAvantArreterPoursuite > 60f) StopPoursuite();// arrete la poursuite de l'IA envers le joueur lorsque celui ci ne le voit plus pendant tout un temps
                }
            }
            else if (IsPatrolling == false && estCouvert == false)
            {
                StopPoursuite(); // arrete la poursuite de l'IA envers le joueur
            }
            else if(estCouvert == true)
            {
                tempsNouvelleDecision += Time.deltaTime;

                if (isAimingPlayer == false) AnimKneel();
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                if (tempsNouvelleDecision > UnityEngine.Random.Range(5f, 15f))
                {
                    tempsNouvelleDecision = 0;
                    isAimingPlayer = volonté[UnityEngine.Random.Range(0, 4)];
                    wantToAttack = true;
                }
                if (vie == IA.vie)
                {
                    RaycastHit h; // On utilise un raycast pour voir si l'IA voit le joueurs
                    if (Physics.Raycast(head.transform.position - new Vector3(0f, 0.75f, 0f), Player.position - this.transform.position, out h) && h.transform.position == Player.position)
                    {
                        AnimAimKneel();
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
                            tempsDebutAttaque = UnityEngine.Random.Range(1f, 0.5f);
                            tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
                        }
                    }
                    else if (isAimingPlayer == true)
                    {
                        AnimAim();
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
                                tempsDebutAttaque = UnityEngine.Random.Range(1f, 0.5f);
                                tempsFinAttaque = UnityEngine.Random.Range(1f, 1.3f);
                            }
                        }
                    }
                    else isAimingPlayer = false;
                }
                else if(vie != IA.vie)
                {
                    AnimKneel();
                    tempsKneelDecision += Time.deltaTime;
                    if(tempsKneelDecision > tempsAvantSeredresser)
                    {
                        tempsAvantSeredresser = UnityEngine.Random.Range(2f, 8f);
                        vie = IA.vie;
                        tempsNouvelleDecision = 0;
                        tempsKneelDecision = 0f;
                    }
                }
                tempsAvantAttaque += Time.deltaTime; // Incréméente le temps avant la prochaine rafale de balle
            }
        }
        else
        {
            AnimDie();
            M4A8.transform.parent = null;
            rbM4A8.useGravity = true;
            bxM4A8.enabled = true;
            Destroy(gameObject, 10);
            if (!mouthHead.isPlaying && playSoundOnce == false)
            {
                mouthHead.PlayOneShot(soundDeath[0]);
                playSoundOnce = true;
            }
        }
    }
    #endregion start & update

    

    #region method
    static private void AnimDie()
    {
        anim.SetBool("IsAimKneel", false);
        anim.SetBool("IsAttacking", false); // ANIMATION Arrete d'attaquer
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : commence a marcher
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsDead", true);
    }
    static private void AnimAim()
    {
        anim.SetBool("IsAimKneel", false);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : arrete de marcher
        anim.SetBool("IsAttacking", false); // ANIMATION : Commence a attaquer
        anim.SetBool("IsAiming", true);
    }
    static private void AnimAimKneel()
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : arrete de marcher
        anim.SetBool("IsAttacking", false); // ANIMATION : Commence a attaquer
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsAimKneel", true);
    }
    static private void AnimKneel()
    {
        anim.SetBool("IsAimKneel", false);
        anim.SetBool("IsAimKneel",false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsAttacking", false); // ANIMATION Arrete d'attaquer
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : commence a marcher
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", true);
    }
    static private void AnimRun()
    {
        anim.SetBool("IsAimKneel", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsAttacking", false); // ANIMATION Arrete d'attaquer
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : commence a marcher
        anim.SetBool("IsRunning", true);
    }
    static private void AnimWalk() // Permet de faire tourner l'animation Walk et de regler l'arme avec l'animation
    {
        anim.SetBool("IsAimKneel", false);
        anim.SetBool("IsRunning",false );
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsAttacking", false); // ANIMATION Arrete d'attaquer
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", true); // ANIMATION : commence a marcher
    }
    static private void AnimIdle() // Permet de faire tourner l'animation idle et de regler l'arme avec l'animation
    {
        anim.SetBool("IsAimKneel", false);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsWalking", false); // ANIMATION : commence a marcher
        anim.SetBool("IsAttacking", false); // ANIMATION : arrete d'attaquer
        anim.SetBool("IsIdle", true); // ANIMATION : fait sa pause
    }
    static private void Animattack() // Permet de faire tourner l'animation Animattack et de regler l'arme avec l'animation
    {
        anim.SetBool("IsAimKneel", false);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : arrete de marcher
        anim.SetBool("IsAttacking", true); // ANIMATION : Commence a attaquer
    }

    private void AttackShoot(Vector3 direction)
    {
        RaycastHit hit; // permet de savoir si le raycaste se confronte a quelque chose

        if (Physics.Raycast(boucheCanon.transform.position, direction, out hit)) // si le raycast est bien en direction du joueur et qu'il le touche bien
        {
            GameObject proj = Instantiate(Projectile) as GameObject; // permet de creer plusieurs balles a l'infini, autant que l'on veut : gameobject proj = balle =
            Projectile.transform.position = boucheCanon.transform.position; // On initialise la positions des balles a la bouche du canon du M4A1
            Rigidbody rb = proj.GetComponent<Rigidbody>(); // permet de récuperer le rigibody du Gameobject proj, qui est en fait la balle
            rb.AddForce(transform.forward * 10000); // permet d'envoyer les balles a une certaine vitess

            Target joueur = hit.transform.GetComponent<Target>(); // permet de recuperer le script de l'entité avec laquelle le 'hit' s'est rencontré

            if(joueur != null) // Si la cible du raycast a bien le script Target attaché
                   joueur.TakeDamage(degats); // On fait subir des dommages au joueurs qui a le script Target attaché

            Sounds.AK47shoot(M4A8Source, M4A8shoot); // permet de jouer le son de tir 

            Destroy(proj, 1f); // detruit le projectil apres 1 seconde d'existance dans le monde du jeux
        }
    }

    private void StopPoursuite()
    {
        IsPatrolling = true;
        vie = IA.vie;
        saitOuEstLeJoueur = false;
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
        int choix = 0;

        if (IA.vie >= (vieMax * 80 / 100)) choix = 1;
        else if (IA.vie >= (vieMax * 50 / 100)) choix = 2;
        else if (IA.vie >= (vieMax * 35 / 100)) choix = 3;
        else if (IA.vie >= (vieMax * 20 / 100)) choix = 4;
        else choix = 5;

        Action<int> Volonte = new Action<int>((decision) =>
      {
          switch (decision)
          {
              case 1: volonté[0] = true; volonté[1] = true; volonté[2] = true; volonté[3] = true; volonté[4] = true; break;
              case 2: volonté[0] = false; volonté[1] = true; volonté[2] = true; volonté[3] = true; volonté[4] = true; break;
              case 3: volonté[0] = false; volonté[1] = false; volonté[2] = true; volonté[3] = true; volonté[4] = true; break;
              case 4: volonté[0] = false; volonté[1] = false; volonté[2] = false; volonté[3] = true; volonté[4] = true; break;
              case 5: volonté[0] = false; volonté[1] = false; volonté[2] = false; volonté[3] = false; volonté[4] = true; break;
              default:  break;
          }
      });

        Volonte(choix);
    }
    #endregion method
}
