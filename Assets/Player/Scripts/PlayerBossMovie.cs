using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerBossMovie : MonoBehaviour
{
    [Header("���[�r�[���Đ����邩�ǂ���")]
    [SerializeField]private bool _isPlayMovie = false;

    [Header("�v���C���[�̊J�n�n�_")]
    [SerializeField] private Transform _playerStartPos;

    [Header("�v���C���[�̉�����")]
    [SerializeField] private float _playerAddPower = 20;

    [Header("�v���C���[�̔��ˊp�xY")]
    [Range(0f, 1f)]
    [SerializeField] private float _playerAddDirY = 0.5f;

    [Header("�{�X")]
    [SerializeField] private Transform _boss;

    [Header("�v���C���[��UI�݂̂̃��C���[")]
    [SerializeField] private LayerMask _onlyLayer;

    [Header("UI�݂̂̃��C���[")]
    [SerializeField] private LayerMask _uiLayer;

    [Header("�S�Ẵ��C���[")]
    [SerializeField] private LayerMask _layerAll;

    [SerializeField] private CinemachineVirtualCamera _camera; 

    [SerializeField] private PlayerControl _playerControl;



    private bool _isEndMovie = false;

    public bool IsEndMovie => _isEndMovie;
    public bool IsPlayMovie => _isPlayMovie;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetLayerOnlyUI()
    {
        Camera.main.cullingMask = _uiLayer;
    }

    public void SetLayerPlayerAndUI()
    {
        Camera.main.cullingMask = _onlyLayer;
    }

    public void SerLayerAll()
    {
        Camera.main.cullingMask = _layerAll;
        _camera.Priority = 0;
    }


    /// <summary>���[�r�[���n�܂����炷�邱��</summary>
    public void MovieStart()
    {
        _playerControl.InputManager.IsCanInput = false;
        _playerControl.Rb.isKinematic = true;
    }

    /// <summary>�{�X�o��̉f�����I�����</summary>
    public void EndMovie()
    {
        //�v���C���[�̈ʒu����
        _playerControl.transform.position = _playerStartPos.position;

        _playerControl.Rb.isKinematic = false;

        //���x�������Ĕ�яo������
        Vector3 dir = _boss.position - _playerStartPos.position;
        dir.y = _playerAddDirY;
        _playerControl.Rb.velocity= dir.normalized*_playerAddPower;

        _playerControl.AnimControl.BossMovieJump();
        _isEndMovie =true;
    }

    public void ExitState()
    {
        _playerControl.InputManager.IsCanInput = true;
    }

}