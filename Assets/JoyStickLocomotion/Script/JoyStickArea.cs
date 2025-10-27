using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class JoyStickArea : MonoBehaviour
{
    [System.Serializable] 
    public struct ClickID
    {
        public int FingerId;
        public float stayTime;

        public ClickID(int fingerId, float stayTime)
        {
            this.FingerId = fingerId;
            this.stayTime = stayTime;
        }
    }

    public enum clickState {Ready, Start, Release};
    public clickState nowClickState;
    public Animator playerAnim;
    public Transform CameraV;
    public Image background, focusIcon;
    public GameObject backImage, Player;
    public VariableJoystick VariableJoystick;
    public Vector2 center = Vector2.zero;
    public float radius = 5f;
    public static bool isMove = false, isRoll = false;
    //public int moveFingerId = -1, clickFingerID = -1;
    public Quaternion iniRot;
    float intervalTime = 0;
    public int clickKind = 0;
    public bool islastClick = false, isCount = false;

    public List<ClickID> clickList = new List<ClickID>();
    public JoyStickController JoyStickController;

    void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 mousePosition = touch.position;

                if (touch.phase == TouchPhase.Began)
                {
                    if (IsPointInCircle(mousePosition, center, radius))
                    {
                        if(intervalTime < 0.3f && islastClick)
                        {
                            if(clickWhich(mousePosition, center, radius) == clickKind && !isRoll)
                            {  
                                isRoll = true;
                                if(clickKind == 1)
                                    Player.transform.rotation = Quaternion.LookRotation(-1* Player.transform.right);
                                else if(clickKind == 2)
                                    Player.transform.rotation = Quaternion.LookRotation(Player.transform.right);
                                else if(clickKind == 4)
                                    Player.transform.rotation = Quaternion.LookRotation(-1* Player.transform.forward);
                                iniRot = Player.transform.rotation;
                                playerAnim.SetBool("roll", true);
                                playerAnim.SetBool("run", false);
                                Debug.Log("Double click : " + clickKind);
                                Player.GetComponent<JoyStickController>().RollOnce();
                            }
                        }
                        else if(AttackController.isAttack) iniRot = Player.transform.rotation;
                        else
                            iniRot = CameraV.rotation;
                        //isMove = true;
                        clickList.Add(new ClickID(touch.fingerId, 0));

                        //moveFingerId = touch.fingerId;
                        //stayTime = 0;
                        backImage.GetComponent<Animator>().SetBool("disappear", true);
                        background.raycastTarget = true;
                        Debug.Log("Mouse click within the circle!");
                        break;
                    }
                }
                else if(touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    if(Mathf.Abs(JoyStickController.Vertical) > 0.1f || Mathf.Abs(JoyStickController.Horizontal) > 0.1f)
                    {
                        if(nowClickState != clickState.Start)
                            isMove = true;
                    }
                    else
                    {
                        //Debug.Log(VariableJoystick.Vertical + "," + VariableJoystick.Horizontal);
                        isMove = false;
                    }
                    for(int j = 0;j < clickList.Count;j++)
                    {
                        if(clickList[j].FingerId == touch.fingerId) 
                        {
                            clickList[j] = new ClickID(touch.fingerId, clickList[j].stayTime + Time.deltaTime);
                            if(!isMove) 
                            {
                                focusIcon.fillAmount = clickList[j].stayTime/0.8f;
                                if(focusIcon.fillAmount >= 0.625 && nowClickState != clickState.Ready && nowClickState != clickState.Start)
                                {
                                    nowClickState = clickState.Ready;
                                    Debug.Log("long click ready");
                                }
                                if(focusIcon.fillAmount >= 1 && nowClickState != clickState.Start) 
                                {
                                    nowClickState = clickState.Start;
                                    Debug.Log("long click start");
                                }
                            }
                        }
                    }
                    //stayTime += Time.deltaTime;
                }
                else if(touch.phase == TouchPhase.Ended)
                {
                    //if(stayTime < 0.5f) islastClick = true;
                    //else islastClick = false;
                    for(int j = 0;j < clickList.Count;j++)
                    {
                        if(clickList[j].FingerId == touch.fingerId) 
                        {
                            if(clickList[j].stayTime < 0.5f)
                            {
                                if(!isMove) Debug.Log("short click trigger");
                                islastClick = true;
                                clickKind = clickWhich(mousePosition, center, radius);
                            }
                            else
                            {
                                if(!isMove && clickList[j].stayTime > 0.8f) Debug.Log("long click trigger");
                                islastClick = false;
                            }
                            nowClickState = clickState.Release;
                            focusIcon.fillAmount = 0;
                            isCount = true;
                            intervalTime = 0;
                            clickList.RemoveAt(j);
                            if(!isClickLong())
                            {
                                isMove = false;
                                backImage.GetComponent<Animator>().SetBool("disappear", false);
                                backImage.SetActive(true);
                                background.raycastTarget = true;
                            }
                        }
                    }
                    
                    
                }
            }
        }

        if(isCount)
        {
            intervalTime += Time.deltaTime;
        }
    }

    public bool isClickLong()
    {
        bool isLong = false;
        for(int i = 0;i<clickList.Count;i++)
        {
            if(clickList[i].stayTime >= 0.5f)
            {
                isLong = true;
                break;
            }
        }

        return isLong;
    }

    public bool IsPointInCircle(Vector2 point, Vector2 circleCenter, float circleRadius)
    {
        return Vector2.Distance(point, circleCenter) <= circleRadius;
    }

    public int clickWhich(Vector2 point, Vector2 circleCenter, float circleRadius)
    {
        int kind = 0; //1:left 2:right 3:up 4:down

        if(point.x < circleCenter.x && (circleCenter.x - point.x) >= circleRadius/2) kind = 1;
        else if(point.x > circleCenter.x && (point.x - circleCenter.x) >= circleRadius/2) kind = 2;
        else if(point.y > circleCenter.y && (point.y - circleCenter.y) >= circleRadius/4) kind = 3;
        else kind = 4;

        return kind;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(center, radius);
    }
}