using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.Characters.FirstPerson;

public class GameManager : MonoBehaviour
{
    public Camera fir_Camera, thr_Camera;
    public FirstPersonController FPC;
    public Animator asuna, frieren, myAnim, fade;
    public GameObject UIHint, UISit, MainC, hint;
    public Transform ini_rot, VirtualCam;
    public int mode = 0; //0:nothing, 1:sit, 3:hello
    public bool isFirst = true;

    //public JoyStickController joyStickController;

    public int nowLevel = 1;
    public GameObject Reward1, Reward2, Ending;

    public bool isTraining = false;

    // Start is called before the first frame update
    void Start()
    {
        if(isTraining) return;

        if(LevelCompletionManager.Instance.IsLevelCompleted("Level1")) nowLevel = 2;
        if(LevelCompletionManager.Instance.IsLevelCompleted("Level2")) nowLevel = 3;
        if(!LevelCompletionManager.Instance.IsLevelRewarded("Level1") && LevelCompletionManager.Instance.IsLevelCompleted("Level1"))
        {
            LevelCompletionManager.Instance.RewardLevel("Level1");
            ShowReward("Level1");
        }
        if(!LevelCompletionManager.Instance.IsLevelRewarded("Level2") && LevelCompletionManager.Instance.IsLevelCompleted("Level2"))
        {
            LevelCompletionManager.Instance.RewardLevel("Level2");
            ShowReward("Level2");
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(isTraining) return;
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Ending.active) 
            {
                Ending.SetActive(false);
                GetComponent<AudioSource>().Play();
            }
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            LevelCompletionManager.Instance.ResetAllLevels();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(Reward1.active) 
            {
                Reward1.SetActive(false);
                GetComponent<AudioSource>().Play();
            }
            if(Reward2.active) 
            {
                Reward2.SetActive(false);
                GetComponent<AudioSource>().Play();
            }
            hint.SetActive(false);
        }
        /*if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("press!");
            isFirst = !isFirst;
            joyStickController.CameraV = isFirst ? fir_Camera.transform : VirtualCam;
            if(!isFirst) joyStickController.m_MouseLook.Init(joyStickController.fakePlayer , joyStickController.CameraV);
            else joyStickController.m_MouseLook.Init(joyStickController.transform , joyStickController.CameraV);
        }
        fir_Camera.GetComponent<Camera>().enabled = isFirst;
        thr_Camera.GetComponent<Camera>().enabled = !isFirst;
        fir_Camera.gameObject.SetActive(isFirst);
        thr_Camera.gameObject.SetActive(!isFirst);*/
    }

    public void active()
    {
        switch(mode)
        {
            /*case 0:
                UIHint.SetActive(false);
                myAnim.SetBool("standup", true);
                fir_Camera.GetComponent<Camera>().enabled = true;
                thr_Camera.GetComponent<Camera>().enabled = false;
                thr_Camera.gameObject.SetActive(false);
                break;
            case 1:
                UIHint.SetActive(true);    
                fir_Camera.GetComponent<Camera>().enabled = false;
                thr_Camera.GetComponent<Camera>().enabled = true;
                thr_Camera.gameObject.SetActive(true);
                MainC.transform.localPosition = new Vector3(38.768f, -0.319f, 14.811f);
                MainC.transform.rotation = Quaternion.identity;
                myAnim.SetBool("standup", false);
                myAnim.Play("sit");
                break;*/
            case 3:
                //myAnim.SetBool("standup", false);
                asuna.Play("hello");
                myAnim.Play("waving");
                fade.SetBool("fadein", true);
                StartCoroutine(GoToLevelScene(1.5f));
                break;
            case 4:
                if(frieren.gameObject.GetComponent<FrierenController>().isEaten)
                {
                    fade.SetBool("fadein", true);
                    StartCoroutine(GoToTrainingScene(1.5f));
                }
                else
                {
                    //myAnim.SetBool("standup", false);
                    frieren.SetBool("kneel", true);
                    myAnim.Play("waving");
                }
                break;
            case 5:
                asuna.Play("hello");
                fade.SetBool("fadein", true);
                StartCoroutine(GoToHomeScene(1.5f));
                break;

        }
    }

    public void OpenSitHint()
    {
        if(mode != 1) UISit.SetActive(true);
    }

    public void set(int num)
    {
        mode = num;
    }

    IEnumerator GoToTrainingScene(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(SceneManager.sceneCountInBuildSettings - 1);
    }

    IEnumerator GoToHomeScene(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(1);
    }

    IEnumerator GoToLevelScene(float time)
    {
        yield return new WaitForSeconds(time);
        if(nowLevel == 1) SceneManager.LoadScene(2);
        else if(nowLevel == 2) SceneManager.LoadScene(5);
        else if(nowLevel == 3)
        {
            Ending.SetActive(true);
        }
    }

    public void ShowReward(string level)
    {
        hint.SetActive(true);
        switch(level)
        {
            case "Level1":
                Reward1.SetActive(true);
                break;
            case "Level2":
                Reward2.SetActive(true);
                break;
        }
    }
}
