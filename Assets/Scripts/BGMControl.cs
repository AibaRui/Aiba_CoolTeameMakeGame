using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMControl : MonoBehaviour
{

    [Header("最初のBGM")]
    [SerializeField] private AudioSource _firstBGM;

    [Header("２つ目のBGM")]
    [SerializeField] private AudioSource _secondBGM;

    private bool _isEndPlay = false;

    void Start()
    {

    }

    void Update()
    {
        if (!_firstBGM.isPlaying && !_isEndPlay)
        {
            _isEndPlay = true;
            _secondBGM.Play();
        }
    }

    public void EndBGM()
    {
        _secondBGM.Stop();
    }

}
