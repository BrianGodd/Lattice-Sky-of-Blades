using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterController : MonoBehaviour
{
    enum state
    {
        FibonacciSphere,
        Aimshoot,
        mutiAimshoot
    }
    [SerializeField] bool isRandomMode = false;
    public int count, maxState;
    public float radius,shootdelay,sweepTime;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public GameObject player,origin;
    public GameObject ltarget,rtarget;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(shootState());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int shootMode = 0;
    public float tOffset;
    IEnumerator shootState()
    {
        while (true)
        {
            if(isRandomMode)changeMode();
            yield return StartCoroutine(shoot());
            yield return new WaitForSeconds(shootdelay);
        }
    }

    void changeMode()
    {
        shootMode = Random.Range(0, maxState);
    }
    bool flag = false;
    [ContextMenu("shoot")]
    IEnumerator shoot()
    {
        switch(shootMode)
        {
            case 0:
                yield return StartCoroutine(ShootDir.mutiAimshootIE(count, bulletSpeed, this.transform, bulletPrefab, player.transform, 5, 5,0.2f));
                break;
            case 1:
                ShootDir.Aimshoot(count, bulletSpeed, this.transform,bulletPrefab,player.transform,5);
                break;
            case 2:
                ShootDir.FibonacciSphere(200,radius,bulletSpeed,origin.transform,bulletPrefab);
                break;
            case 3:
                if (!flag)
                {
                    StartCoroutine(ShootDir.SweepFire(bulletPrefab, origin.transform, bulletSpeed, ltarget.transform, rtarget.transform, sweepTime, shootdelay,tOffset));
                    flag = true;
                }
                    break;
        }
        yield return null;

    }


}
