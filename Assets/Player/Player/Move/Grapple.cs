using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[System.Serializable]
public class Grapple : IPlayerAction
{
    [Header("カメラのPriority")]
    [SerializeField] private int _cameraPriority = 30;

    [Header("速度制限")]
    [SerializeField] private Vector3 _limitSpeed = new Vector3(20, 20, 20);

    [Header("ワイヤーの長さ")]
    [SerializeField] private float _maxWireLong = 30f;

    [Header("レイヤー")]
    [SerializeField] private LayerMask _layer = default;

    [Header("Grapple中断距離")]
    [SerializeField] private float _stopDistance = 30f;

    [Header("ワイヤーの巻き取り速度")]
    [Range(0, 1)]
    [SerializeField] private float _shrinkWireSpeed = 0.9f;

    [Header("移動速度")]
    [SerializeField] private float _moveSpeed = 4;

    [Header("プレイヤーの回転速度")]
    [SerializeField] private float _turnSpeed = 100;

    [Header("バネの強さ")]
    [SerializeField] private float _springPower = 4.5f;

    [Header("ダンパ-の強さ")]
    [SerializeField] private float _damperPower = 7;

    [Header("ダンパ-の強さ")]
    [SerializeField] private float _massScale = 4.5f;

    private RaycastHit _hit;

    /// <summary>Swingの速度制限</summary>
    public void SetSpeedGrapple()
    {
        _playerControl.VelocityLimit.SetLimit(_limitSpeed.x, _limitSpeed.y, _limitSpeed.z);
    }

    /// <summary>グラップルのワイヤーの着地点を探す</summary>
    /// <returns>ワイヤーがHitしかどうか</returns>
    public bool SearchGrapplePoint()
    {
        //構えの時間が終わったらrRayを飛ばし始める
        if (_playerControl.SetUp.IsEndCameraTransition)
        {
            //線を引く位置を決める
            _playerControl.LineRenderer.positionCount = 2;
            _playerControl.LineRenderer.SetPosition(0, _playerControl.PlayerT.position);
            _playerControl.LineRenderer.SetPosition(1, _playerControl.PlayerT.position + Camera.main.transform.forward * _maxWireLong);

            bool rayHit;

            rayHit = Physics.Raycast(_playerControl.PlayerT.position, _playerControl.CameraGrapple.transform.forward, out _hit, _maxWireLong, _layer);

            return rayHit;
        }
        else
        {
            return false;
        }
    }


    /// <summary>ワイヤーを描写する</summary>
    public void DrawLope()
    {
        if (_playerControl.Joint == null)
        {
            return;
        }

        //線を引く位置を決める
        _playerControl.LineRenderer.positionCount = 2;
        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, _hit.point);
    }

    /// <summary>スウィング中止</summary>
    public void StopSwing()
    {
        //Jointを消す
        _playerControl.DestroyJoint();

        _playerControl.Rb.useGravity = true;

        //回転を加える
        _playerControl.ModelT.rotation = _playerControl.PlayerT.rotation;
    }

    /// <summary>ワイヤー着地点と自分のポジションを計算
    /// 一定距離まで近づいたらグラップル終了</summary>
    /// <returns>一定距離まで近づいたかどうか</returns>
    public void CheckDiestance()
    {
        float dis = Vector3.Distance(_playerControl.PlayerT.position, _hit.point);

        //if (dis < _stopDistance)
        //{

        //}   //一定距離まで近づいたら終了
        //else
        //{
        _playerControl.Joint.maxDistance *= _shrinkWireSpeed;
        _playerControl.Joint.minDistance *= _shrinkWireSpeed;
        // }//一定距離まで近づいてなかったらワイヤーを縮ませる

    }

    /// <summary>スウィングの初期設定</summary>
    public void GrappleSetting()
    {
        //重力を無くす
        _playerControl.Rb.useGravity = false;

        //swing/grappleの設定クラスに自身のjointを渡す
        _playerControl.Joint = _playerControl.gameObject.AddComponent<SpringJoint>();

        //Anchor(jointをつけているオブジェクトのローカル座標  ////例)自分についてる命綱の位置)
        //connectedAnchor(アンカーのついてる点のワールド座標　////例)アンカーの先。バンジージャンプの橋の、支えているところ)

        //autoConfigureConnectedAnchorはジョイントをつけた時はOnになっており、AnchorとconnectedAnchor(アンカーの接地点)が同じ
        //つまり自分の居る位置にアンカーを刺してそこでぶら下がっている状態。なのでオフにする

        _playerControl.Joint.autoConfigureConnectedAnchor = false;
        _playerControl.Joint.connectedAnchor = _hit.point;

        //自分とアンカーの位置の間(ジョイント)の長さ。
        float distanceFromPoint = Vector3.Distance(_playerControl.transform.position, _hit.point);

        //ジョイントの長さを変更(*1だとアンカーを指しても長さが縮まらないため、すぐに浮かない)
        //強制的に短くする事で引っ張られる事になる

        _playerControl.Joint.maxDistance = distanceFromPoint * 0.8f;
        _playerControl.Joint.minDistance = distanceFromPoint * 0.2f;

        //ばねの強さ(のび縮みのしやすさ)
        _playerControl.Joint.spring = _springPower;

        //(springの減る量)バネがビヨーンと伸びるのを繰り返してから動かなくなるまでの時間。値が多いほど短くなる
        _playerControl.Joint.damper = _damperPower;

        //質量
        _playerControl.Joint.massScale = _massScale;
    }

    public void GrappleMove()
    {
        //入力を受け取る
        float h = _playerControl.InputManager.HorizontalInput;
        float v = _playerControl.InputManager.VerticalInput;

        //カメラの正面を基準とした回転を計算
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);
        //回数を考慮したベクトル
        Vector3 velo = horizontalRotation * new Vector3(h, 0, v).normalized;

        //速度を加える
        _playerControl.Rb.AddForce(velo * _moveSpeed);


        //回転を加える
        _playerControl.ModelT.LookAt(_hit.point);
    }


}
