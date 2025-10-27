using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XRHealthBehavior : MonoBehaviour
{
    public AudioSource throwS;
    public AudioClip throwC, hitC;
    public Transform parentAttach;
    public GameObject Ball;
    public float MaxHP = 10000, HP;
    private float nextFrameTime = 0, frameTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateBall()
    {
        StartCoroutine(CBall());
        
    }

    public void ThrowSound()
    {
        throwS.clip = throwC;
        throwS.Play();
    }

    public void HitSound()
    {
        throwS.clip = hitC;
        throwS.Play();
    }

    IEnumerator CBall()
    {
        yield return new WaitForSeconds(1f);

        GameObject newBall = Instantiate(Ball);
        newBall.transform.parent = parentAttach;
        newBall.transform.localPosition = Vector3.zero;
        newBall.transform.localRotation = Quaternion.identity;
        newBall.transform.localScale = new Vector3(1, 1, 1);
    }

    void OnTriggerStay(Collider other)
    {
        if(other.tag == "Enemy")
        {
            if(Time.time >= nextFrameTime)
            {
                nextFrameTime = Time.time + frameTime;
                HP -= 400.0f;
            }
        }
    }
}
