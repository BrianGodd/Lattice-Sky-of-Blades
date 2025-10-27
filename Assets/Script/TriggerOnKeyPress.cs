using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerOnKeyPress : MonoBehaviour
{
    [Serializable]
    public class KeyTrigger
    {
        public KeyCode TargetKey;
        public UnityEvent UnityEvent;
    }

    //UI that can add new keys, and bind events to them (like raise event, or change scene) to the keys
    public List<KeyTrigger> KeyTriggerList;

    void Update()
    {
        foreach (var KeyTrigger in KeyTriggerList)
        {
            if (Input.GetKeyDown(KeyTrigger.TargetKey))
            {
                KeyTrigger.UnityEvent?.Invoke();
            }
        }
    }

}
