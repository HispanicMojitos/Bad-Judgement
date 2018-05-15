using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIally : MonoBehaviour
{
    [SerializeField] private AudioSource Mouth;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform player;
    [SerializeField] private Target allyHealthState;
    [SerializeField] private Transform boucheCanon;
    [SerializeField] private AudioSource cz805;
    [SerializeField] private Transform head;
    [SerializeField] private List<Transform> enemies;
    [SerializeField] private TestInteraction choixJoueur;
    private NavMeshAgent cetAllié;
    [HideInInspector] public Transform enemiActuel = null;
    private Movement mvmentPlayer;
    
    private float degats = 0.5f;
    private float MaxDistance = 5;
    private float tempsDelayChangerCible = 0.05f;

    private float temPsAvantEtreDebout = 0;
    private float tempsDebutAttaque = 0f;
    private float tempsFinAttaque = 0f;
    private float tempsAvantAttaque = 0f;
    private float tempsDeTir = 0f;
    private float delayAvntRejoindreJoueur;
    private bool peutRejoindreJoueur = true;
    private bool doitcourir = false;
    [HideInInspector] public bool PeutRevivre = false;
    [HideInInspector] public bool allyEstRéanimé = false;
    private bool peutSuivreJoueur = true;
    [HideInInspector] public bool estHS = false;
    [HideInInspector] public bool ordreDeplacement = false;

    [HideInInspector] public bool autoriséATirer = true;
    private GameObject MuzzleFlash;
    void Start()
    {
        MuzzleFlash = Resources.Load("ParticleEffects/MuzzleFlash") as GameObject;
        cetAllié = this.GetComponent<NavMeshAgent>();
        mvmentPlayer = player.GetComponent<Movement>();
    }


    void FixedUpdate ()
    {
        if (allyHealthState.vie != 0)
        {
            Vector3 direction = player.position - this.transform.position; // Ici on récupere la position du joueur par rapport a l'allié
            direction.y = 0;

            RaycastHit h;
            Debug.DrawRay(head.transform.position, direction * 10,Color.red);

            if (peutSuivreJoueur == true && ordreDeplacement == false) // Ici l'iA va suivre le joueur de manière a le suivre mais de s'arreter si il se trouve trop près
            {
                if (Vector3.Distance(this.transform.position, player.position) > MaxDistance  && peutRejoindreJoueur == true) // PERMET DE SUIVRE LE JOUEUR
                {
                    cetAllié.isStopped = false;
                    cetAllié.SetDestination(player.position);

                    if (mvmentPlayer.characterIsRunning == false && doitcourir == false)
                    {
                        SetAnimation(isWalking: true);
                    }
                    else if (mvmentPlayer.characterIsRunning == true || doitcourir == true)
                    {
                        doitcourir = true;
                        SetAnimation(isRunning: true);
                    }
                }
                else // PERMET D'ARRETER L'IA  lorsqu'il est trop pres du joueur
                {
                    cetAllié.isStopped = true;
                    doitcourir = false;
                    SetAnimation(isIdle: true);

                    if (peutRejoindreJoueur == false)
                    {
                        delayAvntRejoindreJoueur += Time.deltaTime;
                        if (delayAvntRejoindreJoueur > 10)
                        {
                            peutRejoindreJoueur = true;
                            delayAvntRejoindreJoueur = 0;
                        }
                    }
                }
            }
            else if (ordreDeplacement == true)
            {
                if (Vector3.Distance(this.transform.position, (choixJoueur.emplacementCible.position - new Vector3(0, 4f, 0))) > 0.5) // Ici on fais marcher l'IA vers le point chosi
                {
                    cetAllié.isStopped = false;
                    Debug.DrawRay(this.transform.position, (choixJoueur.emplacementCible.position - new Vector3(0, 4f, 0)) * 100, Color.green);
                    SetAnimation(isRunning: true);
                    cetAllié.SetDestination((choixJoueur.emplacementCible.position - new Vector3(0, 4f, 0)));  
                }
                else // Si l'IA a atteind son point a atteindre, elle va  ne plus bouger
                {
                    doitcourir = false;
                    cetAllié.isStopped = true;
                    ordreDeplacement = false;
                    peutRejoindreJoueur = false;
                    choixJoueur.visualisationCiblePosition.SetActive(false);
                    SetAnimation(isIdle: true);
                    Sounds.RadioVoice(GameObject.Find("RadioPlayer").GetComponent<AudioSource>(), "InPosition");
                }

            }

            tempsDelayChangerCible += Time.deltaTime;
            if ((enemiActuel == null && tempsDelayChangerCible > 1) ) // Si l'allié n'as pas de cible, elle va en choisir une
            {
                foreach (Transform cible in enemies)
                {
                    if (cible != null)
                    {
                        if (Physics.Raycast(head.transform.position, (cible.transform.position - transform.position) * 100, out h) && h.transform.position == cible.transform.position)
                        {
                            enemiActuel = cible;
                            Sounds.RadioVoice(GameObject.Find("RadioPlayer").GetComponent<AudioSource>(), "EnemyContact");
                        }
                        if (enemiActuel != null) break;
                    }
                }
                tempsDelayChangerCible = 0;

            }
            else if (enemiActuel != null && ordreDeplacement == false) // PERMER D'ATTAQUER LES CIBLES
            {
                Debug.DrawRay(head.transform.position, (enemiActuel.position - transform.position) * 100, Color.red);
                if (Physics.Raycast(head.transform.position, (enemiActuel.position - transform.position) * 100, out h) && h.transform.position == enemiActuel.transform.position)
                {
                    peutSuivreJoueur = false;
                    peutRejoindreJoueur = false;
                    cetAllié.isStopped = true;
                    direction = enemiActuel.position - this.transform.position;

                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                    SetAnimation(isAiming: true);
                    if (autoriséATirer == true && (tempsAvantAttaque > tempsDebutAttaque) && (tempsAvantAttaque <= tempsFinAttaque)) //Permet de faire tirer des rafales a l'IA
                    {
                        if (tempsDeTir > 0.05f) // permet de cadancer les tirs de l'IA
                        {
                            SetAnimation(isAttack: true);
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

                    if (enemiActuel.GetComponent<AIscripts>().estMort == true) enemiActuel = null;
                }
                else if(Physics.Raycast((head.transform.position - new Vector3(0,0.7f,0)), ((enemiActuel.position - transform.position) - new Vector3(0, 0.7f, 0)) * 100, out h) && h.transform.position == enemiActuel.transform.position)
                { // Si l'enemi est a croupi et que l'IA alliée sait lui tirer dessus
                    peutSuivreJoueur = false;
                    peutRejoindreJoueur = false;
                    cetAllié.isStopped = true;
                    direction = (enemiActuel.position - this.transform.position) - new Vector3(0, 0.7f, 0);

                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                    SetAnimation(isAiming: true);
                    if (autoriséATirer == true && (tempsAvantAttaque > tempsDebutAttaque) && (tempsAvantAttaque <= tempsFinAttaque)) //Permet de faire tirer des rafales a l'IA
                    {
                        if (tempsDeTir > 0.05f) // permet de cadancer les tirs de l'IA
                        {
                            SetAnimation(isAttack: true);
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
                else // ICI L'ALLIE VISE l'IA QUI SE CACHE OU UNE AUTRE IA QUI N'EST PAS CACHEE
                {
                    SetAnimation(isAiming: true);
                    peutSuivreJoueur = false;
                    peutRejoindreJoueur = false;
                    cetAllié.isStopped = true;
                    direction = enemiActuel.position - this.transform.position;

                    this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                }
            }


            if (enemiActuel != null) // Permet de faire changer de cible a l'alliée si la cible qu'elle avait est morte
            {
                if (enemiActuel.GetComponent<AIscripts>().estMort == true)
                {
                    enemiActuel = null;
                    peutSuivreJoueur = true;
                    peutRejoindreJoueur = true;
                    ordreDeplacement = false;

                    Sounds.RadioVoice(GameObject.Find("RadioPlayer").GetComponent<AudioSource>(), "EnemyDown");
                }
            }

        }
        else if (allyHealthState.vie <= 0 && estHS == false && PeutRevivre == false) // Si l'alliée n'a plus de vie
        {
            cetAllié.isStopped = true;
            SetAnimation(isDead: true);
            estHS = true;

            Sounds.Death(Mouth,Resources.Load("Sounds/DeathSongs/Death Screams 1") as AudioClip,false);
        }
        else if (estHS == true && PeutRevivre == true && allyHealthState.vie <= 0) // Ici on active le fait que l'allié Peut Revivre
        {
            
            SetAnimation(seRedresse: true);
            temPsAvantEtreDebout += Time.deltaTime;
            if(temPsAvantEtreDebout > 2f)
            {
                SetAnimation(seRedresse: true);
                temPsAvantEtreDebout = 0;
                estHS = false;
                this.GetComponent<Target>().GainHealth(this.GetComponent<Target>().vieMax/2);
                PeutRevivre = false;
                cetAllié.isStopped = false;
                Sounds.RadioVoice(Mouth, "ThankYou");
            }
             
        }
    }
    


    private void SetAnimation(bool isAimKneel = false, bool isKneel = false, bool kneeGrenad = false, bool isIdle = false, bool isAiming = false, bool isAttack = false, bool isWalking = false, bool isRunning = false, bool isAttackingCloser = false, bool isDead = false, bool seRedresse = false) // Utilisation de prametre nomé, a récuperer en argument nommer pour décider quelle animation sera jouée
    {
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
        anim.SetBool("IsGettingUp", seRedresse);
    }

    [HideInInspector] public bool AlliéPeutTirer = true;
    private int précision = 10; // Plus cette valeur est grande, plus l'IA est précise dans ses tirs
    private void AttackShoot(Vector3 direction, bool cibleEstAGenoux = false)
    {
        if (AlliéPeutTirer == true)
        {
            RaycastHit hit; // permet de savoir si le raycaste se confronte a quelque chose

            if (Physics.Raycast(boucheCanon.transform.position, direction, out hit) && cibleEstAGenoux == false) // si le raycast est bien en direction du joueur et qu'il le touche bien
            {
                float reelDegats;
                int i = UnityEngine.Random.Range(0, précision); // Perme

                if (i == 0) reelDegats = 0; // Permet de choisir le fait si ce tir la fera des degats ou pas au joueurs
                else reelDegats = degats;

                Target joueur = hit.transform.GetComponent<Target>(); // permet de recuperer le script de l'entité avec laquelle le 'hit' s'est rencontré

                if (joueur != null) // Si la cible du raycast a bien le script Target attaché
                    joueur.TakeDamage(reelDegats); // On fait subir des dommages au joueurs qui a le script Target attaché
                
                GameObject FireEffect = Instantiate(MuzzleFlash, boucheCanon.transform) as GameObject;
                Destroy(FireEffect, 1);

                Sounds.Cz805shoot(cz805); // permet de jouer le son de tir 
            }
            else if(cibleEstAGenoux == true && Physics.Raycast((boucheCanon.transform.position - new Vector3(0,0.5f,0)), direction, out hit))
            {
                float reelDegats;
                int i = UnityEngine.Random.Range(0, précision); // Perme

                if (i == 0) reelDegats = 0; // Permet de choisir le fait si ce tir la fera des degats ou pas au joueurs
                else reelDegats = degats;

                Target joueur = hit.transform.GetComponent<Target>(); // permet de recuperer le script de l'entité avec laquelle le 'hit' s'est rencontré

                if (joueur != null) // Si la cible du raycast a bien le script Target attaché
                    joueur.TakeDamage(reelDegats); // On fait subir des dommages au joueurs qui a le script Target attaché
                
                GameObject FireEffect = Instantiate(MuzzleFlash, boucheCanon.transform) as GameObject;
                Destroy(FireEffect, 1);

                Sounds.Cz805shoot(cz805); // permet de jouer le son de tir 
            }
        }
    }
}
