using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeMoney : MonoBehaviour {

    // Use this for initialization

    public Text moneyText;

    void Start()
    {

        moneyText.text = "";
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        RaycastHit hit;

        if ((Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.CompareTag("Money") && Vector3.Distance(transform.position, hit.transform.position) < 10))
        {
            moneyText.enabled = true;
            moneyText.text = "Press F to take the money";

            if (Input.GetKeyDown(KeyCode.F))
            {
                Destroy(hit.transform.gameObject);
                moneyText.enabled = false;
            }
        }
        else if (moneyText.enabled == true) moneyText.enabled = false;
    }
}
