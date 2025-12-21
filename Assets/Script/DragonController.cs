using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour
{
    public static DragonController Instance { get; private set; }
    public Animator animator;
    public GameObject stage1, stage2, stage3;
    public GameObject stage3Trigger;
    public Transform[] PlayerInitPos;
    public PlayerCharacter player;
    public Animator cameraAnim, effectAnim;

    public bool isAttack = false;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartRoar()
    {
        StartCoroutine(PlayerReset());
        switch (GameMaster.instance.levelIndex)
        {
            case 3:
                stage1.SetActive(false);
                stage2.SetActive(true);
                break;
            case 4:
                stage2.SetActive(false);
                stage3.SetActive(true);
                stage3Trigger.SetActive(false);
                break;
            case 5:
                GameMaster.instance.Win();
                break;
            default:
                break;
        }
        animator.SetTrigger("roar");
        GetComponent<AudioSource>().Play();
    }

    IEnumerator PlayerReset()
    {
        //lerp player position to init pos over 1 second
        float elapsedTime = 0f;
        Vector3 startingPos = player.transform.position;
        while (elapsedTime < 1f)
        {
            player.SetTransform(Vector3.Lerp(startingPos, PlayerInitPos[GameMaster.instance.levelIndex-3].position, (elapsedTime / 0.5f)));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if(GameMaster.instance.levelIndex == 4) stage3Trigger.SetActive(true);
        GameMaster.instance.levelIndex++;
    }

    public void RoarEffect()
    {
        cameraAnim.SetTrigger("roar");
        effectAnim.SetTrigger("roar");
    }

    public void EndAttack()
    {
        isAttack = false;
    }

}
