using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour {

    #region Gun Sway
    private float _amount;
    private float _smoothAmount;
    private float _maxAmount;
    [SerializeField] private float amount;
    [SerializeField] private float smoothAmount;
    [SerializeField] private float maxAmount;
    [SerializeField] private float aimAmount;
    [SerializeField] private float aimSmoothAmount;
    [SerializeField] private float aimMaxAmount;
    private Vector3 initialPosition;
    #endregion

    // Use this for initialization
    void Start () {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update () {
        #region Weapon Sway
        if(GunScript.IsAiming)
        {
            _amount = aimAmount;
            _smoothAmount = aimSmoothAmount;
            _maxAmount = aimMaxAmount;
        }
        else
        {
            _amount = amount;
            _smoothAmount = smoothAmount;
            _maxAmount = maxAmount;
        }
        // Sounds.AK47shoot(AK47, AK47shoot); // ANDREWS !! si tu met la methode pour jouer le son ici, tu remarquera que le son joue a l'infini et qu'il est cadencé (a la cadence que j'ai mise)
        float movementX = -Input.GetAxis("Mouse X") * _amount;
        float movementY = -Input.GetAxis("Mouse Y") * _amount;
        // -maxAmount is the amount of movement to the left side
        // maxAmount is the amount of movement to the right side
        movementX = Mathf.Clamp(movementX, -_maxAmount, _maxAmount);
        // original value, min value, max value
        movementY = Mathf.Clamp(movementY, -_maxAmount, _maxAmount);
        // this limits the amount of rotation
        Vector3 finalPositon = new Vector3(movementX, movementY, 0);
        transform.localPosition = Vector3.Lerp(transform.localPosition, finalPositon + initialPosition,  Time.deltaTime * _smoothAmount);
        // this interpolates the initial position with the final position
        #endregion

    }
}
