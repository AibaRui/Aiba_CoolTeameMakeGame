using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ZipMove
{
    [Header("�O���ɉ����������")]
    [SerializeField] private Vector3 _frontZipDir = new Vector3(0, 0.1f, 1);

    [Header("1��ڂɑO���ɉ��������")]
    [SerializeField] private float _frontZipSpeedFirst = 15;

    [Header("1��ځA�n�ʂɋ߂��Ƃ��ɑO���ɉ��������")]
    [SerializeField] private float _frontZipSpeedFirstNearGround = 5;

    [Header("2��ڂɑO���ɉ��������")]
    [SerializeField] private float _frontZipSpeedSecond = 2;
    private Quaternion targetRotation;

    private PlayerControl _playerControl;

    private Zip _zip;


    public void Init(Zip zip, PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _zip = zip;
    }

    public void ZipFirstSetting()
    {

    }

    /// <summary>Zip���̑��x</summary>
    public void ZipAddVelocity(int count)
    {
        //�J������Y���̊p�x
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //�J�����̐��ʂ̃x�N�g����ς���
        Vector3 dir = horizontalRotation * new Vector3(_frontZipDir.x, _frontZipDir.y, _frontZipDir.z).normalized;

        if (count == 0)
        {
            if (_playerControl.GroundCheck.IsHitNearGround())
            {
                _playerControl.Rb.velocity = (dir * _frontZipSpeedFirstNearGround);
            }
            else
            {
                _playerControl.Rb.velocity = (dir * _frontZipSpeedFirst);
            }

        } //����́A����
        else
        {
            _playerControl.Rb.AddForce(dir * _frontZipSpeedSecond, ForceMode.Impulse);
        }   //2��ڈڍs�͒x��

    }


    public void SetRotation()
    {
        //�����̃x�N�g���ݒ�
        Vector3 dir = Camera.main.transform.localEulerAngles;
        dir.x = 0;
        dir.z = 0;
        targetRotation = Quaternion.Euler(dir);
    }

    /// <summary>Zip���̃v���C���[�̊p�x�ݒ�</summary>
    public void ZipSetPlayerRotation()
    {
        var rotationSpeed = 300 * Time.deltaTime;

        if (Mathf.Abs(_playerControl.PlayerT.rotation.y - targetRotation.y) > 0.1f)
        {
            Quaternion setRotation = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, targetRotation, rotationSpeed);
            setRotation.x = 0;
            setRotation.z = 0;
            _playerControl.PlayerT.rotation = setRotation;
        }
    }

}