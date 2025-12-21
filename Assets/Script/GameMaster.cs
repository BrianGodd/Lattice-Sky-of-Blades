using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;
    public int levelIndex = 0;

    public Transform DebugPlayerPos;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PlayerCharacter.Instance.SetTransform(DebugPlayerPos.position);
        }
    }

    public void Win()
    {
        Debug.Log("You Win!");
    }
}
