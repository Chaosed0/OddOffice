using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ColliderEventer : MonoBehaviour
{
    public UnityEvent OnPlayerEntered = new UnityEvent();

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 9)
        {
            OnPlayerEntered.Invoke();
        }
    }
}
