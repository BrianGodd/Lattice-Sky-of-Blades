using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootRock : MonoBehaviour
{
    [SerializeField]GameObject rock,origin,target;
    [SerializeField] float speed,gap,offset;
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
            yield return new WaitForSeconds(gap);
        }
    }

    void shoot()
    {
        var curRock = Instantiate(rock,origin.transform.position,rock.transform.rotation);
        // var rock_rig = curRock.GetComponent<rockMove>();
        var rock_rig = curRock.GetComponent<RockMovingPlatform>();

        rock_rig.init((target.transform.position - origin.transform.position).normalized * speed,origin.transform.position,target.transform.position);
        //print((target.transform.position - origin.transform.position).normalized * speed);
    }
}
