using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HPManager : MonoBehaviour
{
    public XRHealthBehavior XRHealthBehavior;
    public PlayerManager playerManager;
    public FrierenController frierenController;
    public MimicController mimicController;
    //public ARFrierenController ARFrierenController;
    public GameObject Blood;
    public TextMeshProUGUI HPUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(XRHealthBehavior!=null)
        {
            HPUI.text = XRHealthBehavior.HP.ToString() + "/" + XRHealthBehavior.MaxHP.ToString();
            Blood.GetComponent<Image>().fillAmount = XRHealthBehavior.HP/XRHealthBehavior.MaxHP;
            Blood.GetComponent<Image>().color = new Color(255f/255f, XRHealthBehavior.HP/XRHealthBehavior.MaxHP, XRHealthBehavior.HP/XRHealthBehavior.MaxHP);
        }
        else if(playerManager!=null)
        {
            HPUI.text = playerManager.HP.ToString() + "/" + playerManager.MaxHP.ToString();
            Blood.GetComponent<Image>().fillAmount = playerManager.HP/playerManager.MaxHP;
            Blood.GetComponent<Image>().color = new Color(255f/255f, playerManager.HP/playerManager.MaxHP, playerManager.HP/playerManager.MaxHP);
        }
        else if(mimicController != null)
        {
            HPUI.text = mimicController.HP.ToString() + "/" + mimicController.MaxHP.ToString();
            Blood.GetComponent<Image>().fillAmount = mimicController.HP/mimicController.MaxHP;
            Blood.GetComponent<Image>().color = new Color(255f/255f, mimicController.HP/mimicController.MaxHP, mimicController.HP/mimicController.MaxHP);
        }
        // else if(ARFrierenController != null)
        // {
        //    HPUI.text = ARFrierenController.HP.ToString() + "/" + ARFrierenController.MaxHP.ToString();
        //     Blood.GetComponent<Image>().fillAmount = ARFrierenController.HP/ARFrierenController.MaxHP;
        //     Blood.GetComponent<Image>().color = new Color(255f/255f, ARFrierenController.HP/ARFrierenController.MaxHP, ARFrierenController.HP/ARFrierenController.MaxHP); 
        // }
        else
        {
            HPUI.text = frierenController.HP.ToString() + "/" + frierenController.MaxHP.ToString();
            Blood.GetComponent<Image>().fillAmount = frierenController.HP/frierenController.MaxHP;
            Blood.GetComponent<Image>().color = new Color(255f/255f, frierenController.HP/frierenController.MaxHP, frierenController.HP/frierenController.MaxHP);
        }
    }
}
