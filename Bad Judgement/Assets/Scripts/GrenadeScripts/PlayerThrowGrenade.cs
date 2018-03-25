using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowGrenade : MonoBehaviour
{
    
    [SerializeField] private GameObject smokeGrenade;
    [SerializeField] private GameObject lanceurGrenade;
    private int nbrSmokeGrenade;
    private float distanceMax = 25f;

    // Use this for initialization
    void Start ()
    {
        nbrSmokeGrenade = 2;
    }
	
	// Update is called once per frame
	void Update ()
    {

        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        RaycastHit hit;
        Physics.Raycast(transform.position, direction, out hit);
        Debug.DrawLine(transform.position, direction*3, Color.white);
        if (Input.GetKeyDown(KeyCode.G))
        {
            GameObject clone = Instantiate(smokeGrenade, transform);
            clone.transform.parent = null;

            Vector3 distance = transform.position - hit.transform.position; 
            distance.y -= 30;

            if (distance.magnitude < distanceMax && hit.transform != null)
            {
                clone.GetComponent<Rigidbody>().AddForce(-(distance) * (distance.magnitude / 100), ForceMode.Impulse);
            }
            else
            {
                clone.GetComponent<Rigidbody>().AddForce(-(distance) * (distance.magnitude / 100), ForceMode.Impulse);
            }
            
        }
    }
}
