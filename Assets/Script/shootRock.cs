using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootRock : MonoBehaviour
{
    [SerializeField]GameObject rock,origin,target;
    [SerializeField] float speed,gap,offset,minGap,maxGap,Yoffset;
    [SerializeField] bool isRandom,isYrandom;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(countDownShoot());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator countDownShoot()
    {
        
        yield return new WaitForSeconds(offset);
        while (true)
        {
            shoot();
            if(!isRandom)yield return new WaitForSeconds(gap);
            else
            {
                float Rgap = Random.Range(minGap,maxGap);
                yield return new WaitForSeconds(Rgap);
            }
        }
    }

    void shoot()
    {
        Vector3 offsetV = new Vector3(0,Random.Range(-Yoffset,Yoffset),0);
        var curRock = Instantiate(rock,origin.transform.position+offsetV,rock.transform.rotation);
        // var rock_rig = curRock.GetComponent<rockMove>();
        var rock_rig = curRock.GetComponent<RockMovingPlatform>();

        rock_rig.init((target.transform.position - origin.transform.position).normalized * speed,origin.transform.position+offsetV,target.transform.position+offsetV);
        //print((target.transform.position - origin.transform.position).normalized * speed);
    }
}
