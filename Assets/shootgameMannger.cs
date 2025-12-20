using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shootgameMannger : MonoBehaviour
{
    public static shootgameMannger instance;
    [SerializeField] CacTime timer;
    [SerializeField] GameObject shootpoint;
    bool islittlegame = false;
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void getHit()
    {
        timer.reachEnd2();
        shootpoint.SetActive(false);
        islittlegame = false;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (!islittlegame)
            {
                islittlegame = true;
                shootpoint.SetActive(true);    
            }

        }
        
    }
    
}
