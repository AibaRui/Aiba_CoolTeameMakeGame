using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallRunUpZip : IPlayerAction
{
    [Header("Gizmo��`�悷�邩�ǂ���")]
    [SerializeField] private bool _isGizmo = true;


    [Header("�ǂ̃��C���[")]
    [SerializeField] private LayerMask _wallLayer = default;

    [Header("����ŏ��������鑬�x")]
    [SerializeField] private float _zipSpeed = 10;

    [Header("����ŏ��������鑬�x")]
    [SerializeField] private float _addFirstSpeed = 4;

    [Header("Player�̑���")]
    [SerializeField] private Vector3 _playerfootOffset;


    [Header("�������Zip����ꏊ���m�F����Ray�̒���")]
    [SerializeField] private float _checkWallRayLong = 1;

    [Header("�������Zip����ꏊ")]
    [SerializeField] private Vector3 _upZipOffSet;

    [Header("�������Zip����ꏊ���m�F����Ray�̒���")]
    [SerializeField] private float _upZipRayLong;

    [Header("�������Zip������ɉ����Ȃ�������������")]
    [SerializeField] private Vector3 _upZipOffSetCheckNoBuild;

    [Header("�������Zip������ɉ����Ȃ�������������Ray�̒���")]
    [SerializeField] private float _upZipOffSetCheckNoBuildRayLong = 5;

    private bool _isZipToFront = false;

    /// <summary>Zip����|�C���g�̏��</summary>
    private RaycastHit _upZipHit;

    /// <summary>�ǂ��班������I��������ǂ���</summary>
    private bool _isToFarWall = false;

    /// <summary>Zip�����I�������ǂ��� </summary>
    private bool _isEndZip = false;

    /// <summary>Zip�������</summary>
    private Vector3 _zipDir;


    private float _upToFrontPosY;

    public bool IsEndZip => _isEndZip;

    public bool IsZipToFront => _isZipToFront;

    /// <summary>Zip������͂��߂̐ݒ�</summary>
    public void UpZipStart()
    {
        //bool�����Z�b�g
        _isToFarWall = false;
        //bool�����Z�b�g
        _isEndZip = false;


        //���x�����Z�b�g
        _playerControl.Rb.velocity = Vector3.zero;

        if (_isZipToFront)
        {
            _upToFrontPosY = _playerControl.PlayerT.position.y + _upZipOffSetCheckNoBuild.y;

            _playerControl.AnimControl.WallRunZipStart(true);
        }
        else
        {
            //�͂�������̂́A������ƁA�ǂ��班�������p��
            Vector3 addDir = Vector3.up + _playerControl.WallRunCheck.WallDir;

            //�ǂ��班�������悤�ɑ��x��������
            _playerControl.Rb.AddForce(addDir * _addFirstSpeed, ForceMode.Impulse);

            //LineRendrer�̐ݒ�
            _playerControl.LineRenderer.positionCount = 2;
            _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
            _playerControl.LineRenderer.SetPosition(1, _upZipHit.point);

            //�A�j���[�V�����̐ݒ�
            _playerControl.AnimControl.WallRunZipStart(false);
        }
    }

    public void DoUpToFrontZip()
    {
        //�v���C���[��ǂ̕�����������
        Quaternion _targetRotation = Quaternion.LookRotation(-_playerControl.WallRunCheck.WallDir, Vector3.up);
        Quaternion toAngle = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, Time.deltaTime * 400);
        toAngle.x = 0;
        toAngle.z = 0;

        //���݂̉�]�ƁA��]�I���Ƃ̊p�x���ׂ�
        float y = Quaternion.Angle(_targetRotation, _playerControl.PlayerT.rotation);

        //���l�܂ŉ�]���Ă��Ȃ��������]������
        if (y > 1)
        {
            _playerControl.PlayerT.rotation = toAngle;
        }


        Debug.Log("UppingToFront");
        //���x��ݒ�
        _playerControl.Rb.velocity = Vector3.up * 30;

        bool isHit = Physics.Raycast(_playerControl.PlayerT.position + _playerfootOffset, -_playerControl.WallRunCheck.WallDir, _upZipOffSetCheckNoBuildRayLong, _wallLayer);

        if (!isHit)
        {
            Debug.Log("EndFrounZip");
            //
            _isEndZip = true;

            //���x�����Z�b�g
            _playerControl.Rb.velocity = Vector3.zero;

            //�O�ɗ͂�������
            _playerControl.Rb.AddForce(-_playerControl.WallRunCheck.WallDir * 30, ForceMode.Impulse);

            _playerControl.AnimControl.WallRunZipDo(true);
        }
    }


    public void DoZip()
    {
        //�v���C���[��ǂ̕�����������
        Quaternion _targetRotation = Quaternion.LookRotation(-_playerControl.WallRunCheck.WallDir, Vector3.up);
        Quaternion toAngle = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, Time.deltaTime * 400);
        toAngle.x = 0;
        toAngle.z = 0;

        //���݂̉�]�ƁA��]�I���Ƃ̊p�x���ׂ�
        float y = Quaternion.Angle(_targetRotation, _playerControl.PlayerT.rotation);

        //���l�܂ŉ�]���Ă��Ȃ��������]������
        if (y > 1)
        {
            _playerControl.PlayerT.rotation = toAngle;
        }



        if (!_isToFarWall)
        {
            //�v���C���[����ǂ̕����܂�Ray���΂�
            bool isHit = Physics.Raycast(_playerControl.PlayerT.position, -_playerControl.WallRunCheck.WallDir, _checkWallRayLong, _wallLayer);

            //�ǂɓ�����Ȃ��Ȃ�����A���ꂽ�Ƃ������Ƃɂ���
            if (!isHit)
            {
                //���ꂽ���Ƃ�����
                _isToFarWall = true;

                //�ړ�������������߂�
                _zipDir = _upZipHit.point - _playerControl.PlayerT.position;

                //�A�j���[�V�����̐ݒ�
                _playerControl.AnimControl.WallRunZipDo(false);
            }
        }
        else
        {
            Debug.Log("Upping");

            //���x��ݒ�
            _playerControl.Rb.velocity = _zipDir.normalized * _zipSpeed + (_playerControl.WallRunCheck.WallDir * 0.5f);

            //Zip�ڕW�n�_�Ǝ����̋��������߂�
            float distance = Mathf.Abs(_upZipHit.point.y - _playerControl.PlayerT.position.y);

            //���덷���ɓ�������I��
            if (distance < 0.5f)
            {
                Debug.Log("Emd");
                _isEndZip = true;

                //LineRendrer
                _playerControl.LineRenderer.positionCount = 0;
            }
        }
    }


    public bool CheckUpZipPosition()
    {
        //�X�^�[�g�̓v���C���[�̒n�_+Zip�ڕW�n�_
        Vector3 rayStart = _playerControl.PlayerT.position + _upZipOffSet;
        //�����́A�ǂɌ�����������
        Vector3 rayDir = -_playerControl.WallRunCheck.WallDir;

        RaycastHit raycast;

        //Zip�o����Ƃ��낪���邩�T��
        bool isHit = Physics.Raycast(rayStart, rayDir, out raycast, _upZipRayLong, _wallLayer);

        //�X�^�[�g�̓v���C���[�̒n�_+Zip�ڕW�n�_
        Vector3 rayStart2 = _playerControl.PlayerT.position + _upZipOffSet;

       // bool isHitNoBuold = Physics.Raycast(rayStart2, rayDir, out raycast, _upZipOffSetCheckNoBuildRayLong, _wallLayer);

        if (isHit)
        {
            //�������Ă����ꍇ�̂ݏ���n��
            _upZipHit = raycast;
        }

        if (_playerControl.WallRun.MoveDir == WallRun.MoveDirection.Up)
        {
            if (isHit)
            {
                _isZipToFront = false;
                return true;
            }
            else
            {
                _isZipToFront = true;
                return true;
            }
        }
        else
        {
            return false;
        }
    }


    public void OnDrawGizmos(Transform player)
    {
        if (_isGizmo)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawRay(player.position + _upZipOffSet, player.forward * 10);

            Gizmos.color = Color.red;

            Gizmos.DrawRay(player.position + _playerfootOffset, player.forward * _upZipOffSetCheckNoBuildRayLong);
        }
    }


}