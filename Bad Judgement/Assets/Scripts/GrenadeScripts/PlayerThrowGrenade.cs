using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowGrenade : MonoBehaviour
{
    
    [SerializeField] private GameObject smokeGrenade;
    [SerializeField] private GameObject lanceurGrenade;
    public int nbrSmokeGrenade;
    private float distanceMax = 20f;
    private float distanceMin = 4f;

    void Start ()
    {
        nbrSmokeGrenade = 2;
    }
	
	// Update is called once per frame
	void Update ()
    { 
        if (Input.GetKeyDown(KeyCode.G)/* && nbrSmokeGrenade > 0*/)
        {
            Vector3 direction = lanceurGrenade.transform.TransformDirection(Vector3.forward) * 100;
            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit);
            nbrSmokeGrenade--;

            GameObject clone = Instantiate(smokeGrenade, lanceurGrenade.transform);
            clone.transform.parent = null;
             clone.GetComponent<Rigidbody>().AddForce(((direction * 0.15f)), ForceMode.Impulse);

        }
    }
}
