using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    public UnityEvent eventToExecute;
    
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"OnTriggerEvent: Something entered trigger on {gameObject.name}. Object: {other.gameObject.name}, Tag: {other.tag}");
        
        // Check if player entered (by tag or by PlayerManager component)
        bool isPlayer = other.CompareTag("Player") || other.GetComponent<PlayerManager>() != null;
        
        if (isPlayer)
        {
            Debug.Log($"OnTriggerEvent: Player detected! Invoking event on {gameObject.name}");
            if (eventToExecute != null)
            {
                Debug.Log($"OnTriggerEvent: Event has {eventToExecute.GetPersistentEventCount()} listeners");
                eventToExecute?.Invoke();
            }
            else
            {
                Debug.LogWarning($"OnTriggerEvent: Event is null! No listeners configured.");
            }
        }
        else
        {
            Debug.Log($"OnTriggerEvent: Not a player. Tag: {other.tag}, Has PlayerManager: {other.GetComponent<PlayerManager>() != null}");
        }
    }
    
    void OnTriggerStay(Collider other)
    {
        // Also check in OnTriggerStay in case OnTriggerEnter was missed
        bool isPlayer = other.CompareTag("Player") || other.GetComponent<PlayerManager>() != null;
        if (isPlayer && eventToExecute != null)
        {
            // This is just for debugging - don't invoke multiple times
            // Debug.Log($"OnTriggerEvent: Player staying in trigger on {gameObject.name}");
        }
    }
}
