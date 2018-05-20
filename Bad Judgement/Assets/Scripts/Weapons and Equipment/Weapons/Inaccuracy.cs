using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inaccuracy : MonoBehaviour
{

    private Vector3 initialPosition;
    [SerializeField] private float recoil;
    private GameObject weapon;
    WeaponHandler handler;
    // Use this for initialization
    void Start()
    {
        initialPosition = this.transform.position;
        handler = GetComponentInChildren<WeaponHandler>();
        weapon = GetComponentInChildren<Animator>().gameObject;
        //weapon = GetComponentInChildren<WeaponSway>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            if (GunScript.IsAiming)
            {
                weapon.transform.position = Vector3.Slerp(weapon.transform.position, new Vector3(Random.Range(-recoil / 20f, recoil / 20f), Random.Range(-recoil / 40f, recoil / 40f)) + weapon.transform.position, 100f);
            }
            else
            weapon.transform.position = Vector3.Slerp(weapon.transform.position, new Vector3(Random.Range(-recoil / 3f, recoil / 3f), Random.Range(-recoil / 10f, recoil / 10f)) + weapon.transform.position, 20f);
        }
        else
        {
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, initialPosition, 130f);
        }

    }
}
