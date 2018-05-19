using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;

public class VanCamControl : MonoBehaviour
{
    private Camera mainCam;

    public static float verticalSensitivity { get; set; }
    public static float horizontalSensitivity { get; set; }
    public static bool isYAxisInverted { get; set; }

    private float minVertical = 70.0F;
    private float maxVertical = -55.0F;

    private Vector3 actualVerticalEulerAngles;
    //The vertical rotation is clamped, so we need this variable

    private static Vector3 seatPos = new Vector3(220F, 3F, 0F);
    private static Vector3 seatAngles = new Vector3(0F, -90F, 0F);
    private static Vector3 pcPos;

    private Animator anim;

    public static bool isSeating { get; private set; }
    private static bool canMoveCam;

    private void Start()
    {
        mainCam = this.GetComponent<Camera>();
        horizontalSensitivity = 5F;
        verticalSensitivity = 5F;
        actualVerticalEulerAngles = Vector3.zero;
        anim = this.GetComponentInParent<Animator>();
        isSeating = true;
        canMoveCam = true;
    }

    private void Update()
    {
        if (canMoveCam)
        {
            float yGameAxis = Input.GetAxis("Mouse X");
            float xGameAxis = Input.GetAxis("Mouse Y");

            if (isYAxisInverted) xGameAxis *= -(1F);

            MoveCam(xGameAxis, yGameAxis);

            if (Input.GetKeyDown(KeyCode.K)) SwitchToEquip();
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.K)) SwitchToFreeView();
        }

    }

    #region Methods

    private void SwitchToEquip()
    {
        canMoveCam = false;
        this.mainCam.transform.localEulerAngles = Vector3.zero;
        this.mainCam.transform.localPosition = Vector3.zero;
        actualVerticalEulerAngles = Vector3.zero;
        anim.Play("VanCam");
        StartCoroutine(WaitEndAnim());
    }

    IEnumerator WaitEndAnim()
    {
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
        isSeating = false;
    }

    private void SwitchToFreeView()
    {
        anim.Play("VanCanReverse");
        isSeating = true;
        canMoveCam = true;
    }

    private void MoveCam(float inputX, float inputY)
    {
        inputX *= verticalSensitivity;
        inputY *= horizontalSensitivity;

        actualVerticalEulerAngles.x += inputX;
        actualVerticalEulerAngles.y += inputY;

        if (actualVerticalEulerAngles.x >= minVertical) actualVerticalEulerAngles.x = minVertical;
        else if (actualVerticalEulerAngles.x <= maxVertical) actualVerticalEulerAngles.x = maxVertical;

        this.mainCam.transform.localEulerAngles = actualVerticalEulerAngles;
    }

    #endregion
}
