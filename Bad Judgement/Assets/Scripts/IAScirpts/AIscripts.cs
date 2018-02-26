using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIscripts : MonoBehaviour
{

    #region membres
    [SerializeField] private Transform Player; // Nous permet de comparer le joueur a l'intélgience artificelle
    [SerializeField] private Transform head; // Permet de regler les angles de vue par rapport a la tête
    [SerializeField] private static Animator anim; // Récupere les animations de l'IA, on met en static, cela permet de dupliquer l'IA avec ctr+D dans l'editeur de scene 
    private int angleDevueMax = 40; // Angle de vue maximum de l'IA
    private int distanceDeVueMax = 8; // Distance entre l'IA et le joueur a partir de laquelle l'IA va commencer a suivre le joueur
    private int distanceAttaque = 3; // Distance entre l'IA et le joueur a partir de laquelle l'IA va commencer a attaquer

    private bool IsPatrolling = true; // Permet de savoir quand l'enemi poursuit l'iA 
    [SerializeField] private GameObject[] pointDePatrouille; // Recupere l'ensemble des points de patrouille que l'on veuille mettre a l'IA
    [SerializeField] private float vitesseRotation = 0.2f; // Vitesse de rotation de l'IA
    [SerializeField] private float vitesse = 1.5f; // Vitesse de marche
    private float tailleZonePointDepatrouille = 0.1f; // Taille des points de patrouille par lesquelle l'IA va prendre la route du prochain point de patrouille
    private int actuelPointDePatrouille = 0; // retourne le point actuel de patrouille
    private bool reversePatrouille = false; // Permet de savoir dans quel sens de la patrouille l'IA est
    private bool IsPausing = false; // reflete si l'IA doit prendre une pause
    private float Pause = 5f; // Durée de la pause
    #endregion membres

    #region start & update
    void Start ()
    {
        anim = GetComponent<Animator>(); // On récupere les animations dés que le jeux commence
    }

    void Update()
    {

        Vector3 direction = Player.position - this.transform.position; // Ici on retourne le rapport de la direction du joueur par rapport a l' IA au niveau de la position de ceux ci dans l'espace virtuel du jeux

        float angle = Vector3.Angle(direction, head.forward); // Permet de retourner un angle en comparant la position de la tête de l'IA avec celle du joueur

        if (IsPatrolling == true && pointDePatrouille.Length > 0) // Si l'IA patrouille ET qu'il existe des point de patrouille pou lui patrouiller, alors  son algorithme se met en place (évite les erreurs)
        {
            if (IsPausing == false) // si il n'a pas a faire de pause, il continue son bonhome de chemin
            {
                anim.SetBool("IsIdle", false); // ANIMATION : arrete de rien faire
                anim.SetBool("IsWalking", true); // ANIMATION : commence a marcher
                if ((actuelPointDePatrouille == 3 || actuelPointDePatrouille == 5) && Pause > 0) // Permet de ne faire la pause qu'
                {
                    anim.SetBool("IsWalking", false); // arrete de marcher
                    anim.SetBool("IsIdle", true); // commence a rien branler
                    IsPausing = true; // l'IA prens sa pause
                    Pause = Pause - Time.deltaTime; // Malheuresement le temps d'une pause ne dure jamais longtemps !! (Bref le temps de la pause diminue)
                }
                else IsPausing = false;

                if (!(actuelPointDePatrouille == 3 || actuelPointDePatrouille == 5)) Pause = 5f; // Permet de remettre la pause a son stade initial

                if (Vector3.Distance(pointDePatrouille[actuelPointDePatrouille].transform.position, transform.position) < tailleZonePointDepatrouille && IsPausing == false) // On verifie la distance entre le point de patrouille actuel et l'IA
                {
                    //actuelPointDePatrouille = Random.Range(0, pointDePatrouille.Length);
                    if (reversePatrouille == false)
                        actuelPointDePatrouille++; // On a atteint notre point de patrouille, on passe au suivant !
                    else
                        actuelPointDePatrouille--; // On a atteint notre point de patrouille, on passe au suivant ! sens inverse de la patrouille

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
                if (IsPausing == false)
                {
                    direction = pointDePatrouille[actuelPointDePatrouille].transform.position - transform.position; // Permet d'ajuster la direction que doit prendre l'IA a chaque frame
                                                                                                                    //Notemment ici l'IA prend la direction du point actuel de patrouille qu'elle doit rejoindre
                    this.transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), vitesseRotation * Time.deltaTime);
                    // L'ia tourne en direction du point de patrouille actuel pour pouvoir se diriger ver celui ci
                    this.transform.Translate(0, 0, Time.deltaTime * vitesse); // Donne une certaine vitesse a l'IA lorsqu'il marche
                }

            }

            if ((Vector3.Distance(Player.position, this.transform.position) < distanceDeVueMax) && (angle < angleDevueMax || IsPatrolling == false)) // Si la distance entre le joueur  ET l'IA auquel on attache ce script est inférieur à 10, ET que le joueur se trouve dans la région de l'espace situé dans l'angle de vue défini de l'IAalors on va faire quelquechose
            {
                IsPatrolling = false;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f); // La rotation de l'IA va se faire en fonction de 2 parametre : 
                                                                                                                               //Quaternion.Slerp prend en parametre 2 rootation et retourne la rotation de l'IA : 
                                                                                                                               //1er parametre : niveau de base de rotation de l'IA dans l'espace de jeux, 
                                                                                                                               //2eme parametre : nouvelle rotation de l'IA mise a jour en fonction du changement de position du joueur (Vector3 direction)

                anim.SetBool("IsIdle", false); // ANIMATION : permet de ne pas jouer l'animation IsIdle via un Bool grace a l'éditeur d'animation : Animator
                                               // SetBool permet ainsi de définir les animations mise dans l'Animator au moyen d'un bool : false arrete l'animation, true joue l'animation
                if (direction.magnitude > distanceAttaque) // Direction.magnitude represente la distance mathémathique entre le joueur et l'IA
                {
                    this.transform.Translate(0, 0, Time.deltaTime * vitesse); // Permet de faire avancer l'IA sur son axe des Z
                    anim.SetBool("IsWalking", true); // ANIMATION : Va commencer a marcher
                    anim.SetBool("IsAttacking", false); // ANIMATION : arrete d'attaquer
                }
                else
                {
                    anim.SetBool("IsAttacking", true); // ANIMATION : arrete de marcher
                    anim.SetBool("IsWalking", false); // ANIMATION : va attaquer
                }
            }
            else if (IsPatrolling == false)
            {
                anim.SetBool("IsIdle", true); // ANIMATION : Va "rien faire" et rester planté comme un joli petit poirreaux
                anim.SetBool("IsWalking", false); //  ANIMATION : Ne marche pas
                anim.SetBool("IsAttacking", false); // ANIMATION : N'attaque pas 
                IsPatrolling = true;
            }
        }
    }
        #endregion start & update
    
}
