using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootTask : MonoBehaviour
{
    [SerializeField] private TaskManager _manager;

    public void TaskClear()
    {
        _manager.EndRootTask();
    }

}
