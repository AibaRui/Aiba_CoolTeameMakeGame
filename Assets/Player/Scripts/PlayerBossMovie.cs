using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField] private PlayerControl _playerControl;



    private bool _isEndMovie = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>ボス登場の映像が終わった</summary>
    public void EndMovie()
    {
        _playerControl.transform.position = _playerStartPos.position;
        Vector3 dir = _boss.position - _playerStartPos.position;
        dir.y = _playerAddDirY;

        _playerControl.Rb.velocity= dir*_playerAddPower;

    }


}
