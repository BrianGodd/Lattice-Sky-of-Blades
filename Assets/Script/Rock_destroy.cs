using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock_destroy : MonoBehaviour
{
    bool flag = false;
    [SerializeField] float destroyTime = 0.7f,respawnTime;
    [SerializeField] bool isrespawn;
    [SerializeField] GameObject rock;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator countDownDestroy(float tim)
    {
        yield return new WaitForSeconds(tim);
        if(!isrespawn)Destroy(gameObject);
        else
        {
            rock.SetActive(false);
            yield return new WaitForSeconds(respawnTime);
            rock.SetActive(true);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (flag) return;
            flag = true;
            StartCoroutine(countDownDestroy(destroyTime));
        }
    }


}
