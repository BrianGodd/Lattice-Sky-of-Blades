using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMaster : MonoBehaviour
{
    public Animator fade;
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
        fade.SetBool("fadein", true);
        Time.timeScale = 0.1f;
        StartCoroutine(GoToHomeScene(0.15f));
    }

    IEnumerator GoToHomeScene(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
    }
}
