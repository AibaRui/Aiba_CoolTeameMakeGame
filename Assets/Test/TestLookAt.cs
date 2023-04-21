using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestLookAt : MonoBehaviour
{
    [SerializeField] private Transform _target;



    void Update()
    {
        transform.up = _target.position;
        Quaternion r = transform.rotation;
        r.x = 0;
        r.y = 0;
        transform.up = Vector3.up;
        transform.rotation = r;

    }
}
