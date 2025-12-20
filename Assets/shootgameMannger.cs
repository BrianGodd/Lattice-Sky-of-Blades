using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootgameMannger : MonoBehaviour
{
    public static shootgameMannger instance;
    [SerializeField] CacTime timer;
    [SerializeField] GameObject shootpoint,player,tpto;
    bool islittlegame = false,tpflag;
    [SerializeField] float waittime;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getHit()
    {
        if (!tpflag)
        {
            timer.reachEnd2();
            shootpoint.SetActive(false);
            islittlegame = false;
            tpflag = true;
            StartCoroutine(tpCount());
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!islittlegame)
            {
                islittlegame = true;
                shootpoint.SetActive(true);    
            }

        }
        
    }
    IEnumerator tpCount()
    {
        yield return new WaitForSeconds(waittime);
        player.GetComponent<PlayerCharacter>().SetTransform(tpto.transform.position);
        tpflag = false;
    }
}
