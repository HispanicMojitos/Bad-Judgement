using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIally : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform player;
    [SerializeField] private Target allyHealthState;
    [SerializeField] private Transform boucheCanon;
    [SerializeField] private AudioSource cz805;
    [SerializeField] private AudioClip cz805shoot;
    [SerializeField] private Transform head;
    private new Collider[] collider;
    private List<Transform> enemies;
    private Transform enemiActuel;
    private Movement mvmentPlayer;

    private float degats = 1f;
    private float MaxDistance = 5;
    private float tempsDelayChangerCible;

    private bool doitcourir = false;
    public bool allyEstRéanimé = false;
    private bool aUneCible = false;


    void Start()
    {
        mvmentPlayer = player.GetComponent<Movement>();

        collider = Physics.OverlapSphere(this.transform.position, 100); // permet de recuperer tout les objet dans un rayon determiné
        enemies = new List<Transform>();
        foreach (Collider objetProche in collider)
        {
            if (objetProche.CompareTag("Enemy") == true )
            {
                enemies.Add(objetProche.transform);
            }
        }
    }

	void Update ()
    {
        if (allyHealthState.vie != 0)
        {
            Vector3 direction = player.position - this.transform.position;
            direction.y = 0;

            Debug.DrawRay(head.transform.position, direction * 10,Color.red);

            if (Vector3.Distance(this.transform.position, player.position) > MaxDistance)
            {
                
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);

                if (mvmentPlayer.characterIsRunning == false && doitcourir == false)
                {
                    this.transform.Translate(0, 0, Time.deltaTime * 1.8f);
                    SetAnimation(isWalking: true);
                }
                else if (mvmentPlayer.characterIsRunning == true || doitcourir == true)
                {
                    this.transform.Translate(0, 0, Time.deltaTime * 0.9f);
                    doitcourir = true;
                    SetAnimation(isRunning: true);
                }
            }
            else
            {
                doitcourir = false;
                SetAnimation(isIdle: true);
            }

            tempsDelayChangerCible += Time.deltaTime ;
            if(aUneCible == false && tempsDelayChangerCible > 1)
            {
                foreach(Transform cible in enemies)
                {
                    RaycastHit h;
                    if(Physics.Raycast(head.transform.position, cible.position, out h) && h.transform.position == cible.transform.position)
                    {
                        enemiActuel = cible;
                    }
                }
            }


        }
        else if (allyHealthState.vie == 0 )
        {
            SetAnimation(isDead: true);
        }
    }

    private void SetAnimation(bool isAimKneel = false, bool isKneel = false, bool kneeGrenad = false, bool isIdle = false, bool isAiming = false, bool isAttack = false, bool isWalking = false, bool isRunning = false, bool isAttackingCloser = false, bool isDead = false) // Utilisation de prametre nomé, a récuperer en argument nommer pour décider quelle animation sera jouée
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
    }

    [HideInInspector] public bool AlliéPeutTirer = true;
    private int précision = 10; // Plus cette valeur est grande, plus l'IA est précise dans ses tirs
    private void AttackShoot(Vector3 direction)
    {
        if (AlliéPeutTirer == true)
        {
            RaycastHit hit; // permet de savoir si le raycaste se confronte a quelque chose

            if (Physics.Raycast(boucheCanon.transform.position, direction, out hit)) // si le raycast est bien en direction du joueur et qu'il le touche bien
            {
                float reelDegats;
                int i = UnityEngine.Random.Range(0, précision); // Perme

                if (i == 0) reelDegats = 0; // Permet de choisir le fait si ce tir la fera des degats ou pas au joueurs
                else reelDegats = degats;

                Target joueur = hit.transform.GetComponent<Target>(); // permet de recuperer le script de l'entité avec laquelle le 'hit' s'est rencontré

                if (joueur != null) // Si la cible du raycast a bien le script Target attaché
                    joueur.TakeDamage(reelDegats); // On fait subir des dommages au joueurs qui a le script Target attaché

                Sounds.AK47shoot(cz805, cz805shoot); // permet de jouer le son de tir 
            }
        }
    }
}
