using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    #region Gun Sway
    [SerializeField] private float amount;
    [SerializeField] private float smoothAmount;
    [SerializeField] private float maxAmount;
    private Vector3 initialPosition;

    #endregion

    // Use this for initialization
    void Start () {
        initialPosition = transform.localPosition;

    }

    // Update is called once per frame
    void Update () {
        #region Weapon Sway
        // Sounds.AK47shoot(AK47, AK47shoot); // ANDREWS !! si tu met la methode pour jouer le son ici, tu remarquera que le son joue a l'infini et qu'il est cadencé (a la cadence que j'ai mise)
        float movementX = -Input.GetAxis("Mouse X") * amount;
        float movementY = -Input.GetAxis("Mouse Y") * amount;
        // -maxAmount is the amount of movement to the left side
        // maxAmount is the amount of movement to the right side
        movementX = Mathf.Clamp(movementX, -maxAmount, maxAmount);
        // original value, min value, max value
        movementY = Mathf.Clamp(movementY, -maxAmount, maxAmount);
        // this limits the amount of rotation
        Vector3 finalPositon = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPositon + initialPosition,  Time.deltaTime * smoothAmount);
        // this interpolates the initial position with the final position
        #endregion

    }
}
