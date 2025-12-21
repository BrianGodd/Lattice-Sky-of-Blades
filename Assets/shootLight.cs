using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootLight : MonoBehaviour
{

    [SerializeField]GameObject readyCircle, lightnigBolt;
     Quaternion quaternion_ti;
    Vector3 targetPos;
    float radius = 3f;
    [SerializeField] float chargeTime = 2f,delay = 2f,shootDelay = 1f,randomRange = 5f,offsetTime = 2f;
    GameObject circleObj, lightingObj;
    [SerializeField] bool istilt;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(shootLighting());
        quaternion_ti = Quaternion.Euler(26, 0, 0);
       // quaternion_ti *= Quaternion.Euler(90, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
       
        
    }

    IEnumerator shootLighting()
    {
        yield return new WaitForSeconds(offsetTime);
        while (true)
        {
            targetPos = PlayerCharacter.Instance.transform.position;
            if (!istilt)
            {
                targetPos.x += Random.Range(-randomRange, randomRange);
                targetPos.z += Random.Range(-randomRange, randomRange);
                circleObj = Instantiate(readyCircle, targetPos, Quaternion.Euler(0, 0, 0));
            }
            else
            {
                targetPos.x += Random.Range(-randomRange, randomRange);
                float offsetz = Random.Range(-randomRange, randomRange);
                targetPos.y += offsetz * Mathf.Sin(26f/180f*Mathf.PI);
                targetPos.z -= offsetz * Mathf.Cos(26f / 180f * Mathf.PI);
                targetPos.y+=0.3f;
                circleObj = Instantiate(readyCircle, targetPos, quaternion_ti);
                
            }
                yield return new WaitForSeconds(chargeTime);
            lightingObj = Instantiate(lightnigBolt, targetPos , Quaternion.Euler(90, 0, 0));
            if (ishit())
            {
                hit();
            }
            yield return new WaitForSeconds(delay);
            Destroy(circleObj);
            Destroy(lightingObj);
            yield return new WaitForSeconds(shootDelay);
        }
    }

    void hit()
    {
        print("hit");
        UnityEngine.SceneManagement.SceneManager.LoadScene(1 + GameMaster.instance.levelIndex);
    }

    bool ishit()
    {
        Vector3 disV = PlayerCharacter.Instance.transform.position - targetPos;
        disV.y = 0;
        if(disV.magnitude <= radius)
        {
            return true;
        }
        return false;
    }
            
}
