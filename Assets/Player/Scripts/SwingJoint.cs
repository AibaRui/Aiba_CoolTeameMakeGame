using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Swing時のワイヤーの描写に関する処理をまとめたクラス</summary>
[System.Serializable]
public class SwingJoint
{
    [Header("[======Jointの設定======]")]
    [Header("Test用、固定")]
    [SerializeField] private Vector3 _swingHitPos = new Vector3(0, 30, 25.5f);
    [Header("バネの強さ")]
    [SerializeField] private float _springPower = 1f;
    [Header("ダンパ-の強さ")]
    [SerializeField] private float _damperPower = 1;
    [Header("ダンパ-の強さ")]
    [SerializeField] private float _massScale = 1;

    [Header("[=====ワイヤーの描画設定=====]")]
    [Header("ワイヤーを描画するまでの時間")]
    [SerializeField] private float _dorwWireTime = 0.4f;
    [Header("ワイヤーの描画速度")]
    [SerializeField] private float _drowWireSpeed = 1;

    private float _setWireLongPercent;

    /// <summary>ワイヤーを描画するかどうか</summary>
    private bool _isDrowWire;

    /// <summary>ワイヤーを描画するまでの時間を計算する</summary>
    private float _countDrowWireTime = 0;

    private Vector3 _loapPoint;

    private PlayerControl _playerControl;

    private Swing _swing;

    public void Init(Swing swing, PlayerControl playerControl)
    {
        _swing = swing;
        _playerControl = playerControl;
    }

    /// <summary>
    /// Swing開始時のJointの設定をする
    /// </summary>
    /// <param name="hitPoint">ワイヤーの着地点</param>
    /// <param name="distanceFromPoint">マズルからワイヤーの着地点までの長さ</param>
    public void SetStarSwingtJoint(Vector3 hitPoint, float distanceFromPoint,Vector3 loapPoint)
    {
        _loapPoint = loapPoint;

        //swing/grappleの設定クラスに自身のjointを渡す
        _playerControl.Joint = _playerControl.gameObject.AddComponent<SpringJoint>();

        //Anchor(jointをつけているオブジェクトのローカル座標  ////例)自分についてる命綱の位置)
        //connectedAnchor(アンカーのついてる点のワールド座標　////例)アンカーの先。バンジージャンプの橋の、支えているところ)
        //autoConfigureConnectedAnchorはジョイントをつけた時はOnになっており、AnchorとconnectedAnchor(アンカーの接地点)が同じ
        //つまり自分の居る位置にアンカーを刺してそこでぶら下がっている状態。なのでオフにする
        _playerControl.Joint.autoConfigureConnectedAnchor = false;

        _playerControl.Joint.connectedAnchor = hitPoint;

        _playerControl.Joint.maxDistance = distanceFromPoint * 1f;

        _playerControl.Joint.minDistance = distanceFromPoint * 0.6f;


        //ばねの強さ(のび縮みのしやすさ)
        _playerControl.Joint.spring = _springPower;
        //(springの減る量)バネがビヨーンと伸びるのを繰り返してから動かなくなるまでの時間。値が多いほど短くなる
        _playerControl.Joint.damper = _damperPower;
        //質量
        _playerControl.Joint.massScale = _massScale;
    }

    /// <summary>ワイヤーを描画するまでの時間を計算する</summary>
    public void CountDrowWireTime()
    {
        if (_isDrowWire) return;

        _countDrowWireTime += Time.deltaTime;

        if (_countDrowWireTime > _dorwWireTime)
        {
            _isDrowWire = true;
        }
    }

    public void StopJoint()
    {
        //LineRendrerを消す
        _playerControl.LineRenderer.positionCount = 0;

        //Jointを消す
        _playerControl.DestroyJoint();

        _countDrowWireTime = 0;

        _isDrowWire = false;
    }

    /// <summary>ワイヤーを描画する</summary>
    public void DrowWire()
    {
        if (_playerControl.Joint == null || !_isDrowWire)
        {
            return;
        }

        _setWireLongPercent += _drowWireSpeed * Time.deltaTime;
        if (_setWireLongPercent > 1)
        {
            _setWireLongPercent = 1;
        }

        float step = Vector3.Distance(_playerControl.Hads.position, _loapPoint) * _setWireLongPercent;
        Vector3 setPoint = Vector3.MoveTowards(_playerControl.Hads.position, _loapPoint, step);

        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, setPoint);
    }

    /// <summary>ワイヤーを描画する前の初期設定</summary>
    public void FirstSettingDrawLope()
    {
        if (_playerControl.Joint == null) return;

        _setWireLongPercent = 0;
        _playerControl.LineRenderer.positionCount = 2;
        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, _playerControl.Hads.position);
    }


    public void SetJointDistance(float max,float min)
    {
        _playerControl.Joint.maxDistance = max;
        _playerControl.Joint.minDistance = min;
    }

}
