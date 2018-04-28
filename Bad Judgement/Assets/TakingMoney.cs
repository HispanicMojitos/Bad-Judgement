using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakingMoney : MonoBehaviour {

    private Text displayText;



	// Use this for initialization
	void Start () {
        displayText.text = "";
	}
	
	// Update is called once per frame
	void FixedUpdate () {

        Vector3 direction = transform.TransformDirection(Vector3.forward) * 100;
        RaycastHit hit;

        if ((Physics.Raycast(transform.position, direction, out hit, 3f) && hit.transform.CompareTag("Money") && Vector3.Distance(transform.position, hit.transform.position) < 3))
        {
            SetText();
        }
    }

    void SetText()
    {
        displayText.text = "Press G to take the money";
    }

    private void OnTriggerEnter(Collider thing)
    {
        if (thing.gameObject.CompareTag("Money")) thing.gameObject.SetActive(false);
    }
}
