using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerAnimController : MonoBehaviour
{
    public AudioSource Sword, Kirito;
    public AudioClip[] ATTSound;
    public Animator animator;
    public GameObject StarBust, hintT;
    float skillCount = 0, needCount = 36;
    bool isSkill = false, isLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerCharacter.Instance != null)
        {
            PlayerCharacter.Instance.OnJump += PlayJumpAnim;
            PlayerCharacter.Instance.OnLand += PlayLandingAnim;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerCharacter.Instance.IsWalking)
        {
            if(PlayerCharacter.Instance.CurrentVelocity.magnitude > 8f)
            {
                animator.SetBool("run", false);
                animator.SetBool("fast run", true);
            }
            else
            {
                animator.SetBool("fast run", false);
                animator.SetBool("run", true);
            }
        }
        else 
        {
            animator.SetBool("run", false);
            animator.SetBool("fast run", false);
        }

        if(Input.GetMouseButtonDown(0))
        {
            if(!Sword.isPlaying) Sword.Play();
            if(!Kirito.isPlaying)
            {
                Kirito.clip = ATTSound[0];
                Kirito.Play();
            }
            animator.SetBool("slashleft", true);
            animator.SetBool("slashright", false);
            if(isLeft && isSkill)// && !animator.GetCurrentAnimatorStateInfo(4).IsName("rightslash"))
            {
                isLeft = false;
                skillCount+=1;
                GameObject ttt = Instantiate(hintT);
                ttt.GetComponent<HintController>().tt.text = ((int)(skillCount)).ToString();
                Destroy(ttt, 2);
            }
        }

        /*if(!isLeft && isSkill && !animator.GetCurrentAnimatorStateInfo(3).IsName("leftSlash")) RC.SetActive(true);
        else RC.SetActive(false);
        
        if(isLeft && isSkill && !animator.GetCurrentAnimatorStateInfo(4).IsName("rightslash")) LC.SetActive(true);
        else LC.SetActive(false);*/

        if(Input.GetMouseButtonDown(1))
        {
            if(!Sword.isPlaying) Sword.Play();
            if(!Kirito.isPlaying)
            {
                Kirito.clip = ATTSound[1];
                Kirito.Play();
            }
            animator.SetBool("slashright", true);
            animator.SetBool("slashleft", false);
            if(!isLeft && isSkill)// && !animator.GetCurrentAnimatorStateInfo(3).IsName("leftSlash"))
            {
                isLeft = true;
                skillCount+=1;
                GameObject ttt = Instantiate(hintT);
                ttt.GetComponent<HintController>().tt.text = ((int)(skillCount)).ToString();
                Destroy(ttt, 2);
            }
        }
        if(Input.GetKeyDown(KeyCode.F) && !isSkill)
        {
            isSkill = true;
            Kirito.clip = ATTSound[2];
            Kirito.Play();
        }

        if(isSkill)
        {
            animator.SetFloat("speed", (skillCount)/(needCount) + 1f);

            if(skillCount>=needCount)
            {
                isSkill = false;
                skillCount = 0;
                Debug.Log("Success!");
                GameObject st = Instantiate(StarBust);
                st.transform.position = this.transform.position;
                Vector3 stDirection = transform.forward;
                st.transform.rotation = Quaternion.LookRotation(stDirection);
                Destroy(st, 10);
            }
        }
    }

    public void PlayJumpAnim()
    {
        animator.SetBool("jump", true);
        animator.SetBool("run", false);
        animator.SetBool("fast run", false);
    }

    public void PlayLandingAnim()
    {
        Debug.Log("LandAnim");
        animator.SetTrigger("land");
        animator.SetBool("jump", false);
    }

    public void PlayDashAnim()
    {
        animator.SetBool("roll", true);
    }
}
