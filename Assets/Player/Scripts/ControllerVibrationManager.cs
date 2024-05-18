using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class ControllerVibrationManager : MonoBehaviour
{
    [Header("毎フレームどれくらい速度を上げるか")]
    [SerializeField] private float _addSpeed = 0.01f;

    [Header("最大パワー")]
    [SerializeField] private float _maxPower = 0.3f;

    [Header("最小パワー")]
    [SerializeField] private float _minPower = 0.1f;

    private Gamepad gamepad;

    private float _nowPower = 0;

    private void Start()
    {
        gamepad = Gamepad.current;
    }

    private void OnDisable()
    {
        gamepad.SetMotorSpeeds(0f, 0f);
    }

    public void StartVibration(VivrationPower power)
    {
        if (gamepad != null)
        {
            if (power == VivrationPower.Swing)
            {
                gamepad.SetMotorSpeeds(0.2f, 0.2f);
            }
            else if (power == VivrationPower.SetUp)
            {
                gamepad.SetMotorSpeeds(0.4f, 0.4f);
            }
            else
            {
                gamepad.SetMotorSpeeds(0.4f, 0.4f);
            }

        }
    }

    public void DoVibration()
    {
        if (gamepad == null) return;

        if (_nowPower <= _maxPower)
        {
            _nowPower += Time.deltaTime;
        }   //Maxまでいって無かったら追加
        else
        {
            return;
        }

        gamepad.SetMotorSpeeds(_nowPower, _nowPower);
    }


    public void StopVibration()
    {
        if (gamepad == null) return;

        if (gamepad != null)
        {
            _nowPower = _minPower;
            gamepad.SetMotorSpeeds(0f, 0f);
        }
    }



}

public enum VivrationPower
{
    Swing,
    SetUp,
    Power,
}