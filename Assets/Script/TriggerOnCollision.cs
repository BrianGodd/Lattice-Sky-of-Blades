using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnCollision : MonoBehaviour
{

  public LayerMask TargetLayerMask;

  public UnityEvent UnityEvent, UnityEvent2;

  public float TriggerGap = 5f;
  protected float lastTriggeredTime = -1000f;


  private void OnTriggerStay(Collider other)
  {
    //if layer mask is included in target mask
    if ((TargetLayerMask | (1 << other.gameObject.layer)) == TargetLayerMask)
    {
      UnityEvent?.Invoke();
      lastTriggeredTime = Time.time;
    }
  }

  private void OnTriggerExit(Collider other)
  {
    //if layer mask is included in target mask
    if ((TargetLayerMask | (1 << other.gameObject.layer)) == TargetLayerMask)
    {
        UnityEvent2?.Invoke();
    }
  }
}