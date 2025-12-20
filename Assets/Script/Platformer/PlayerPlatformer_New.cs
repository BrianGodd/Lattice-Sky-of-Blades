using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityStandardAssets.Characters.FirstPerson;
using KinematicCharacterController;

public class PlayerPlatformer_New : MonoBehaviour
{
    public bool isMagicJump = false;
    public Animator effectAnim, camerAnim;
    public Volume speedPP;

    public GameObject lastPlatform;
    public KinematicCharacterMotor motor;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    void Start()
    {
        if (PlayerCharacter.Instance != null)
        {
            PlayerCharacter.Instance.OnMagicJump += MagicJump;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isMagicJump)
        {
            if(motor.BaseVelocity.y < 0) Time.timeScale = 0.9f;
            else if(motor.BaseVelocity.y >= 0) Time.timeScale = 1.2f;
        }

        //speed post-process effect weight adjustment based on player velocity magnitude , max at 20 m/s, min at 15 m/s, linear interpolation
        float vspeed = motor.BaseVelocity.magnitude;
        if(vspeed >= 15f)
        {
            speedPP.weight = Mathf.Clamp01((vspeed - 15f) / (21f - 15f));
        }
        else
        {
            speedPP.weight = 0f;
        }
    }

    public void MagicJump()
    {
        StartCoroutine(MagicEffect());
        if(lastPlatform == null) return;
        AudioSource src = lastPlatform.GetComponent<AudioSource>();

        MusicManager.Instance.PlaySFX3D(
            src.clip,
            lastPlatform.transform.position,
            src.volume
        );
        lastPlatform.GetComponent<MeshDestroy>().DestroyMesh();
    }

    IEnumerator MagicEffect()
    {
        Time.timeScale = 0.01f;
        effectAnim.SetBool("isCharging", true);
        yield return new WaitForSeconds(0.001f);
        //GetComponent<FirstPersonController>().addSpeed = 1.1f;
        //GetComponent<FirstPersonController>().addForwardSpeed = 1.2f;
        isMagicJump = true;
        effectAnim.SetBool("isCharging", false);
        camerAnim.SetBool("fov", true);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platformer") || other.gameObject.layer == LayerMask.NameToLayer("StaticPlatformer"))
        {
            if(camerAnim.GetBool("fov")) camerAnim.SetBool("fov", false);
            isMagicJump = false;
            Time.timeScale = 1f;
            lastPlatform = other.gameObject;
            Debug.Log("Stand on : " + lastPlatform.name);
            //GetComponent<FirstPersonController>().addSpeed = 0f;
            //GetComponent<FirstPersonController>().addForwardSpeed = 0f;
            Debug.Log("Stand On!");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Platformer") || other.gameObject.layer == LayerMask.NameToLayer("StaticPlatformer"))
        {
            //reset timescale
            Time.timeScale = 1f;
            Debug.Log("Left!");
        }
    }
}
