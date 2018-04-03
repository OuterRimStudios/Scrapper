using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Camera myCamera;

    public float rotationSpeed = 200f;
    public float firstPersonClampValue = 90f;
    public float thirdPersonClampValue = 30f;

    public Vector3 firstPersonOffset;
    public Vector3 thirdPersonOffset;
    public float switchViewSpeed;

    public static bool canAct;

    float clampValue;
    float lookY;
    
    float yRotationValue;
    Quaternion cameraYRotation;

    Transform player;

    bool firstPerson;
    bool switchView;

    private void Start()
    {
        canAct = true;
        firstPerson = true;
        player = GameObject.Find("Player").transform;
    }

    public void RecieveInput(float _lookY)
    {
        lookY = _lookY;
    }

    public void SwitchView()
    {
        firstPerson = !firstPerson;
        switchView = true;
    }

    private void LateUpdate()
    {
        if (!canAct) return;
        Look();
        SwitchingView();
    }

    void Look()
    {
        yRotationValue += -lookY * rotationSpeed * Time.deltaTime;

        if(firstPerson)
            clampValue = thirdPersonClampValue;
        else
            clampValue = firstPersonClampValue;

        yRotationValue = ClampAngle(yRotationValue, -clampValue, clampValue);
        cameraYRotation = Quaternion.Euler(yRotationValue, 0, 0);
        myCamera.transform.localRotation = Quaternion.Slerp(myCamera.transform.localRotation, cameraYRotation, 1);
    }

    void SwitchingView()
    {
        if (switchView)
        {
            if (!firstPerson)
            {
                myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, thirdPersonOffset, switchViewSpeed * Time.deltaTime);

                if (myCamera.transform.localPosition == thirdPersonOffset)
                    switchView = false;
            }
            else
            {
                myCamera.transform.localPosition = Vector3.Lerp(myCamera.transform.localPosition, firstPersonOffset, switchViewSpeed * Time.deltaTime);
                if (myCamera.transform.localPosition == firstPersonOffset)
                    switchView = false;
            }
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360.0f)
            angle += 360.0f;
        if (angle > 360.0f)
            angle -= 360.0f;
        return Mathf.Clamp(angle, min, max);
    }
}
