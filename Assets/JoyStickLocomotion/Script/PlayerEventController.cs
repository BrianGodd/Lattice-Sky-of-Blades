using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEventController : MonoBehaviour
{
    public static bool isAbleSlash = true;
    
    public void EndRoll()
    {
        GetComponent<Animator>().SetBool("roll", false);
        JoyStickArea.isRoll = false;
    }

    public void EndLSlash()
    {
        GetComponent<Animator>().SetBool("slashleft", false);
        AttackController.isAttack = false;
        isAbleSlash = true;
    }

    public void EndRSlash()
    {
        GetComponent<Animator>().SetBool("slashright", false);
        AttackController.isAttack = false;
        isAbleSlash = true;
    }

    public void AbleSlash()
    {
        isAbleSlash = true;
    }
}
