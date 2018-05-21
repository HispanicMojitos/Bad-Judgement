using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Objectives_M1 : MonoBehaviour
{
    #region Members 
    [SerializeField] private GameObject player;

    private TakeMoney Money;

    public GameObject Showtime;
    public GameObject Escape;
    public GameObject Car;

    public int objDone;
    public bool canExit = false;

    List<GameObject> mesIa = new List<GameObject>();

    #endregion

    #region Objects

    public GameObject Van1;
    public GameObject Van2;
    public GameObject Van3;

    public GameObject shield1;
    public GameObject shield2;
    public GameObject shield3;
    public GameObject shield4;
    public GameObject shield5;
    public GameObject shield6;
    public GameObject shield7;
    public GameObject shield8;
    public GameObject shield9;
    public GameObject shield10;
    public GameObject shield11;
    public GameObject shield12;
    public GameObject shield13;
    public GameObject shield14;
    public GameObject shield15;

    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject enemy5;
    public GameObject enemy6;
    public GameObject enemy7;
    public GameObject enemy8;
    public GameObject enemy9;
    public GameObject enemy10;


    [SerializeField] private List<GameObject> barrieres;

    #endregion



    void Start ()
    {
        #region barriere
        foreach (GameObject bar in barrieres) if(bar != null)bar.SetActive(false); // Permet ainsi de prendre toutes les barrieres dans la liste et de toutes les activer
        #endregion barriere

        #region Van
        Van1.SetActive(false);
        Van2.SetActive(false);
        Van3.SetActive(false);
        #endregion

        #region Shields
        shield1.SetActive(false);
        shield2.SetActive(false);
        shield3.SetActive(false);
        shield4.SetActive(false);
        shield5.SetActive(false);
        shield6.SetActive(false);
        shield7.SetActive(false);
        shield8.SetActive(false);
        shield9.SetActive(false);
        shield10.SetActive(false);
        shield11.SetActive(false);
        shield12.SetActive(false);
        shield13.SetActive(false);
        shield14.SetActive(false);
        shield15.SetActive(false);
        #endregion

        #region ennemies

        enemy1.SetActive(false);
        enemy2.SetActive(false);
        enemy3.SetActive(false);
        enemy4.SetActive(false);
        enemy5.SetActive(false);
        enemy6.SetActive(false);
        enemy7.SetActive(false);
        enemy8.SetActive(false);
        enemy9.SetActive(false);
        enemy10.SetActive(false);

        #endregion

        #region Texts
        Showtime.SetActive(true);
        Escape.SetActive(false);
        Car.SetActive(false);
        #endregion
        Money = player.GetComponentInChildren<TakeMoney>();
        mesIa[0] = enemy1;
        mesIa[1] = enemy2;
        mesIa[2] = enemy3;
        mesIa[3] = enemy4;
        mesIa[4] = enemy5;
        mesIa[5] = enemy6;
        mesIa[6] = enemy7;
        mesIa[7] = enemy8;
        mesIa[8] = enemy9;
        mesIa[9] = enemy10;
    }


    void Update ()
    {
		if ( Money.moneyCount == 1)
        {   
            // Une fois le vol commis le SWAT debarque
            Van1.SetActive(true);
            Van2.SetActive(true);
            Van3.SetActive(true);

            foreach (GameObject bar in barrieres) if (bar != null) bar.SetActive(true);

            shield1.SetActive(true);
            shield2.SetActive(true);
            shield3.SetActive(true);
            shield4.SetActive(true);
            shield5.SetActive(true);
            shield6.SetActive(true);
            shield7.SetActive(true);
            shield8.SetActive(true);
            shield9.SetActive(true);
            shield10.SetActive(true);
            shield11.SetActive(true);
            shield12.SetActive(true);
            shield13.SetActive(true);
            shield14.SetActive(true);
            shield15.SetActive(true);

            enemy1.SetActive(true);
            enemy2.SetActive(true);
            enemy3.SetActive(true);
            enemy4.SetActive(true);
            enemy5.SetActive(true);
            enemy6.SetActive(true);
            enemy7.SetActive(true);
            enemy8.SetActive(true);
            enemy9.SetActive(true);
            enemy10.SetActive(true);
            if (Money.moneyCount == 4)
            {
                Showtime.SetActive(false);
                Escape.SetActive(true);
                objDone++;
            }
        }
        if (objDone == 2 && Money.moneyCount == 4)   // + condition sorti bank ou premiere IA tuee
        {
             if (mesIa.Exists(x => x != null))
            {
                canExit = false;
            }
             else
            {
                canExit = true;
                Escape.SetActive(false);
                Car.SetActive(true);
                objDone++;
            }

        }
        if (objDone == 3 && canExit == true)
        {
            Teleport.ToMainMenu();
        }
	}
}
