using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrowGrenade : MonoBehaviour
{
    [SerializeField] private GameObject smokeGrenade;
    [SerializeField] private GameObject lanceurGrenade;
    public int nbrSmokeGrenade = 2;
	
	void Update ()
    { 
        if (Input.GetKeyDown(KeyCode.G) && nbrSmokeGrenade > 0) // Si on appuie sur G et que le nombre de grenade que l'on transporte est supérieur a 0
        {
            Vector3 direction = lanceurGrenade.transform.TransformDirection(Vector3.forward) * 100;
            RaycastHit hit;
            Physics.Raycast(transform.position, direction, out hit);
            nbrSmokeGrenade--;

            GameObject clone = Instantiate(smokeGrenade, lanceurGrenade.transform); // On crée une smokeGrenade dans le monde
            clone.transform.parent = null; // On detache la grenade de l'object pour pouvoir l'utiliser
            clone.GetComponent<Rigidbody>().AddForce(((direction * 0.15f)), ForceMode.Impulse); // Une certaine force de lancé est appliqué sur la grenade

        }
    }
}
