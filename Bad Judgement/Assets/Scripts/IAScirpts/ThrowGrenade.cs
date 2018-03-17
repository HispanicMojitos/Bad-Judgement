using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour {

    [SerializeField] private GameObject Player;
    [SerializeField] private Transform cible;
    [SerializeField] private GameObject grenade;
    [SerializeField] private Rigidbody rbGrenade;
    private float tempsDelai;

	// Use this for initialization
	void Start ()
    {
        grenade.GetComponent<GrenadeScript>().joueur = Player;
    }

    // Update is called once per frame
    void Update()
    {
        tempsDelai += Time.deltaTime;
        if (tempsDelai > 1)
        {
            tempsDelai = 0;
            GameObject clone = Instantiate(grenade,this.transform);
            Vector3 distance = this.transform.position - Player.transform.position;
            distance.y -= 15;
            clone.GetComponent<Rigidbody>().AddForce(-(distance) * (distance.magnitude/25) , ForceMode.Impulse);
        }
    }
}
