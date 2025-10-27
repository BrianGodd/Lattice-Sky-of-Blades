using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrierenController : MonoBehaviour
{
    public float MaxHP = 18000, HP;
    public float nextFrameTime = 0, frameTime = 0.2f, nowTime = 3f, healTime = 3f;
    public bool isEaten = false;
    public bool isScene1 = true;
    public Animator mimic;
    public GameObject Kirito, hint;
    public GameObject[] magic;
    public GameManager GameManager;
    float time = 0f, spelltime = 5;

    public float floatSpeed = 10f;
    public float floatMagnitude = 10f;
    private float startY;

    // Start is called before the first frame update
    void Start()
    {
        HP = MaxHP;
        startY = transform.position.y;
        spelltime = Random.Range(5,10);
    }

    // Update is called once per frame
    void Update()
    {
        if(!isScene1)
        {
            if(HP>0)
            {
                Vector3 lookAtPosition = Kirito.transform.position;
                lookAtPosition.y = transform.position.y;
                transform.LookAt(lookAtPosition);
                if(time>=spelltime)
                {
                    time = 0;
                    spelltime = Random.Range(3,7);
                    this.gameObject.GetComponent<Animator>().Play("magic");
                    int magic_id = Random.Range(0, magic.Length);
                    if(magic_id != 0)
                    {
                        GameObject m_hint = Instantiate(hint);
                        m_hint.transform.position = new Vector3(Kirito.transform.position.x, 0.6f, Kirito.transform.position.z);
                        Destroy(m_hint, 1.5f);
                        StartCoroutine(MagicSpell(2f, magic_id, m_hint.transform.position));
                    }
                    else StartCoroutine(MagicSpell(2f, magic_id, Vector3.zero));

                }
                else
                {
                    time += Time.deltaTime;
                }

                if(nowTime >= healTime && HP < MaxHP)
                {
                    if(Time.time >= nextFrameTime)
                    {
                        nextFrameTime = Time.time + frameTime;
                        HP += 50.0f;
                        if(HP > MaxHP) HP = MaxHP;
                    }
                }
                else nowTime += Time.deltaTime;
            
                float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatMagnitude;
                transform.position = new Vector3(transform.position.x, startY + yOffset, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(transform.position.x, startY, transform.position.z);
                this.gameObject.GetComponent<Animator>().SetBool("die", true);
                StartCoroutine(CallGM(6f));
            }

        }

        
    }

    public void CallMimic()
    {
        mimic.Play("Mimic");
    }

    IEnumerator MagicSpell(float time, int ind, Vector3 pos)
    {
        yield return new WaitForSeconds(time);

        GameObject m = Instantiate(magic[ind]);
        Vector3 magicDirection = transform.forward;
        if(ind != 0) m.transform.position =  pos;
        else m.transform.position = transform.position;
        m.transform.rotation = Quaternion.LookRotation(magicDirection);
        Destroy(m, 10f);
    }
                
    IEnumerator CallGM(float time)
    {
        yield return new WaitForSeconds(time);

        GameManager.active();
    }

    void OnTriggerEnter(Collider other)
    {
        if(!isScene1 && other.tag == "sword")
        {
            nowTime = 0;
            HP -= 100.0f;
        }
        if(!isScene1 && other.tag == "KSkill")
        {
            nowTime = 0;
            HP -= 333.0f;
        }
    }
}
