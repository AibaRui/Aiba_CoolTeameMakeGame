using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOverrayChange : MonoBehaviour
{
    [Header("変更するレイヤー")]
    [SerializeField] private LayerMask _layerMask;

    [Header("元のレイヤー")]
    [SerializeField] private LayerMask _defultLayerMask;

    [SerializeField] private GameObject _imge;


    [Header("開始")]
    [SerializeField] private float _timeT = 0.7f;

    private float _time;

    private bool _wait;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.unscaledDeltaTime;

        if (!_wait)
        {
            if (_time >_timeT)
            {
                _wait = true;
                _time = 0;
                Camera.main.cullingMask = _layerMask;
                _imge.SetActive(true);
                Time.timeScale = 0f;
            }
        }
        else
        {
            if (_time > 0.9f)
            {
                Camera.main.cullingMask = _defultLayerMask;
                _imge.SetActive(false);
                Time.timeScale = 1f;
            }
        }



    }
}
