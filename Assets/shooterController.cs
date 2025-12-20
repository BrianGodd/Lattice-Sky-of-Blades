using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooterController : MonoBehaviour
{
    public int count;
    public float radius;
    public float bulletSpeed;
    public GameObject bulletPrefab;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int shootMode = 0;

    [ContextMenu("shoot")]
    void shoot()
    {
        switch(shootMode)
        {
            case 0:
                StartCoroutine(ShootDir.mutiAimshootIE(count, bulletSpeed, this.transform, bulletPrefab, player.transform, 5, 5,0.2f));
                break;
            case 1:
                ShootDir.Aimshoot(count, bulletSpeed, this.transform,bulletPrefab,player.transform,5);
                break;
            case 2:
                ShootDir.FibonacciSphere(count,radius,bulletSpeed,this.transform,bulletPrefab);
                break;
        }
      
    }
}
