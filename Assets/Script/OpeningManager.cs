using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class OpeningManager : MonoBehaviour
{
    public GameObject MenuCanvas, Main;

    public AudioClip OP;
    public AudioSource Click;

    [Header("OP Images")]
    public GameObject[] opImages;

    [Header("Fade Animation")]
    public Animator fadeAnimator;      // 控制 fade 的 Animator
    public string fadeBoolName = "fading";

    [Header("Hint")]
    public GameObject hintObject;      // 點擊提示

    [Header("Scene")]
    public float lastDelay = 2f;       // 最後一張後等待時間
    public int nextSceneIndex;         // 下一個 scene index

    int currentIndex = -1;
    bool isWaitingInput = false;
    bool isFinished = false;


    public void AnimStart()
    {
        Debug.Log("Start OP");
        if (opImages.Length == 0) return;

        fadeAnimator.SetTrigger(fadeBoolName);

        currentIndex = 0;

        StartCoroutine(DisplayFirst());
        isFinished = false;
    }

    void Update()
    {
        if (!isWaitingInput || isFinished)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            OnClick();
            Click.Play();
        }
    }

    void OnClick()
    {
        hintObject.SetActive(false);
        fadeAnimator.SetTrigger(fadeBoolName);

        currentIndex++;

        // 🔚 最後一張之後
        if (currentIndex >= opImages.Length)
        {
            isFinished = true;
            StartCoroutine(LoadNextScene());
            return;
        }

        StartCoroutine(DisplayImage(currentIndex));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator DisplayFirst()
    {
        GetComponent<AudioSource>().Stop();
        MenuCanvas.SetActive(false);
        yield return new WaitForSeconds(2f);
        GetComponent<AudioSource>().clip = OP;
        GetComponent<AudioSource>().Play();
        Main.SetActive(false);
        opImages[currentIndex].SetActive(true);
        hintObject.SetActive(true);
        isWaitingInput = true;
    }

    IEnumerator DisplayImage(int ind)
    {
        yield return new WaitForSeconds(2f);
        opImages[currentIndex-1].SetActive(false);
        opImages[currentIndex].SetActive(true);
        hintObject.SetActive(true);
    }

    IEnumerator LoadNextScene()
    {
        yield return new WaitForSeconds(lastDelay);
        SceneManager.LoadScene(nextSceneIndex);
    }
}
