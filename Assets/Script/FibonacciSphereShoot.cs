using System.Collections;
using UnityEngine;

public class ShootDir : MonoBehaviour
{


    void Start()
    {
       // SpawnBullets();
    }

    public static void FibonacciSphere(int count,float radius,float bulletSpeed,Transform transform,GameObject bulletPrefab)
    {
        float offset = 2f / count;
        float increment = Mathf.PI * (3 - Mathf.Sqrt(5)); // 黃金角度
        Quaternion offsetRot = Quaternion.AngleAxis(Random.Range(0,360), Vector3.up);
        for (int i = 0; i < count; i++)
        {
            float y = ((i * offset) - 1) + (offset / 2);
            float r = Mathf.Sqrt(1 - y * y);

            float phi = i * increment;

            Vector3 dir = new Vector3(
                Mathf.Cos(phi) * r,
                y,
                Mathf.Sin(phi) * r
            );
            dir = (offsetRot * dir).normalized;
            Vector3 pos = transform.position + dir * radius;
            GameObject b = Instantiate(bulletPrefab, pos, Quaternion.identity);
            b.GetComponent<Rigidbody>().velocity = dir * bulletSpeed;
        }
    }
    public static void Aimshoot(int count, float bulletSpeed, Transform transform, GameObject bulletPrefab, Transform target,float angleOffset)
    {
        Vector3 dir = target.position - transform.position;
        //dir.y = 0f;
        dir.Normalize();

        // 把方向轉成 Quaternion
        Quaternion centerRot = Quaternion.LookRotation(dir, Vector3.up);


        if (count % 2 == 1)
        {
            // 先射中間那一顆
            SpawnBullet(centerRot,bulletPrefab,transform,bulletSpeed);
        }


        // 左右偏移
        for (int i = 1; i <= count/2; i++)
        {
            float angle = angleOffset * i;

            // 左偏移
            SpawnBullet(centerRot * Quaternion.Euler(0, -angle, 0), bulletPrefab, transform, bulletSpeed);

            // 右偏移
            SpawnBullet(centerRot * Quaternion.Euler(0, +angle, 0), bulletPrefab, transform, bulletSpeed);
        }
    }



     public static IEnumerator mutiAimshootIE(int count, float bulletSpeed, Transform transform, GameObject bulletPrefab, Transform target, float angleOffset, int waves, float delay)
    {
        for (int i = 0; i < waves; i++)
        {
            Aimshoot(count, bulletSpeed, transform, bulletPrefab, target, angleOffset);
            yield return new WaitForSeconds(delay);
        }
    }

    static void SpawnBullet(Quaternion rot,GameObject bulletPrefab,Transform transform,float bulletSpeed)
    {
        GameObject b = Instantiate(bulletPrefab, transform.position, rot);
        b.GetComponent<Rigidbody>().velocity = rot * Vector3.forward * bulletSpeed;
    }

    static float GetAngleFromPosition(Vector3 targetPos,Vector3 orginPos)
    {
        Vector3 dir = targetPos - orginPos;
        dir.y = 0f;

        return Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
    }
    public static IEnumerator SweepFire(GameObject bulletPrefeb,Transform origin,float bulletSpeed, Transform startangleObj, Transform endAngleObj,float sweepTime,float firegap,float tOffset = 0)
    {

        float startAngle = GetAngleFromPosition(startangleObj.position,origin.position);
        if (startAngle < 0) startAngle += 360;
        float endAngle = GetAngleFromPosition(endAngleObj.position,origin.position);
        if (endAngle < 0) endAngle += 360;

        float t = tOffset;
        bool forward = true;
        if (startAngle > endAngle)
        {
            var tmp = endAngle;
            endAngle = startAngle;
            startAngle = tmp;
            t = 1 - t;
            forward = false;

        }
        while (true)
        {
            if (forward)
            {
                t += firegap / sweepTime;
                if (t >= 0.9)
                {
                    t = 1;
                    forward = false;
                }
            }
            else
            {
                t -= firegap / sweepTime;
                if (t <= 0.1)
                {
                    t = 0;
                    forward = true;
                }
            }
            float angle = Mathf.Lerp(startAngle, endAngle, t);

            SpawnBullet(Quaternion.Euler(0,angle,0), bulletPrefeb, origin, bulletSpeed);

            yield return new WaitForSeconds(firegap);
        }
    }

}
