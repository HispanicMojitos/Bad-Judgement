using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectives_M1 : MonoBehaviour {


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

    #endregion

    void Start ()
    {
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

    }


    void Update () {
		
	}
}
