using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inaccuracy : MonoBehaviour
{

    private Vector3 initialPosition;
    [SerializeField] private float spread;
    private float initialSpread;
    private float maxValue;
    private GameObject weapon;
    WeaponHandler handler;
    // Use this for initialization
    void Start()
    {
        initialPosition = this.transform.position;
        handler = GetComponentInChildren<WeaponHandler>();
        weapon = this.GetComponentInChildren<GunScript>().gameObject;
        initialSpread = spread;
        maxValue = initialSpread * 2;
        spread = GetComponentInParent<PlayerLoadout>().weapons[0].Spread;
        //weapon = GetComponentInChildren<WeaponSway>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetButton("Fire1"))
        {

            if (GunScript.IsAiming)
            {
                spread += Time.deltaTime * 5 * spread;
                if (spread >= maxValue * 3) spread = maxValue;

                weapon.transform.position = Vector3.Slerp(weapon.transform.position, new Vector3(Random.Range(-spread / 15f, spread / 15f), Random.Range(-spread / 15f, spread / 15f)) + weapon.transform.position, 200f);
            }
            else
            {
                spread += Time.deltaTime * spread;
                if (spread >= maxValue * 1.5) spread = maxValue;

                weapon.transform.position = Vector3.Slerp(weapon.transform.position, new Vector3(Random.Range(-spread / 5f, spread / 5f), Random.Range(-spread / 5f, spread / 5f)) + weapon.transform.position, 300f);
            }
        }
        else
        {
            spread = initialSpread;
            weapon.transform.position = Vector3.Lerp(weapon.transform.position, initialPosition, 130f);
        }

    }
}
