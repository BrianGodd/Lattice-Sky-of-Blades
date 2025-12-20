using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bulletBehavior : MonoBehaviour
{

    float Maxtime = 8f;
    [SerializeField] bool isStage0;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestroyCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator DestroyCountdown()
    {
        yield return new WaitForSeconds(Maxtime);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            print("Player Hit!");
            if (!isStage0)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            }
            else
            {
                shootgameMannger.instance.getHit();
            }
            //Destroy(other.gameObject);
            //Destroy(this.gameObject);
        }
    }

    public void setTime(float existTime)
    {
        Maxtime = existTime;
    }
}
