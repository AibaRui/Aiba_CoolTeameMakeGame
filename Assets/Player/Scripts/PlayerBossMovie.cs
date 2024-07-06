using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerBossMovie : MonoBehaviour
{
    [Header("ムービーを再生するかどうか")]
    [SerializeField]private bool _isPlayMovie = false;

    [Header("プレイヤーの開始地点")]
    [SerializeField] private Transform _playerStartPos;

    [Header("プレイヤーの加速力")]
    [SerializeField] private float _playerAddPower = 20;

    [Header("プレイヤーの発射角度Y")]
    [Range(0f, 1f)]
    [SerializeField] private float _playerAddDirY = 0.5f;

    [Header("ボス")]
    [SerializeField] private Transform _boss;
    [SerializeField] private BossControl _bossControl;


    [Header("プレイヤーとUIのみのレイヤー")]
    [SerializeField] private LayerMask _onlyLayer;

    [Header("UIのみのレイヤー")]
    [SerializeField] private LayerMask _uiLayer;

    [Header("全てのレイヤー")]
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


    /// <summary>ムービーが始まったらすること</summary>
    public void MovieStart()
    {
        _playerControl.InputManager.IsCanInput = false;
        _playerControl.Rb.isKinematic = true;
        _bossControl.IsMovie = true;
    }

    /// <summary>ボス登場の映像が終わった</summary>
    public void EndMovie()
    {
        //プレイヤーの位置調整
        _playerControl.transform.position = _playerStartPos.position;

        _playerControl.Rb.isKinematic = false;

        //速度を加えて飛び出させる
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
