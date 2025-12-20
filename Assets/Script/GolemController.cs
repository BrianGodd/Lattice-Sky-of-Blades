using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemController : MonoBehaviour
{
    public Animator animator;
    public float MaxHP = 18000, HP;
    public GameObject[] Rocks;
    public GameManager GameManager;
    float time = 0f, spelltime = 5;
    public Transform[] RockAnchor;
    public float frontForce = 2000f;
    public float UpForce = 200f;

    private GameObject nowRock, nowRock2;

    public Animator cameraAnim, effectAnim;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        spelltime = Random.Range(3,5);
    }

    // Update is called once per frame
    void Update()
    {
        /*if(HP>0)
        {
            // Vector3 lookAtPosition = Kirito.transform.position;
            // lookAtPosition.y = transform.position.y;
            // transform.LookAt(lookAtPosition);
            if(time>=spelltime)
            {
                time = 0;
                spelltime = Random.Range(1,4);
                //this.gameObject.GetComponent<Animator>().Play("throw");
                StartCoroutine(ThrowRock(Rocks[0]));
            }
            else
            {
                time += Time.deltaTime;
            }
        }
        else
        {
            this.gameObject.GetComponent<Animator>().SetBool("die", true);
            StartCoroutine(CallGM(6f));
        }*/
        
    }

    public void StartRoar()
    {
        animator.SetTrigger("roar");
    }

    public void RoarEffect()
    {
        cameraAnim.SetTrigger("roar");
        effectAnim.SetTrigger("roar");
    }

    public void AnimSpawn(int mode)
    {
        nowRock = Instantiate(Rocks[0]);
        nowRock.transform.position = RockAnchor[mode].position;
        nowRock.transform.parent = RockAnchor[mode];
    }

    public void AnimSpawnBoth()
    {
        nowRock = Instantiate(Rocks[0]);
        nowRock.transform.position = RockAnchor[0].position;
        nowRock.transform.parent = RockAnchor[0];

        nowRock2 = Instantiate(Rocks[0]);
        nowRock2.transform.position = RockAnchor[1].position;
        nowRock2.transform.parent = RockAnchor[1];
    }

    public void AnimThrow(int mode)
    {
        nowRock.transform.parent = null;
        nowRock.transform.rotation = Quaternion.Euler(90, 0, 0);
        Rigidbody rb = nowRock.GetComponent<Rigidbody>();
        rb.AddTorque(Vector3.up * 100f, ForceMode.Acceleration);
        rb.AddForce(RockAnchor[mode].forward * frontForce + transform.up * UpForce);
        StartCoroutine(SmallRock(nowRock));
        StartCoroutine(LocRock(nowRock, Random.Range(4f, 6f)));
        Destroy(nowRock, 10f);
    }

    public void AnimThrowBoth()
    {
        nowRock.transform.parent = null;
        nowRock.transform.rotation = Quaternion.Euler(90, 0, 0);
        Rigidbody rb = nowRock.GetComponent<Rigidbody>();
        rb.AddTorque(Vector3.up * 100f, ForceMode.Acceleration);
        rb.AddForce((RockAnchor[0].forward + RockAnchor[0].right*0.3f) * frontForce + transform.up * UpForce);
        StartCoroutine(SmallRock(nowRock));
        StartCoroutine(LocRock(nowRock, Random.Range(4f, 6f)));
        Destroy(nowRock, 10f);

        nowRock2.transform.parent = null;
        nowRock2.transform.rotation = Quaternion.Euler(90, 0, 0);
        Rigidbody rb2 = nowRock2.GetComponent<Rigidbody>();
        rb2.AddTorque(Vector3.up * 100f, ForceMode.Acceleration);
        rb2.AddForce((RockAnchor[0].forward - RockAnchor[0].right*0.3f) * frontForce + transform.up * UpForce);
        StartCoroutine(SmallRock(nowRock2));
        StartCoroutine(LocRock(nowRock2, Random.Range(4f, 6f)));
        Destroy(nowRock2, 10f);
    }

    public void ChangeMode()
    {
        int nowMode = animator.GetInteger("mode");
        //random next mode
        int nextMode = Random.Range(0, 3);
        while(nextMode == nowMode)
        {
            nextMode = Random.Range(0, 3);
        }
        animator.SetInteger("mode", nextMode);
    }

    IEnumerator SmallRock(GameObject rock)
    {
        Vector3 targetScale = new Vector3(0.4f, 0.4f, 0.4f);
        while(rock.transform.localScale.x > targetScale.x)
        {
            rock.transform.localScale = Vector3.Lerp(rock.transform.localScale, targetScale, Time.deltaTime * 0.8f);
            yield return null;
        }
        rock.transform.localScale = targetScale;
    }

    IEnumerator LocRock(GameObject rock, float targetY)
    {
        Rigidbody rb = rock.GetComponent<Rigidbody>();
        while(rock.transform.position.y > targetY)
        {
            yield return null;
        }
        Vector3 vel = rb.velocity;
        vel.y = 0;
        rb.velocity = vel;
        rb.useGravity = false;
    }

    IEnumerator CallGM(float time)
    {
        yield return new WaitForSeconds(time);

        GameManager.active();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "sword")
        {
            HP -= 100.0f;
        }
        if(other.tag == "KSkill")
        {
            HP -= 333.0f;
        }
    }
}
