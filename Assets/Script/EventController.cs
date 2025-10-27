using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventController : MonoBehaviour
{
    public void HindObj()
    {
        this.gameObject.SetActive(false);
    }

    public void DestroyObj()
    {
        Destroy(this.gameObject);
    }
}
