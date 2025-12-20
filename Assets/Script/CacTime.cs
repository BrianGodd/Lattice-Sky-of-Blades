using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CacTime : MonoBehaviour
{

    float timer;
   [SerializeField] TMP_Text Text;
    float besttimeSpeed = 100000,besttimeBuillet = 0;
    bool countflag;
    // Start is called before the first frame update
    void Start()
    {
        countflag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (countflag)
        {
            timer += Time.deltaTime;
            Text.text = timer.ToString("F3");
        }
        else
        {

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            timer = 0;
            countflag = true;
        }
    }

    public void reachEnd()
    {
        countflag = false;
        besttimeSpeed = Mathf.Min(besttimeSpeed, timer);
        Text.text = "Time : " + timer.ToString("F3")+"\nBest Time : "+besttimeSpeed.ToString("F3");
    }

    public void reachEnd2()
    {
        countflag = false;
        besttimeBuillet = Mathf.Min(besttimeBuillet, timer);
        Text.text = "Time : " + timer.ToString("F3") + "\nBest Time : " + besttimeBuillet.ToString("F3");
    }
}
