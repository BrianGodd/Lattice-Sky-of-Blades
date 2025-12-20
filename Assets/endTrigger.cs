using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class endTrigger : MonoBehaviour
{
    [SerializeField]CacTime timer;
    [SerializeField] GameObject tpto,player;
    [SerializeField] float waittime;
    bool tpflag;
    // Start is called before the first frame update
    void Start()
    {
        tpflag = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && !tpflag)
        {
            timer.reachEnd();
            tpflag = true;
            StartCoroutine(tpCount());
        }
    }

    IEnumerator tpCount()
    {
        yield return new WaitForSeconds(waittime);
        player.transform.position = tpto.transform.position;
        tpflag = false;
    }


}
