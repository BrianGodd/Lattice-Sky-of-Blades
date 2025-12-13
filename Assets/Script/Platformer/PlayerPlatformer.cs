using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerPlatformer : MonoBehaviour
{
    public bool isGround = false, isMagicJump = false;
    public Camera playerCamera;
    public Animator effectAnim;
    public Volume speedPP;

    float standTime = 0f, chargeTime = 0f;

    private GameObject lastPlatform;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isGround && Input.GetKey(KeyCode.Space))
        {
            chargeTime += Time.deltaTime;
            if(standTime > 0.3f && chargeTime > 0.3f && lastPlatform.layer == LayerMask.NameToLayer("Platformer"))
            {
                //slowdown game timescale for dramatic effect
                Time.timeScale = Mathf.Clamp(1f - chargeTime * 0.5f, 0.5f, 1f);
                if(effectAnim.GetBool("isCharging") == false)
                {
                    effectAnim.SetBool("isCharging", true);
                }
            }
        }
        if(isGround && Input.GetKeyUp(KeyCode.Space))
        {
            if(standTime < 0.3f && chargeTime < 0.3f)
            {
                Debug.Log("Magic Jump!");
                StartCoroutine(MagicEffect());
                if(lastPlatform.layer == LayerMask.NameToLayer("Platformer")) 
                {
                    AudioSource src = lastPlatform.GetComponent<AudioSource>();

                    MusicManager.Instance.PlaySFX3D(
                        src.clip,
                        lastPlatform.transform.position,
                        src.volume
                    );
                    lastPlatform.GetComponent<MeshDestroy>().DestroyMesh();
                }
            }
            else if(chargeTime < 0.3f)
            {
                Debug.Log("Normal Jump:" + standTime + ", " + chargeTime);
            }
            else if(lastPlatform.layer == LayerMask.NameToLayer("Platformer"))
            {
                Debug.Log("Bouncing Jump!");
                GetComponent<FirstPersonController>().addSpeed = chargeTime * 5f;
                GetComponent<FirstPersonController>().addForwardSpeed = 1.2f;
                if(effectAnim.GetBool("isCharging") == true)
                {
                    effectAnim.SetBool("isCharging", false);
                }
                playerCamera.GetComponent<Animator>().SetBool("fov", true);
            }
            chargeTime = 0f;
        }

        if(isMagicJump)
        {
            if(GetComponent<CharacterController>().velocity.y < 0) Time.timeScale = 0.9f;
            else if(GetComponent<CharacterController>().velocity.y >= 0) Time.timeScale = 1.2f;
        }

        //speed post-process effect weight adjustment based on player velocity magnitude , max at 20 m/s, min at 15 m/s, linear interpolation
        float vspeed = GetComponent<CharacterController>().velocity.magnitude;
        if(vspeed >= 15f)
        {
            speedPP.weight = Mathf.Clamp01((vspeed - 15f) / (21f - 15f));
        }
        else
        {
            speedPP.weight = 0f;
        }
    }

    IEnumerator MagicEffect()
    {
        Time.timeScale = 0.01f;
        effectAnim.SetBool("isCharging", true);
        yield return new WaitForSeconds(0.001f);
        GetComponent<FirstPersonController>().addSpeed = 1.1f;
        GetComponent<FirstPersonController>().addForwardSpeed = 1.2f;
        isMagicJump = true;
        effectAnim.SetBool("isCharging", false);
        playerCamera.GetComponent<Animator>().SetBool("fov", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platformer") || other.gameObject.layer == LayerMask.NameToLayer("StaticPlatformer"))
        {
            if(playerCamera.GetComponent<Animator>().GetBool("fov")) playerCamera.GetComponent<Animator>().SetBool("fov", false);
            standTime = 0f;
            isGround = true;
            isMagicJump = false;
            Time.timeScale = 1f;
            lastPlatform = other.gameObject;
            GetComponent<FirstPersonController>().addSpeed = 0f;
            GetComponent<FirstPersonController>().addForwardSpeed = 0f;
            Debug.Log("Stand On!");
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platformer") || other.gameObject.layer == LayerMask.NameToLayer("StaticPlatformer"))
        {
            standTime += Time.deltaTime;
            if(other.gameObject.GetComponent<Rigidbody>() != null && other.gameObject.layer == LayerMask.NameToLayer("Platformer"))
            {
                Rigidbody platRB = other.gameObject.GetComponent<Rigidbody>();
                platRB.isKinematic = false;
                platRB.useGravity = true;
            }
            //Debug.Log("Standing On!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platformer") || other.gameObject.layer == LayerMask.NameToLayer("StaticPlatformer"))
        {
            //reset timescale
            Time.timeScale = 1f;
            standTime = 0f;
            isGround = false;
            Debug.Log("Left!");
        }
    }
}
