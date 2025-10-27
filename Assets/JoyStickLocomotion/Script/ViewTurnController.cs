using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTurnController : MonoBehaviour
{
    public GameObject FirstPersonCam;
    public JoyStickArea JoyStickArea;
    public Transform CameraV;
    public int turnFingerId = -1;
    private Vector2 startTouchPosition, currentTouchPosition;
    private float targetBackY;
    public Quaternion iniRot, iniCRot;
    public float turnSpeed = 3f;
    public bool isTurnBack = false, isTurnToBack = false;
    public static bool isTurn = false;

    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (JoyStickArea.IsPointInCircle(touch.position, JoyStickArea.center, JoyStickArea.radius)) return;
                isTurn = true;
                iniRot = CameraV.rotation;
                iniCRot = transform.rotation;
                isTurnBack = false;
                turnFingerId = touch.fingerId;
                startTouchPosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
            {
                if (touch.fingerId == turnFingerId)
                {
                    currentTouchPosition = touch.position;

                    float screenWidth = Screen.width;

                    float startFraction = startTouchPosition.x / screenWidth;
                    float endFraction = currentTouchPosition.x / screenWidth;

                    //Debug.Log("Start Fraction: " + startFraction + " , " + "End Fraction: " + endFraction);

                    if(JoyStickArea.isMove) //turn camera
                    {
                        Quaternion targetRot = Quaternion.Euler(19.19f, iniRot.eulerAngles.y + 180*(endFraction-startFraction), iniRot.eulerAngles.z);
                        CameraV.rotation =
                        Quaternion.Slerp(CameraV.rotation, targetRot, Time.deltaTime * turnSpeed);
                    }
                    else //turn character
                    {
                        Quaternion targetRot = Quaternion.Euler(19.19f, iniRot.eulerAngles.y + 180*(endFraction-startFraction), iniRot.eulerAngles.z);
                        CameraV.rotation =
                        Quaternion.Slerp(CameraV.rotation, targetRot, Time.deltaTime * turnSpeed);
                        Quaternion targetCRot = Quaternion.Euler(iniCRot.eulerAngles.x, iniCRot.eulerAngles.y + 180*(endFraction-startFraction), iniCRot.eulerAngles.z);
                        transform.rotation =
                        Quaternion.Slerp(transform.rotation, targetCRot, Time.deltaTime * turnSpeed);
                    }
                }
            }
            else if(touch.phase == TouchPhase.Ended)
            {
                if (touch.fingerId == turnFingerId)
                {
                    turnFingerId = -1;
                    if(JoyStickArea.isMove) //turn camera back
                        isTurnBack = true;
                    else
                    {
                        float startX = startTouchPosition.x / Screen.width;
                        float startY = startTouchPosition.y / Screen.height;
                        float endX = touch.position.x / Screen.width;
                        float endY = touch.position.y / Screen.height;
                        
                        if(Mathf.Abs(endX - startX) < 0.2f && (startY - endY) > 0.1f)
                        {
                            targetBackY = transform.eulerAngles.y + 180;
                            isTurnToBack = true;
                        }
                        else
                            isTurn = false;
                    }
                }
            }
        }

        if(isTurnBack)
        {
            Quaternion targetRot = Quaternion.Euler(19.19f, transform.eulerAngles.y, iniRot.eulerAngles.z);
            CameraV.rotation =
            Quaternion.Slerp(CameraV.rotation, targetRot, Time.deltaTime * turnSpeed);
            if(Mathf.Abs(CameraV.eulerAngles.y - transform.eulerAngles.y) < 0.1f)
            {
                isTurn = false;
                isTurnBack = false;
            }
        }


        if(isTurnToBack)
        {
            Quaternion targetRot = Quaternion.Euler(19.19f, targetBackY, iniRot.eulerAngles.z);
            CameraV.rotation =
            Quaternion.Slerp(CameraV.rotation, targetRot, Time.deltaTime * turnSpeed * 2.5f);
            Quaternion targetCRot = Quaternion.Euler(iniCRot.eulerAngles.x, targetBackY, iniCRot.eulerAngles.z);
            transform.rotation =
            Quaternion.Slerp(transform.rotation, targetCRot, Time.deltaTime * turnSpeed* 2.5f);
            Debug.Log((int)Mathf.Abs(CameraV.eulerAngles.y - targetBackY) % 360);
            if((int)Mathf.Abs(CameraV.eulerAngles.y - targetBackY) % 360 <= 1 ||  (int)Mathf.Abs(CameraV.eulerAngles.y - targetBackY) % 360 >= 359)
            {
                isTurn = false;
                isTurnToBack = false;
            }
        }
    }

    public void SwitchViewer()
    {
        FirstPersonCam.SetActive(!FirstPersonCam.active);
    }
}
