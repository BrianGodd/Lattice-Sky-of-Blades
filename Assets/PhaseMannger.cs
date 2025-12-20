using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseMannger : MonoBehaviour
{

    [SerializeField] List< GameObject> Phasedisables;
    // Start is called before the first frame update
    void Start()
    {
        //changeState(0);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void changeState(int i)
    {
        for (int j = 0; j < Phasedisables.Count; j++)
        {
            if (j == i)
            {
                Phasedisables[j].SetActive(true);
            }
            else
            {
                Phasedisables[j].SetActive(false);
            }
        }
    }

}
