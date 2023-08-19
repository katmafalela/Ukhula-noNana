using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yarn.Unity;

public class TriggerEvents : MonoBehaviour
{
    public UnityEvent onTriggerEnter;
    public UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            onTriggerEnter.Invoke();
        }
    }

    private void OnTriggerExit(Collider other) //?might need to call  DismissLine
    {
        if (other.gameObject.tag == "Player")
        {
            onTriggerExit.Invoke();
        }
    }
}
