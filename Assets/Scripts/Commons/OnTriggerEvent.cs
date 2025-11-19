using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    public UnityEvent eventToExecute;
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Me Enter");
        if (other.CompareTag("Player"))
        {
            Debug.Log("Me Enter");
            eventToExecute?.Invoke();
        }
    }
}
