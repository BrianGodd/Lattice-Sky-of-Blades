using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicController : MonoBehaviour
{
    public AudioSource fA;
    public GameObject frieren;
    public Animator fAn;
    public Vector3 FT;

    public float MaxHP = 8000, HP;
    public float nextFrameTime = 0, frameTime = 0.01f;

    void Start()
    {
        HP = MaxHP;
    }

    void Update()
    {
        if(HP<=0) Destroy(this.gameObject);

    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "QuickAttack")
        {
            HP -= 3000.0f;
            other.enabled = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "FSkill")
        {
            if(Time.time >= nextFrameTime)
            {
                nextFrameTime = Time.time + frameTime;
                HP -= 20.0f;
            }
        }
    }

    public void Crunch()
    {
        frieren.GetComponent<FrierenController>().isEaten = true;
        frieren.transform.parent = this.transform;
        frieren.transform.position = FT;
        fA.Play();
        fAn.SetBool("help", true);
    }
}
