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
    public GameObject UIHint, UISit, MainC;
    public Transform ini_rot, VirtualCam;
    public int mode = 0; //0:nothing, 1:sit, 3:hello
    public bool isFirst = true;

    public JoyStickController joyStickController;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
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
        thr_Camera.gameObject.SetActive(!isFirst);
    }

    public void active()
    {
        switch(mode)
        {
            case 0:
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
                break;
            case 3:
                myAnim.SetBool("standup", false);
                asuna.Play("hello");
                myAnim.Play("waving");
                break;
            case 4:
                if(frieren.gameObject.GetComponent<FrierenController>().isEaten)
                {
                    fade.SetBool("fadein", true);
                    StartCoroutine(GoToScene2(1.5f));
                }
                else
                {
                    myAnim.SetBool("standup", false);
                    frieren.SetBool("kneel", true);
                    myAnim.Play("waving");
                }
                break;
            case 5:
                fade.SetBool("fadein", true);
                StartCoroutine(GoToScene1(1.5f));
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

    IEnumerator GoToScene2(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(1);
    }

    IEnumerator GoToScene1(float time)
    {
        yield return new WaitForSeconds(time);

        SceneManager.LoadScene(0);
    }
}
