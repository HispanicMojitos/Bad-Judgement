using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIscripts : MonoBehaviour
{

    #region membres
    [SerializeField] [Range(0f, 1f)] private float cadenceDetir = 0.1f; // plus cadenceDetir est faible, plus l'IA va tirer rapidement
    private float tempsDeTir = 0f; // TOUCHE PAS A CA PTIT CON = la valeur 0 doit etre absolument initialisée pour permettre a l'IA de tirer
    [SerializeField] private GameObject Projectile; // Recupere la forme des projéctile envoyé par le M4A8
    [SerializeField] private AudioSource M4A8Source; // Recupere la source des son du M4A8
    [SerializeField] private AudioClip M4A8shoot;// Recupere le son du M4A8
    [SerializeField] private GameObject boucheCanon;
    [SerializeField] [Range(0f, 10f)] private float degats = 1f; // Permet de regler les degats de l'IA
    [SerializeField] private Transform M4A8; // prend la position du M4A8
    [SerializeField] private Transform Player; // Nous permet de comparer le joueur a l'intélgience artificelle
    [SerializeField] private Transform head; // Permet de regler les angles de vue par rapport a la tête
    [SerializeField] private static Animator anim; // Récupere les animations de l'IA, on met en static, cela permet de dupliquer l'IA avec ctr+D dans l'editeur de scene 
    private int angleDevueMax = 60; // Angle de vue maximum de l'IA
    private int distanceDeVueMax = 50; // Distance entre l'IA et le joueur a partir de laquelle l'IA va commencer a suivre le joueur
    private int distanceAttaque = 30;// Distance entre l'IA et le joueur a partir de laquelle l'IA va commencer a attaquer
    private Target IA; // permet de récuperer le script Target attaché a l'IA auquel on attache ce script
    private float vie = 0f; //On initialise la vie de l'IA
    private bool IsPatrolling = true; // Permet de savoir quand l'enemi poursuit l'iA 
    [SerializeField] private GameObject[] pointDePatrouille; // Recupere l'ensemble des points de patrouille que l'on veuille mettre a l'IA
    [SerializeField] private float vitesseRotation = 0.2f; // Vitesse de rotation de l'IA
    [SerializeField] private float vitesse = 1.5f; // Vitesse de marche
    private float tailleZonePointDepatrouille = 1f; // Taille des points de patrouille par lesquelle l'IA va prendre la route du prochain point de patrouille
    private int actuelPointDePatrouille = 0; // retourne le point actuel de patrouille
    private bool reversePatrouille = false; // Permet de savoir dans quel sens de la patrouille l'IA est
    private bool IsPausing = false; // reflete si l'IA doit prendre une pause
    private float Pause = 5f; // Durée de la pause

    private bool changeDirection = false;
    private bool[] volonté;
    private bool saitOuEstLeJoueur = false;
    private float nouvelleDecision = 0f;
    private float tempsAvantArreterPoursuite = 0f;
    private float debutAttaque = 0f;
    private float finAttaque = 0f;
    private float tempsAvantAttaque = 0f;
    private int[] pointDePatrouillrProcheDePointDeCouverture; // Valeur entre [] => POINT DE CONTROLEE, valeur tout cours : POINT DE PATRUILLE le plus proche au point de controlle correspondant
    private float vieMax;
    private int choix = 0;
    private bool isAimingPlayer = false;
    private bool chercheCouverture = false;
    private bool wantToAttack = false;
    private bool searchCover = false;
    private bool estCouvert = false;
    [SerializeField] private GameObject[] pointDeCouverture;
     // [SerializeField] private GameObject chercheurCouverture;
    #endregion membres

    #region start & update
    void Start()
    {
        anim = GetComponent<Animator>(); // On récupere les animations dés que le jeux commence
        IA = GetComponent<Target>(); // On récupere les donnée du script Target attaché a la même IA que Ce script-ci
        vie = IA.vie; // On recupere la vie de l'IA via le script Target 
        vieMax = IA.vie;

        volonté = new bool[5];
        debutAttaque = Random.Range(1f, 0.5f);
        finAttaque = Random.Range(1f, 1.3f);
        pointDePatrouillrProcheDePointDeCouverture = new int[pointDeCouverture.Length];


        DeterminePointDePatrouilleProchePointDeCouverture();
    }
    
    void Update()
    {

        if (IA.vie > 0)
        {
            Vector3 direction = Player.position - this.transform.position; // Ici on retourne le rapport de la direction du joueur par rapport a l' IA au niveau de la position de ceux ci dans l'espace virtuel du jeux
            direction.y = 0; // evite que l'IA marche dans le vide lorsqu'on saute
            float angle = Vector3.Angle(direction, head.forward); // Permet de retourner un angle en comparant la position de la tête de l'IA avec celle du joueur

            Debug.DrawLine(transform.position, direction * 100, Color.blue);
            if (IsPatrolling && pointDePatrouille.Length > 0 || chercheCouverture == true) // Si l'IA patrouille ET qu'il existe des point de patrouille pou lui patrouiller, alors  son algorithme se met en place (évite les erreurs)
            {
                if (IA.vie == vie) // Si l'IA n'est pas attaquée
                {
                    if ((!IsPausing) && chercheCouverture == false) AnimWalk(M4A8);// si il n'a pas a faire de pause, il continue son bonhome de chemin
                    else if (chercheCouverture == true)
                    {
                        AnimRun(M4A8);
                    }

                    if ( ((actuelPointDePatrouille %3 == 0 ) && Pause >= 0) && chercheCouverture == false) // Permet de ne faire la pause qu'a un point de patrouille donné
                    {
                        AnimIdle(M4A8);
                        IsPausing = true; // l'IA prens sa pause
                        Pause = Pause - Time.deltaTime; // Malheuresement le temps d'une pause ne dure jamais longtemps !! (Bref le temps de la pause diminue)
                    }
                    else IsPausing = false;

                    if (!(actuelPointDePatrouille %3 == 0)) Pause = Random.Range(5f, 15f); // Permet de remettre la pause a son stade initial 

                    if ( (Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, transform.position) < tailleZonePointDepatrouille && !IsPausing) && chercheCouverture == false) // On verifie la distance entre le point de patrouille actuel et l'IA
                    {
                        //actuelPointDePatrouille = Random.Range(0, pointDePatrouille.Length);
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
                    if( ((Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, transform.position) < tailleZonePointDepatrouille) && chercheCouverture == true && searchCover == false) || changeDirection == true )
                    {
                        if (actuelPointDePatrouille == 1)
                        {
                            searchCover = true;
                        }
                        else if (  actuelPointDePatrouille > 1  )
                        {
                            if(!searchCover) actuelPointDePatrouille--;
                        }
                        else if (actuelPointDePatrouille < 1)
                        {
                            if(!searchCover) actuelPointDePatrouille++;
                        }
                        changeDirection = false;
                    }

                    if (!IsPausing && chercheCouverture == false && searchCover == false)
                    {
                        direction = pointDePatrouille[actuelPointDePatrouille].transform.position - transform.position;
                        // Permet d'ajuster la direction que doit prendre l'IA a chaque frame
                        //Notemment ici l'IA prend la direction du point actuel de patrouille qu'elle doit rejoindre

                        this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime);
                        // L'ia tourne en direction du point de patrouille actuel pour pouvoir se diriger ver celui ci
                        this.transform.Translate(0, 0, Time.deltaTime * vitesse); // Donne une certaine vitesse a l'IA lorsqu'il marche
                    }
                    if(IsPausing == false && chercheCouverture == true)
                    {
                        if (!searchCover)
                        {
                            direction = pointDePatrouille[actuelPointDePatrouille].transform.position - transform.position;
                            // Permet d'ajuster la direction que doit prendre l'IA a chaque frame
                            //Notemment ici l'IA prend la direction du point actuel de patrouille qu'elle doit rejoindre

                            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime);
                            // L'ia tourne en direction du point de patrouille actuel pour pouvoir se diriger ver celui ci
                            this.transform.Translate(0, 0, Time.deltaTime * vitesse); // Donne une certaine vitesse a l'IA lorsqu'il marche
                        }
                        else if(searchCover)
                        {
                            direction = pointDeCouverture[1].transform.position - transform.position;
                            this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime);
                            this.transform.Translate(0, 0, Time.deltaTime * vitesse);
                        }
                    }
                    if (Vector3.Distance(pointDeCouverture[1].transform.position, transform.position) < 0.3f)
                    {
                        chercheCouverture = false;
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


            if (!(IA.vie == vie))
            {
                switch (VolonteEtat())
                {
                    case 1: volonté[0] = true; volonté[1] = true; volonté[2] = true; volonté[3] = true; volonté[4] = true; break;
                    case 2: volonté[0] = false; volonté[1] = true; volonté[2] = true; volonté[3] = true; volonté[4] = true; break;
                    case 3: volonté[0] = false; volonté[1] = false; volonté[2] = true; volonté[3] = true; volonté[4] = true; break;
                    case 4: volonté[0] = false; volonté[1] = false; volonté[2] = false; volonté[3] = true; volonté[4] = true; break;
                    case 5: volonté[0] = false; volonté[1] = false; volonté[2] = false; volonté[3] = false; volonté[4] = true; break;
                }
            }


            if (  (((Vector3.Distance(Player.position, this.transform.position) < distanceDeVueMax) && (angle < angleDevueMax || IsPatrolling == false))  || saitOuEstLeJoueur) && chercheCouverture == false && estCouvert == false)
            {// Si la distance entre le joueur  ET l'IA auquel on attache ce script est inférieur à la distance de vue max, ET que le joueur se trouve dans la région de l'espace situé dans l'angle de vue défini de l'IAalors on va faire quelquechose
                nouvelleDecision += Time.deltaTime;
                RaycastHit h; // On utilise un raycast pour voir si l'IA voit le joueurs
                if (Physics.Raycast(head.transform.position, Player.position - this.transform.position, out h) && h.transform.position == Player.position)
                { // Si 
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                    IsPatrolling = false;

                    if (direction.magnitude > distanceAttaque) // Direction.magnitude represente la distance mathémathique entre le joueur et l'IA
                    {
                        this.transform.Translate(0, 0, Time.deltaTime * vitesse); // Permet de faire avancer l'IA sur son axe des Z
                        AnimRun(M4A8);
                    }
                    else
                    {
                        isAimingPlayer = true;
                        AnimAim(M4A8);
                        if ((tempsAvantAttaque > debutAttaque) && (tempsAvantAttaque <= finAttaque))
                        {
                            if (tempsDeTir > cadenceDetir) // permet de cadancer les tirs de l'IA
                            {
                                Animattack(M4A8);
                                AttackShoot(direction); // Permet de faire attaquer l'IA
                                tempsDeTir = 0;
                            }
                            else tempsDeTir += Time.deltaTime; // le temps a attendre pour que l'IA pousse effectuer un autre tir augmonte
                        }
                        else if (tempsAvantAttaque > 1.2f)
                        {
                            tempsAvantAttaque = 0;
                            debutAttaque = Random.Range(1f, 0.5f);
                            finAttaque = Random.Range(1f, 1.3f);
                        }
                        tempsAvantAttaque += Time.deltaTime;
                    }
                }
                else if (isAimingPlayer == true)
                {
                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                    AnimAim(M4A8);
                    tempsAvantArreterPoursuite += Time.deltaTime;
                    if (nouvelleDecision > 5f)
                    {
                        int rd = Random.Range(0, 4);
                        chercheCouverture = true; // volonté[rd];
                        nouvelleDecision = 0;
                        changeDirection = true;
                    }
                    if (tempsAvantArreterPoursuite > 60f) stopPoursuite();// arrete la poursuite de l'IA envers le joueur
                }
            }
            else if (IsPatrolling == false)
            {
                AnimIdle(M4A8); // Joue l'animation Idle et stabilise la position de l'arme avec
                stopPoursuite(); // arrete la poursuite de l'IA envers le joueur
            }
            else if(estCouvert == true)
            {
                AnimKneel(M4A8);
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                nouvelleDecision += Time.deltaTime;
                if (nouvelleDecision > 5f)
                {
                    nouvelleDecision = 0;
                    isAimingPlayer = true;
                }
                else if(isAimingPlayer == true)
                {
                    RaycastHit h; // On utilise un raycast pour voir si l'IA voit le joueurs
                    if (Physics.Raycast(head.transform.position, Player.position - this.transform.position, out h) && h.transform.position == Player.position)
                    {
                        isAimingPlayer = true;
                        AnimAim(M4A8);
                        if ((tempsAvantAttaque > debutAttaque) && (tempsAvantAttaque <= finAttaque))
                        {
                            if (tempsDeTir > cadenceDetir) // permet de cadancer les tirs de l'IA
                            {
                                Animattack(M4A8);
                                AttackShoot(direction); // Permet de faire attaquer l'IA
                                tempsDeTir = 0;
                            }
                            else tempsDeTir += Time.deltaTime; // le temps a attendre pour que l'IA pousse effectuer un autre tir augmonte
                        }
                        else if (tempsAvantAttaque > 1.2f)
                        {
                            tempsAvantAttaque = 0;
                            debutAttaque = Random.Range(1f, 0.5f);
                            finAttaque = Random.Range(1f, 1.3f);
                        }
                        tempsAvantAttaque += Time.deltaTime;
                    }
                }
            }
        }
        else
        {
            AnimDie();
            M4A8.transform.parent = null;
            Destroy(gameObject, 10);
        }
    }
    #endregion start & update





    #region method
    static private void AnimDie()
    {
        anim.SetBool("IsAttacking", false); // ANIMATION Arrete d'attaquer
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : commence a marcher
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsDead", true);
    }
    static private void AnimAim(Transform M4A8)
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : arrete de marcher
        anim.SetBool("IsAttacking", false); // ANIMATION : Commence a attaquer
        anim.SetBool("IsAiming", true);
        Vector3 M4A8position = new Vector3(0.069f, 1.451f, 0.401f); // On change la position de l'arme pour qu'elle colle avec l'animation
        M4A8.transform.localPosition = M4A8position; // récupere la position locale du M4A8
        M4A8.transform.localRotation = Quaternion.Euler(0f, 177.7f, 0f); // On change la rotation de l'arme pour qu'elle colle avec l'animation (Quaternion.uler permet de faire en sorte que l'objet reste qualibré aux degrés inscrit a chaque frame, MAIS NE FAIT ABSOLUMENT PAS TOURNER L'ARME A CHAQUE FRAME NON!

    }

    static private void AnimKneel(Transform M4A8)
    {
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsAttacking", false); // ANIMATION Arrete d'attaquer
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : commence a marcher
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", true);
        Vector3 M4A8position = new Vector3(-0.079f, 1.079f, 0.254f);
        M4A8.transform.localPosition = M4A8position;
        M4A8.transform.localRotation = Quaternion.Euler(-31.912f, 110.747f, -9.157f);
    }

    static private void AnimRun(Transform M4A8)
    {
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsAttacking", false); // ANIMATION Arrete d'attaquer
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : commence a marcher
        anim.SetBool("IsRunning", true);
        Vector3 M4A8position = new Vector3(-0.079f, 1.079f, 0.254f);
        M4A8.transform.localPosition = M4A8position;
        M4A8.transform.localRotation = Quaternion.Euler(-31.912f, 110.747f, -9.157f);
    }

    static private void AnimWalk(Transform M4A8) // Permet de faire tourner l'animation Walk et de regler l'arme avec l'animation
    {
        anim.SetBool("IsRunning",false );
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsAttacking", false); // ANIMATION Arrete d'attaquer
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", true); // ANIMATION : commence a marcher
        Vector3 M4A8position = new Vector3(-0.079f, 1.079f, 0.254f);
        M4A8.transform.localPosition = M4A8position;
        M4A8.transform.localRotation = Quaternion.Euler(-31.912f, 110.747f, -9.157f);

    }

    static private void AnimIdle(Transform M4A8) // Permet de faire tourner l'animation idle et de regler l'arme avec l'animation
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsWalking", false); // ANIMATION : commence a marcher
        anim.SetBool("IsAttacking", false); // ANIMATION : arrete d'attaquer
        anim.SetBool("IsIdle", true); // ANIMATION : fait sa pause
        Vector3 M4A8position = new Vector3(-0.155f, 1.081f, 0.314f);
        M4A8.transform.localPosition = M4A8position;
        M4A8.transform.localRotation = Quaternion.Euler(-26.864f, 116.425f, -11.742f);
    }

    static private void Animattack(Transform M4A8) // Permet de faire tourner l'animation Animattack et de regler l'arme avec l'animation
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsKneel", false);
        anim.SetBool("IsAiming", false);
        anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
        anim.SetBool("IsWalking", false); // ANIMATION : arrete de marcher
        anim.SetBool("IsAttacking", true); // ANIMATION : Commence a attaquer
        Vector3 M4A8position = new Vector3(0.069f, 1.451f, 0.401f); // On change la position de l'arme pour qu'elle colle avec l'animation
        M4A8.transform.localPosition = M4A8position; // récupere la position locale du M4A8
        M4A8.transform.localRotation = Quaternion.Euler(0f, 177.7f, 0f); // On change la rotation de l'arme pour qu'elle colle avec l'animation (Quaternion.uler permet de faire en sorte que l'objet reste qualibré aux degrés inscrit a chaque frame, MAIS NE FAIT ABSOLUMENT PAS TOURNER L'ARME A CHAQUE FRAME NON!
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

    private void stopPoursuite()
    {
        IsPatrolling = true;
        vie = IA.vie;
        saitOuEstLeJoueur = false;
    }

    private void DeterminePointDePatrouilleProchePointDeCouverture() // Permet de savoir quel point de patrouille est le plus proche de tel point de couverture
    {
        for(int i = 0; i<pointDeCouverture.Length; i++) // Permet de verifier la distance de tout les point de patrouille pour chaque point de couverture
        {
            pointDePatrouillrProcheDePointDeCouverture[i] = 0;

            for (int j = 0; i<pointDePatrouille.Length; j++) // Permet de verifier la ditance de tout les points de patrouille existant
            {
                for(int k = 0; i<pointDePatrouille.Length; k++) // Permet de verifier la distance du point de patrouille séléctioneé
                {
                    if( Vector3.Distance(pointDeCouverture[i].transform.position, pointDePatrouille[j].transform.position) < (Vector3.Distance(pointDeCouverture[i].transform.position, pointDePatrouille[k].transform.position) ))
                    {
                        pointDePatrouillrProcheDePointDeCouverture[i] = j;
                    }
                }
            }
        }
    }

    private int CherchePointDeCouvertureProche() // Permet de savoir quel point de couverture est le plus proche du joueur
    {
        int poinDeCouvertureProche = 0;
        for(int i = 0; i < pointDePatrouillrProcheDePointDeCouverture.Length; i++)
        {
            for (int j = 0; j < pointDePatrouillrProcheDePointDeCouverture.Length; j++)
            {
                if (Mathf.Abs(Vector3.Distance(Player.transform.position, pointDeCouverture[j].transform.position)) < Mathf.Abs(Vector3.Distance(Player.transform.position, pointDeCouverture[i].transform.position)))
                {
                    poinDeCouvertureProche = j;
                }
            }
        }
        return poinDeCouvertureProche;
    }

    private int VolonteEtat()
    {
        if (IA.vie >= (vieMax * 80 / 100) ) return 1;
        else if (IA.vie >= (vieMax * 50 / 100)) return 2;
        else if (IA.vie >= (vieMax * 35 / 100)) return 3;
        else if (IA.vie >= (vieMax * 20 / 100)) return 4;
        else return 5;
    }
    #endregion method
}
