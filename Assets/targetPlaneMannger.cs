using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetPlaneMannger : MonoBehaviour
{
    public static targetPlaneMannger instance;
    int scores,maxscores;
    bool unScore;
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
        init();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void init()
    {
        scores = 0;
        unScore = true;
    }

    public void newRound()
    {

    }

    public void tryAddScore(int Deltascore)
    {
        if (unScore)
        {
            scores += Deltascore;
            unScore = false;
            print(scores);
        }
    }

}
