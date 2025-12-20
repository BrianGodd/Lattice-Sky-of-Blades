using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class targetPlaneMannger : MonoBehaviour
{
    public static targetPlaneMannger instance;
    [SerializeField] GameObject tpto,origin;
    [SerializeField] float waittime,disapearTime;
    int scores,maxscores;
    [SerializeField] int maxRound;
    int nowRound;
    bool unScore;
    [SerializeField] TMP_Text text;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        maxscores = 0;
        //init();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        scores = 0;
        unScore = true;
        nowRound = 0;
        text.text = "Round : " + (nowRound + 1).ToString() + "\nScore : " + scores.ToString();
    }

    public void newRound()
    {
        nowRound += 1;
        if (nowRound == maxRound)
        {
            maxscores = Mathf.Max(maxscores,scores);
            text.text = "Score : " + scores.ToString()+"\n Best Score : "+ maxscores;
            StartCoroutine(tpCount(origin.transform.position));
            StartCoroutine(dialogDisable());
        }
        else
        {
            text.text = "Round : " +(nowRound+1).ToString()+"/"+maxRound.ToString()+"\nScore : " + scores.ToString();
            StartCoroutine(tpCount(tpto.transform.position));
        }
    }

    public void tryAddScore(int Deltascore)
    {
        if (unScore)
        {
            scores += Deltascore;
            unScore = false;
            print(scores);
            newRound();
        }
    }

    IEnumerator tpCount(Vector3 pos)
    {
        yield return new WaitForSeconds(waittime);
        PlayerCharacter.Instance.SetTransform(pos);
        unScore = true;
    }

    IEnumerator dialogDisable()
    {
        yield return new WaitForSeconds(disapearTime);
        text.text = "";

    }

}
