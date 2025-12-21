using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tmpResetStage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < GameMaster.instance.DeadY)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(2 + GameMaster.instance.levelIndex);

        }
    }
}
