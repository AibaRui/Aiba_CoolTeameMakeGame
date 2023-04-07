using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraControl : MonoBehaviour
{


    public enum InputChoice
    {
        KeyboardAndMouse, Controller,
    }

    [SerializeField] Transform follow;
    [SerializeField] Transform lookAt;

    public InputChoice inputChoice;
    // public InvertSettings keyboardAndMouseInvertSettings;
    // public InvertSettings controllerInvertSettings;
    public bool allowRuntimeCameraSettingsChanges;


    [SerializeField] private CinemachineVirtualCamera keyboardAndMouseCamera;
    [SerializeField] private CinemachineVirtualCamera controllerCamera;

    void Awake()
    {
        UpdateCameraSettings();
    }

    void Update()
    {
        if (allowRuntimeCameraSettingsChanges)
        {
            UpdateCameraSettings();
        }
    }

    void UpdateCameraSettings()
    {
        keyboardAndMouseCamera.Follow = follow;
        keyboardAndMouseCamera.LookAt = lookAt;
        // keyboardAndMouseCamera.m_XAxis.m_InvertInput = keyboardAndMouseInvertSettings.invertX;
        // keyboardAndMouseCamera.m_YAxis.m_InvertInput = keyboardAndMouseInvertSettings.invertY;

        //  controllerCamera.m_XAxis.m_InvertInput = controllerInvertSettings.invertX;
        //  controllerCamera.m_YAxis.m_InvertInput = controllerInvertSettings.invertY;
        controllerCamera.Follow = follow;
        controllerCamera.LookAt = lookAt;

        keyboardAndMouseCamera.Priority = inputChoice == InputChoice.KeyboardAndMouse ? 1 : 0;
        controllerCamera.Priority = inputChoice == InputChoice.Controller ? 1 : 0;
    }
} 

