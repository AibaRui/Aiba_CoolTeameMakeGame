using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WallRunUpZip : IPlayerAction
{
    [Header("Gizmoを描画するかどうか")]
    [SerializeField] private bool _isGizmo = true;


    [Header("壁のレイヤー")]
    [SerializeField] private LayerMask _wallLayer = default;

    [Header("初回で少し加える速度")]
    [SerializeField] private float _zipSpeed = 10;

    [Header("初回で少し加える速度")]
    [SerializeField] private float _addFirstSpeed = 4;

    [Header("Playerの足元")]
    [SerializeField] private Vector3 _playerfootOffset;


    [Header("上方向のZipする場所を確認するRayの長さ")]
    [SerializeField] private float _checkWallRayLong = 1;

    [Header("上方向のZipする場所")]
    [SerializeField] private Vector3 _upZipOffSet;

    [Header("上方向のZipする場所を確認するRayの長さ")]
    [SerializeField] private float _upZipRayLong;

    [Header("上方向のZipした先に何かないかを検査する")]
    [SerializeField] private Vector3 _upZipOffSetCheckNoBuild;

    [Header("上方向のZipした先に何かないかを検査するRayの長さ")]
    [SerializeField] private float _upZipOffSetCheckNoBuildRayLong = 5;

    private float _countWaitTime = 0;

    private bool _isEndUppinng = false;

    private bool _isZipToFront = false;

    /// <summary>Zipするポイントの情報</summary>
    private RaycastHit _upZipHit;

    /// <summary>壁から少し離れ終わったかどうか</summary>
    private bool _isToFarWall = false;

    /// <summary>Zipをし終えたかどうか </summary>
    private bool _isEndZip = false;

    /// <summary>Zipする方向</summary>
    private Vector3 _zipDir;

    private Vector3 _wireHitPoint = default;


    private float _upToFrontPosY;

    public bool IsEndZip => _isEndZip;

    public bool IsZipToFront => _isZipToFront;

    /// <summary>Zipをするはじめの設定</summary>
    public void UpZipStart()
    {
        //boolをリセット
        _isToFarWall = false;
        //boolをリセット
        _isEndZip = false;

        _countWaitTime = 0;
        _isEndUppinng = false;

        //速度をリセット
        _playerControl.Rb.velocity = Vector3.zero;

        if (_isZipToFront)
        {
            _upToFrontPosY = _playerControl.PlayerT.position.y + _upZipOffSetCheckNoBuild.y;

            _playerControl.AnimControl.WallRunZipStart(true);
        }
        else
        {
            //力を加えるのは、少し上と、壁から少し離れる用に
            Vector3 addDir = Vector3.up + _playerControl.WallRunCheck.WallDir;

            //壁から少し離れるように速度を加える
            _playerControl.Rb.AddForce(addDir * _addFirstSpeed, ForceMode.Impulse);

            //アニメーションの設定
            _playerControl.AnimControl.WallRunZipStart(false);
        }

            //LineRendrerの設定
            _playerControl.LineRenderer.positionCount = 2;
            _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
            _playerControl.LineRenderer.SetPosition(1, _wireHitPoint);
    }

    public void SetLineRenderer()
    {
        if (_isEndZip) return;
        _playerControl.LineRenderer.SetPosition(0, _playerControl.Hads.position);
        _playerControl.LineRenderer.SetPosition(1, _wireHitPoint);
    }

    public void DoUpToFrontZip()
    {
        if (!_isEndUppinng)
        {
            //プレイヤーを壁の方向を向ける
            Quaternion _targetRotation = Quaternion.LookRotation(-_playerControl.WallRunCheck.WallDir, Vector3.up);
            Quaternion toAngle = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, Time.deltaTime * 400);
            toAngle.x = 0;
            toAngle.z = 0;

            //現在の回転と、回転終了との角度を比べる
            float y = Quaternion.Angle(_targetRotation, _playerControl.PlayerT.rotation);

            //一定値まで回転していなかったら回転させる
            if (y > 1)
            {
                _playerControl.PlayerT.rotation = toAngle;
            }


            Debug.Log("UppingToFront");
            //速度を設定
            _playerControl.Rb.velocity = Vector3.up * 30;

            bool isHit = Physics.Raycast(_playerControl.PlayerT.position + _playerfootOffset, -_playerControl.WallRunCheck.WallDir, _upZipOffSetCheckNoBuildRayLong, _wallLayer);

            if (!isHit)
            {
                Debug.Log("EndFrounZip");
                //
                _isEndUppinng = true;

                Vector3 dir = (-_playerControl.WallRunCheck.WallDir) + new Vector3(0, 1, 0);
                //速度をリセット
                _playerControl.Rb.velocity = (dir * 2);


                _playerControl.AnimControl.WallRunZipDo(true);
            }
        }
        else
        {
            _countWaitTime += Time.deltaTime;

            if (_countWaitTime > 0.28f)
            {
                _isEndZip = true;

                Vector3 dir = (-_playerControl.WallRunCheck.WallDir) + new Vector3(0, 1, 0);

                //前に力を加える
                _playerControl.Rb.AddForce(dir.normalized * 30, ForceMode.Impulse);
            }
        }
    }


    public void DoZip()
    {
        //プレイヤーを壁の方向を向ける
        Quaternion _targetRotation = Quaternion.LookRotation(-_playerControl.WallRunCheck.WallDir, Vector3.up);
        Quaternion toAngle = Quaternion.RotateTowards(_playerControl.PlayerT.rotation, _targetRotation, Time.deltaTime * 400);
        toAngle.x = 0;
        toAngle.z = 0;

        //現在の回転と、回転終了との角度を比べる
        float y = Quaternion.Angle(_targetRotation, _playerControl.PlayerT.rotation);

        //一定値まで回転していなかったら回転させる
        if (y > 1)
        {
            _playerControl.PlayerT.rotation = toAngle;
        }



        if (!_isToFarWall)
        {
            //プレイヤーから壁の方向までRayを飛ばす
            bool isHit = Physics.Raycast(_playerControl.PlayerT.position, -_playerControl.WallRunCheck.WallDir, _checkWallRayLong, _wallLayer);

            //壁に当たらなくなったら、離れたということにする
            if (!isHit)
            {
                //離れたことを示す
                _isToFarWall = true;

                //移動する向きを決める
                _zipDir = _upZipHit.point - _playerControl.PlayerT.position;

                //アニメーションの設定
                _playerControl.AnimControl.WallRunZipDo(false);
            }
        }
        else
        {
            Debug.Log("Upping");

            //速度を設定
            _playerControl.Rb.velocity = _zipDir.normalized * _zipSpeed + (_playerControl.WallRunCheck.WallDir * 0.5f);

            //Zip目標地点と自分の距離を求める
            float distance = Mathf.Abs(_upZipHit.point.y - _playerControl.PlayerT.position.y);

            //一定誤差内に入ったら終了
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
        //スタートはプレイヤーの地点+Zip目標地点
        Vector3 rayStart = _playerControl.PlayerT.position + _upZipOffSet;
        //向きは、壁に向かった方向
        Vector3 rayDir = -_playerControl.WallRunCheck.WallDir;

        RaycastHit raycast;

        //Zip出来るところがあるか探す
        bool isHit = Physics.Raycast(rayStart, rayDir, out raycast, _upZipRayLong, _wallLayer);

        //スタートはプレイヤーの地点+Zip目標地点
        Vector3 rayStart2 = _playerControl.PlayerT.position + _upZipOffSet;

        // bool isHitNoBuold = Physics.Raycast(rayStart2, rayDir, out raycast, _upZipOffSetCheckNoBuildRayLong, _wallLayer);

        if (isHit)
        {
            //当たっていた場合のみ情報を渡す
            _upZipHit = raycast;
            _wireHitPoint = raycast.point;
        }
        else
        {
            int roopCount = 1;
            RaycastHit rayHit;

            //絶対にHitさせておく
            Physics.Raycast(_playerControl.PlayerT.position, rayDir, out rayHit, _upZipRayLong, _wallLayer);

            while (true)
            {
                Vector3 addY = new Vector3(0, roopCount * 0.5f, 0);
                //スタートはプレイヤーの地点+Zip目標地点
                Vector3 checkStart = _playerControl.PlayerT.position + addY;

                //Zip出来るところがあるか探す
                bool hit = Physics.Raycast(checkStart, rayDir, out rayHit, _upZipRayLong, _wallLayer);

                if (hit)
                {
                    _wireHitPoint = rayHit.point;
                }
                else
                {
                    break;
                }

                roopCount++;

                //もし当たらなかった際の応急処置
                if(roopCount>50)
                {
                    break;
                }

            }
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
