using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponRecoil : MonoBehaviour
{

    private Vector3 initialPosition;
    private float force;
    private Transform recoilmodd;
    private float recoil;
    [SerializeField] private Camera cam;
    public float muzzleJumpAngle = 5.0f;
    public float muzzleTurnRatio = 0.2f;
    // Use this for initialization
    void Start()
    {
        initialPosition = this.transform.position;
        cam = transform.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Camera.main.transform.localEulerAngles += new Vector3(Camera.main.transform.localEulerAngles.x + muzzleJumpAngle, Camera.main.transform.localEulerAngles.y + muzzleJumpAngle / 5.0f, 0.0f);
        }

    }
}
