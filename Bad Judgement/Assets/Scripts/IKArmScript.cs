using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKArmScript : MonoBehaviour {

    [SerializeField]
    private Transform target;
    // This what we will be trying to grab
    [SerializeField]
    private Transform handTransform;
    // This is the hand XD
    [SerializeField]
    private Transform shoulderTransform;
    // This is.... the shoulder
    [SerializeField]
    private Transform shoulderElbowPoint;
    // This is the part that joins the wrist and shuolder
    [SerializeField]
    private float bicepLength;
    // This is the bicep length duh

    [SerializeField]
    private Transform wristTransform;
    // Wrist point
    [SerializeField]
    private Transform wristElbowPoint;
    // Joint between wrist and bicep
    private float wristLength;
    // This is the forearm length

    [SerializeField]
    private Transform elbowWeight;
    // This will be the elbow's orientation

    private float elbowZ;
    // This is the variable distance between the shoulder and the elbow
    private float distToTarget;
    // Distance between shoulder and target because we don't want the shoulder to get all wonky if the gun is too far


	// Use this for initialization
	void Start () {

        bicepLength = Vector3.Distance(shoulderTransform.position, shoulderElbowPoint.position);
        // Here we get the distance between shoulder and elbow point
        wristLength = Vector3.Distance(wristTransform.position, shoulderTransform.position);
        // Same thing but for the forearm
		
	}
	
	// Update is called once per frame
	void Update () {

        handTransform.rotation = target.rotation;
        // Follow the target's rotation to fit on the gun handle
        handTransform.position = wristTransform.position;
        // We don't put this = the target because if the gun goes out of range the hand will stretch
        transform.LookAt(target, transform.position - elbowWeight.position);
        // We access the transform property of the object where this script will be
        // we look at the target and tell where does the arm move from there
        // in this case we want the elbow to look downwards
        // that's why we substract the elbow weight
        distToTarget = Vector3.Distance(target.position, shoulderTransform.position);
        // We search the distance between the shoulder and the gun
        // now we search for our pretty formula from www.MathWorld.WolfRam.com/Circle-CircleIntersection.html
        // this is for searching where the elbow should bend
        elbowZ = (Mathf.Pow(distToTarget, 2) - Mathf.Pow(wristLength, 2) + Mathf.Pow(bicepLength, 2)) / (distToTarget * 2);

        float tmpZ = Mathf.Clamp(distToTarget, 0, wristLength + bicepLength);
        wristTransform.localPosition = tmpZ * Vector3.forward;
        // this will prevent the wrist from being too far or too close

        if (distToTarget < bicepLength + wristLength && distToTarget > Mathf.Max(bicepLength, wristLength) - Mathf.Min(bicepLength, wristLength))
        {// this prevents the 2 "circles" from being one inside the other
            shoulderTransform.localRotation = Quaternion.Euler(Mathf.Acos(elbowZ / bicepLength) *
                Mathf.Rad2Deg, 0, 0);
            // this gives us the rotation that we need on the x axis, Acos does the contrary of cos you enter 2 values and it gives you the rotation needed to have that value
            // we then proceed to transform that value into degrees

            wristTransform.localRotation = Quaternion.Euler(-(Mathf.Acos((distToTarget - elbowZ) / wristLength) * Mathf.Rad2Deg), 0, 0);
            // same thing as above but we use negative because are working backwards

        }

        if (distToTarget >= bicepLength+wristLength)
        {
            shoulderTransform.localRotation = Quaternion.Euler(0, 0, 0);
            wristTransform.localRotation = Quaternion.Euler(0, 0, 0);
            // if the target is too far then we have straight arms
        }
        if (distToTarget <= Mathf.Max(bicepLength,wristLength) - Mathf.Min(bicepLength,wristLength))
        {
            shoulderTransform.localRotation = Quaternion.Euler(0, 0, 0);
            wristTransform.localRotation = Quaternion.Euler(180, 0, 0);
            // flip wrist backwards
        }
        

    }
}
