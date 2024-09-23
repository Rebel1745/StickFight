using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreComponent : MonoBehaviour
{
    protected Core _core;

    protected virtual void Awake()
    {
        _core = transform.parent.GetComponent<Core>();

        if (!_core)
        {
            Debug.LogError("There is no core on the parent");
        }
    }
}
