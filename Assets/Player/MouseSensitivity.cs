using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class MouseSensitivity : MonoBehaviour
{
    CinemachinePOV _camera;
    [SerializeField] CinemachineVirtualCamera _ca;

    [Header("マウス感度のSlider")]
    [SerializeField] private Slider _mouseSensivitySlider;

    public Slider MouseSensivitySlider => _mouseSensivitySlider;

    [Header("初期設定のマウス感度")]
    [SerializeField] private float _firstMouseSensivity = 100;

    [Header("マウス感度の最大値")]
    [SerializeField] private float _maxMouseSensivity = 200;

    [Header("マウス感度の最小値")]
    [SerializeField] private float _minSensitivity = 50;

    private float _nowSensivity;

    public float FirstMouseSensivity => _firstMouseSensivity;

    public float NowSensivity => _nowSensivity;

    public void FirstSetting()
    {
        _camera = _ca.GetCinemachineComponent<CinemachinePOV>();
        _mouseSensivitySlider.maxValue = _maxMouseSensivity;
        _mouseSensivitySlider.minValue = _minSensitivity;
    }

    public void ChangeSensitivity(float value)
    {
        _camera.m_HorizontalAxis.m_MaxSpeed = value;
        _camera.m_VerticalAxis.m_MaxSpeed = value;

        _mouseSensivitySlider.value = value;
        _nowSensivity = value;

    }
}
