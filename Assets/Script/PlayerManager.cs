using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public Animator anim;
    public GameManager GameManager;
    public PlayerAnimController PlayerAnimController;
    public float MaxHP = 18000, HP;
    public float nextFrameTime = 0, frameTime = 0.01f, hurtTime = 3, nowTime = 3;
    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {
        if(nowTime >= hurtTime) PlayerAnimController.isHurt = false;
        else nowTime += Time.deltaTime;

        if(HP<=0) GameManager.active();

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "QuickAttack")
        {
            nowTime = 0;
            if(!PlayerAnimController.isHurt) anim.Play("OnHurt");
            PlayerAnimController.isHurt = true;
            HP -= 4000.0f;
            other.enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "FSkill")
        {
            if(Time.time >= nextFrameTime)
            {
                nowTime = 0;
                if(!PlayerAnimController.isHurt) anim.Play("OnHurt");
                PlayerAnimController.isHurt = true;
                nextFrameTime = Time.time + frameTime;
                HP -= 200.0f;
            }
        }
    }
}
