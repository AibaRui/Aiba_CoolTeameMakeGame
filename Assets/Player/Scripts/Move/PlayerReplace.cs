using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlayerReplace : MonoBehaviour
{
    [Header("ã¸ŠJŽn‚Ü‚Å‚ÌŽžŠÔ")]
    [SerializeField] private float _waitTime = 1;

    [Header("ã¸‘¬“x")]
    [SerializeField] private float _removeSpeed = 40;

    [Header("Šî€‚Ì‚‚³")]
    [SerializeField] private float _resetYPositin = 200;

    [SerializeField] private Transform _upMedalPos;

    [SerializeField] private PlayableDirector _upMovie;

    [SerializeField] private PlayerControl _playerControl;

    private float _countWaitTime = 0;

    private bool _isUpping = false;
    private bool _isStart = false;
    private bool _isRemove = false;


    public bool IsRemove => _isRemove;

    private ReplceType _replceType = ReplceType.Up;


    public void StartReplace()
    {
        _isStart = true;

        _isUpping = false;
        _countWaitTime = 0;
        _playerControl.InputManager.IsCanInput = false;

        _playerControl.AnimControl.RePlace1(true);

        _playerControl.Rb.velocity = new Vector3(0, -1, 0);

        if (_replceType == ReplceType.Up)
        {
            _upMovie.Play();
        }

    }


    public void Remove()
    {
        if (_replceType == ReplceType.Up)
        {
            if (!_isUpping)
            {
                _countWaitTime += Time.deltaTime;
                if (_countWaitTime > _waitTime)
                {
                    _isUpping = true;
                }
                else
                {
                    return;
                }
            }


            _playerControl.Rb.velocity = Vector3.up * _removeSpeed;

            if (_playerControl.transform.position.y > _resetYPositin)
            {
                RemoveEnd();
            }
        }
    }

    public void ReplaceLateUpddata()
    {
        if (_replceType == ReplceType.Up)
        {
            if (_isUpping)
            {
                _playerControl.LineRenderer.positionCount = 2;
                _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
                _playerControl.LineRenderer.SetPosition(1, _upMedalPos.position);
            }
        }
    }

    public void RemoveEnd()
    {
        _playerControl.InputManager.IsCanInput = true;
        _playerControl.AnimControl.RePlace1(false);
        _isRemove = false;
        _isStart = false;

        _playerControl.LineRenderer.positionCount = 0;
    }

    public void EnterReplaceZone(ReplceType replceType)
    {
        if (_isRemove) return;
        _replceType = replceType;

        _isRemove = true;
    }


    public void ExitReplaceZone()
    {
        if (!_isStart)
            _isRemove = false;
    }

}

public enum ReplceType
{
    Up,
    Center,
}