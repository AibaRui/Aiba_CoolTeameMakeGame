using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallRun : IPlayerAction
{
    [Header("�������x")]
    [SerializeField] private float _moveSpeed = 4;

    [Header("�i�s�����̂̉�]�̑���")]
    [SerializeField] private float _rotateMoveDirSpeed = 50;

    [Header("���ړ����̃v���C���[�̉�]�̑���")]
    [SerializeField] private float _rotateSpeed = 200;

    [Header("�ړ����̃v���C���[�̉�]�̑���")]
    [SerializeField] private float _moveRotateSpeed = 20;

    [Header("�Ō�̃W�����v�̋���_�ړ���")]
    [SerializeField] private float _lastJumpPower = 10;

    [Header("�Ō�̃W�����v�̋���_�~�܂��Ă鎞")]
    [SerializeField] private float _lastJumpPowerNoMove = 5;

    private float _noMoveTimeCount = 0;

    private bool _isEndNoMove = false;

    private Vector3 _useMoveDir;

    private MoveDirection _moveDirection = MoveDirection.Up;


    public float MoveSpeed => _moveSpeed;

    public bool IsEndNoMove => _isEndNoMove;
    public Vector3 UseMoveDir => _useMoveDir;
    public MoveDirection MoveDir => _moveDirection;

    public enum MoveDirection
    {
        Up,
        Right,
        Left,

    }

    public void SetMoveDir(MoveDirection moveDirection)
    {
        _moveDirection = moveDirection;

        //�O�ς��g���A�i�s���������
        Vector3 wallForward = _playerControl.WallRunCheck.WallCrossRight;

        if (moveDirection == MoveDirection.Up)
        {
            _useMoveDir = Vector3.up;
        }
        else if (moveDirection == MoveDirection.Right)
        {
            _useMoveDir = wallForward;
        }
        else
        {
            _useMoveDir = -wallForward;
        }
    }

    public void CountNoMove()
    {
        if (_isEndNoMove)
        {
            float h = _playerControl.InputManager.HorizontalInput;
            float v = _playerControl.InputManager.VerticalInput;

            if (h != 0 || v > 0)
            {
                _isEndNoMove = false;
                _noMoveTimeCount = 0;
            }

            _noMoveTimeCount += Time.deltaTime;

            if (_noMoveTimeCount > 1.5f)
            {
                _isEndNoMove = false;
                _playerControl.Rb.velocity = Vector3.zero;
                _noMoveTimeCount = 0;
            }
        }
    }

    public void SetNoMove(bool set)
    {
        _isEndNoMove = set;

        if (!set)
        {
            _noMoveTimeCount = 0;
        }
    }


    /// <summary>�O�ς����߂�֐�</summary>
    /// <param name="nomal"></param>
    /// <param name="foward"></param>
    /// <returns></returns>
    public Vector3 GetCross(Vector3 nomal, Vector3 foward)
    {
        //�@�������
        Vector3 wallNomal = nomal;
        //�O�ς��g���A�i�s���������
        Vector3 wallForward = Vector3.Cross(wallNomal, _playerControl.PlayerT.up);

        //�v���X�ƃ}�C�i�X�̊O�σx�N�g���Ǝ��g�̌����Ă���������ׂ�B
        //�߂�����i�ޕ����Ƃ���
        if ((foward - wallForward).magnitude > (foward - -wallForward).magnitude)
        {
            wallForward = -wallForward;
        }
        return wallForward;
    }


    /// <summary>�~�܂��Ă���ۂ̃v���C���[�̌��������߂�</summary>
    public void MidleDir()
    {



    }

    /// <summary>�ǂ̕����ɗ͂�������</summary>
    public void AddWall()
    {
        if (_playerControl.WallRunCheck.Hit.collider == null)
        {
            return;
        }   //null�`�F�b�N

        //�ǂƕ��t�̃x�N�g���B�O�ςŋ��߂�
        Vector3 wallForward = _playerControl.WallRunCheck.WallCrossRight;

        //�ǂƐ����̃x�N�g�����Ƃ�
        Vector3 dir = Vector3.Cross(wallForward, Vector3.up);

        //dir�̌������ǂƔ��΂̕����������ۂɂ́A���]������B
        if ((_playerControl.WallRunCheck.Hit.normal - dir).magnitude > (_playerControl.WallRunCheck.Hit.normal - -dir).magnitude)
        {
            dir = -dir;
        }

        Debug.DrawRay(_playerControl.PlayerT.position, dir, Color.blue);


        _playerControl.Rb.AddForce(-dir * 5);

        if (_playerControl.Rb.velocity.y < 0)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
        }
    }


    /// <summary>�ړ������ɃL��������]������</summary>
    public void CharactorRotateToMoveDirection(float hInput)
    {
        //�O�ς��g���A�i�s���������
        Vector3 wallCrossRight = _playerControl.WallRunCheck.WallCrossRight;

        //��]����������
        Quaternion _targetRotation = default;

        if (_moveDirection == MoveDirection.Up)
        {
            if (hInput > 0.5f)
            {
                _targetRotation = Quaternion.LookRotation(wallCrossRight, Vector3.up);
            }
            else if (hInput < -0.5f)
            {
                _targetRotation = Quaternion.LookRotation(-wallCrossRight, Vector3.up);
            }
            else
            {
                Vector3 dir = -_playerControl.WallRunCheck.WallDir;
                _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            }

        }
        else if (_moveDirection == MoveDirection.Right)
        {
            if (hInput < -0.5f)
            {
                Vector3 dir = -_playerControl.WallRunCheck.WallDir;
                _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            }
            else
            {
                _targetRotation = Quaternion.LookRotation(wallCrossRight, Vector3.up);
            }
        }
        else if (_moveDirection == MoveDirection.Left)
        {
            if (hInput > 0.5f)
            {
                Vector3 dir = -_playerControl.WallRunCheck.WallDir;
                _targetRotation = Quaternion.LookRotation(dir, Vector3.up);
            }
            else
            {
                _targetRotation = Quaternion.LookRotation(-wallCrossRight, Vector3.up);
            }
        }

        //��]������
        Quaternion toAngle = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, Time.deltaTime * _moveRotateSpeed);
        toAngle.x = 0;
        toAngle.z = 0;



        //���݂̉�]�ƁA��]�I���Ƃ̊p�x���ׂ�
        float y = Quaternion.Angle(_targetRotation, _playerControl.PlayerT.rotation);

        if (y > 1)
        {
            _playerControl.PlayerT.rotation = toAngle;
        }   //�p�x��1�x�ȓ��ɂ܂Ŏ��܂�����I��

        Quaternion upRotation = Quaternion.LookRotation(-_playerControl.WallRunCheck.WallDir, Vector3.up);
        Quaternion rightRotation = Quaternion.LookRotation(wallCrossRight, Vector3.up);
        Quaternion leftRotation = Quaternion.LookRotation(-wallCrossRight, Vector3.up);

        float angleUp = Quaternion.Angle(_playerControl.PlayerT.rotation, upRotation);
        float angleRight = Quaternion.Angle(_playerControl.PlayerT.rotation, rightRotation);
        float angleLeft = Quaternion.Angle(_playerControl.PlayerT.rotation, leftRotation);

        if (angleUp < 70)
        {
            _moveDirection = MoveDirection.Up;
            _playerControl.AnimControl.WallRunUpSet(true);
        }
        else if (angleRight < 40)
        {
            _moveDirection = MoveDirection.Right;
            _playerControl.AnimControl.WallRunUpSet(false);
            _playerControl.AnimControl.SetWallRunHitRight(true);
        }
        else if (angleLeft < 70)
        {
            _moveDirection = MoveDirection.Left;
            _playerControl.AnimControl.WallRunUpSet(false);
            _playerControl.AnimControl.SetWallRunHitRight(false);
        }

        // Debug.Log(_moveDirection);
    }


    public void WallMove()
    {
        //�ǂƕ��t�̃x�N�g���B�O�ςŋ��߂�
        Vector3 wallForward = Vector3.Cross(_playerControl.WallRunCheck.Hit.normal, Vector3.up);

        //�ǂƐ����̃x�N�g�����Ƃ�
        Vector3 dir = Vector3.Cross(wallForward, Vector3.up);

        if ((_playerControl.WallRunCheck.Hit.normal - dir).magnitude > (_playerControl.WallRunCheck.Hit.normal - -dir).magnitude)
        {
            dir = -dir;
        }

        float h = _playerControl.InputManager.HorizontalInput;


        Vector3 targetMoveDir = default;

        if (_moveDirection == MoveDirection.Up)
        {
            if (h > 0.2f)
            {
                targetMoveDir = wallForward;
            }
            else if (h < -0.2f)
            {
                targetMoveDir = -wallForward;
            }
            else
            {
                targetMoveDir = Vector3.up;
            }
        }
        else if (_moveDirection == MoveDirection.Right)
        {
            if (h < 0)
            {
                targetMoveDir = Vector3.up;
            }
            else
            {
                targetMoveDir = wallForward;
            }
        }
        else if (_moveDirection == MoveDirection.Left)
        {
            if (h > 0f)
            {
                targetMoveDir = Vector3.up;
            }
            else
            {
                targetMoveDir = -wallForward;
            }
        }

        //Player�̉�]
        CharactorRotateToMoveDirection(h);

        //�ړ������̉�]�̑���
        float rotationAngle = Time.deltaTime * _rotateMoveDirSpeed;


        //_useMoveDir�����ݎg�p���Ă���ړ�����
        //moveDir���ڕW�̈ړ�����

        //�w�肳�ꂽ��] rotation �ƍő�p�x rotationAngle ���l�����āA�x�N�g�� _useMoveDir ����]�����鏈�����s���Ă���
        if ((_useMoveDir != targetMoveDir))
        {
            //���݂̃x�N�g��(_useMoveDir)����A�ڕW�̃x�N�g��(moveDir)�ցA��]���邽�߂ɕK�v�ȃN�H�[�^�j�I�����v�Z�B
            Quaternion rotation = Quaternion.FromToRotation(_useMoveDir, targetMoveDir);

            _useMoveDir = Quaternion.RotateTowards(Quaternion.identity, rotation, rotationAngle) * _useMoveDir;


            //Quaternion r = Quaternion.Euler(_useMoveDir);

            //_useMoveDir = Quaternion.RotateTowards(r, rotation, rotationAngle) * _useMoveDir;
        }

        //�ǂ̕��։����񂹂��(�ǂɂ��������邽��)
        float addWallPower = 1;

        //��苗������Ă����狭���͂����킦��
        if (Vector3.Distance(_playerControl.PlayerT.position, _playerControl.WallRunCheck.Hit.point) > 0.7f)
        {
            addWallPower = 10;
        }

        _playerControl.Rb.velocity = (_useMoveDir * _moveSpeed) + -dir * addWallPower;

        if (_playerControl.Rb.velocity.y < 0)
        {
            _playerControl.Rb.velocity = new Vector3(_playerControl.Rb.velocity.x, 0, _playerControl.Rb.velocity.z);
        }
        //  Debug.DrawRay(_playerControl.PlayerT.position, _useMoveDir * 50, Color.green);
        //Debug.DrawRay(_playerControl.PlayerT.position, _playerControl.Rb.velocity.normalized * 40,Color.white);
    }

    public void LastJump(bool isMove)
    {
        if (isMove)
        {
            if (_playerControl.WallRun.MoveDir == MoveDirection.Up)
            {
                _playerControl.Rb.AddForce(Vector3.up * 15, ForceMode.Impulse);
            }
            else
            {
                Vector3 dir = Vector3.up + _playerControl.WallRunCheck.Hit.normal;
                _playerControl.Rb.AddForce(dir * _lastJumpPower, ForceMode.Impulse);
            }
        }
        else
        {
            _playerControl.Rb.AddForce(_playerControl.WallRunCheck.Hit.normal * _lastJumpPowerNoMove, ForceMode.Impulse);
        }

        _playerControl.ModelT.rotation = _playerControl.PlayerT.rotation;
    }

}