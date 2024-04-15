using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerTimeLineSignal : MonoBehaviour
{
    [SerializeField] private PlayerControl _playerControl;

    [SerializeField] private CinemachineVirtualCamera _camera;

    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private Transform _medal;
    [SerializeField] private Transform _armPos;

    public void BigDanageJump()
    {
        _playerControl.PlayerDamage.BigDamageMoveEnd();
    }

    public void BigDamageEnd()
    {
        _playerControl.PlayerDamage.EndBigDamage();
    }

    public void BigDamageSlow()
    {
        Vector3 velo = _playerControl.Rb.velocity;
        velo.x *= 0.5f;
        velo.z *= 0.5f;
        _playerControl.Rb.velocity = velo;
    }

    public void CameraPriorityChange()
    {
        _camera.Priority = 300;
    }

    public void OnLine()
    {
        _playerControl.PlayerDamage.OnLine(true);
    }

    public void OffLine()
    {
        _playerControl.PlayerDamage.OnLine(false);
    }

    public void OnMaterialGray()
    {
        _playerControl.PlayerMaterial.ChangePlayerMaterial(PlayerMaterialType.GrayScal);
    }

    public void OffGrayMaterial()
    {
        _playerControl.PlayerMaterial.ChangePlayerMaterial(PlayerMaterialType.Nomal);
    }

}
