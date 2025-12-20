using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpBase : MonoBehaviour
{
    [SerializeField] float Tptime,baseY,MaxY;
    [SerializeField]GameObject Effect,tpto;
    float staytime;
    bool isstay;
    // Start is called before the first frame update
    void Start()
    {
        staytime = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isstay)
        {
            staytime += Time.deltaTime;

            Effect.transform.position = new Vector3(Effect.transform.position.x , Mathf.Lerp(baseY,MaxY,staytime/Tptime), Effect.transform.position.z);
            if (staytime > Tptime)
            {
                staytime = 0;
                isstay = false;
                PlayerCharacter.Instance.SetTransform(tpto.transform.position);
                targetPlaneMannger.instance.init();
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) { 
        isstay = false;
        staytime = 0;
        Effect.transform.position = new Vector3(Effect.transform.position.x,baseY, Effect.transform.position.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if  (other.gameObject.CompareTag("Player")) isstay = true;
    }

}
