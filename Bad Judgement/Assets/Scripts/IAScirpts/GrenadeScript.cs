using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeScript : MonoBehaviour {

    [SerializeField] private float delay = 3f;

    private float CountDown;
    private bool aExplosé = false;
    private float rayonExplosion = 5f;

    public GameObject effetExplosion;

	void Start ()
    {
        CountDown = delay;
	}
	
	// Update is called once per frame
	void Update ()
    {
        CountDown -= Time.deltaTime;
        if (CountDown <= 0f && aExplosé == false)
        {
            Explode();
            aExplosé = true;
        }
	}

    private void Explode()
    {
        Instantiate(effetExplosion, transform.position, transform.rotation);

        Physics.Over

        Destroy(gameObject);
    }
}
