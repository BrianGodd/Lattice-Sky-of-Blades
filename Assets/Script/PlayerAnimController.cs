using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerAnimController : MonoBehaviour
{
    public bool isScene1 = true, isHurt = false;
    public AudioSource Sword, Kirito;
    public AudioClip[] ATTSound;
    public Animator animator;
    public FirstPersonController FPC;
    public GameObject StarBust, hintT, LC, RC;
    float skillCount = 0, needCount = 36;
    bool isSkill = false, isLeft = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(FPC.isWalk && !FPC.m_Jump)
        {
            animator.SetBool("run", true);
            animator.SetBool("jump", false);
        }
        if(!FPC.m_IsWalking && !FPC.m_Jump)
        {
            animator.SetBool("fast run", true);
            animator.SetBool("jump", false);
        }
        else if(FPC.m_IsWalking)
        {
            animator.SetBool("fast run", false);
        }
        if(FPC.m_Jump)
        {
            animator.SetBool("jump", true);
        }
        if(!FPC.isWalk)
        {
            animator.SetBool("run", false);
            if(!FPC.m_Jump) animator.SetBool("jump", false);
        }

        if(isScene1)
        {
            if(Input.GetKey(KeyCode.F) && !animator.GetBool("slash"))
            {
                Sword.Play();
                Kirito.clip = ATTSound[Random.Range(0, ATTSound.Length)];
                Kirito.Play();
                animator.SetBool("slash", true);
                StartCoroutine(CloseAnim(1.2f, "slash"));
            }
        }
        else
        {
            if(Input.GetMouseButtonDown(0))
            {
                if(!Sword.isPlaying) Sword.Play();
                if(!Kirito.isPlaying)
                {
                    Kirito.clip = ATTSound[0];
                    Kirito.Play();
                }
                animator.Play("leftSlash");
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
                animator.Play("rightslash");
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
                FPC.m_WalkSpeed = 3f;
                FPC.m_RunSpeed = 6f;
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
            else if (isHurt)
            {
                FPC.m_WalkSpeed = 2f;
                FPC.m_RunSpeed = 4f;
            }
            else
            {
                FPC.m_WalkSpeed = 5f;
                FPC.m_RunSpeed = 10f;
            }
        }
    }

    IEnumerator CloseAnim(float time, string anim)
    {
        yield return new WaitForSeconds(time);

        animator.SetBool(anim, false);
    }
}
