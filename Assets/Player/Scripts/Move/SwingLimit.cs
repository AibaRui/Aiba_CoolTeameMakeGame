using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SwingLimit
{
    [Header("Swing‚ðŠJŽn‚Å‚«‚é‚Ü‚Å‚ÌŽžŠÔ")]
    [SerializeField] private float _swingStartTime = 1f;

    private float _countSwingStartTime = 0;

    private bool _isCanSwing = false;

    private PlayerControl _playerControl;

    public bool IsCanSwing => _isCanSwing;

    public void Init(PlayerControl playerControl)
    {
        _playerControl = playerControl;
    }


    public void SetSwingLimit(float time)
    {
        _isCanSwing = false;
        _countSwingStartTime = 0;
    }


    public void CountSwingLimitTime()
    {
        if (_isCanSwing) return;

        _countSwingStartTime += Time.deltaTime;

        if (_swingStartTime > _countSwingStartTime)
        {
            _isCanSwing = true;
            _countSwingStartTime = 0;
        }
    }

}
