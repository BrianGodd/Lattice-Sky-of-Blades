using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public Animator playerAnim;
    public JoyStickArea JoyStickArea;
    float stayTime = 0;
    public static bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Touch touch in Input.touches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                if (JoyStickArea.IsPointInCircle(touch.position, JoyStickArea.center, JoyStickArea.radius)) return;
                if(!PlayerEventController.isAbleSlash) return;
                if(touch.position.x < JoyStickArea.center.x)
                {
                    isAttack = true;
                    playerAnim.SetBool("slashleft", true); 
                    playerAnim.SetBool("slashright", false);   
                }
                else
                {
                    isAttack = true;
                    playerAnim.SetBool("slashright", true);
                    playerAnim.SetBool("slashleft", false);
                }
                PlayerEventController.isAbleSlash = false;
            }
        }
    }
}
