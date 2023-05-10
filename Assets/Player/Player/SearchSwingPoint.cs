using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SearchSwingPoint : IPlayerAction
{
    [Header("箱のRayを描画するかどうか")]
    [SerializeField]
    private bool _isDrowCube = true;

    [Header("箱のサイズ")]
    [SerializeField] private Vector3 _boxSize;

    [SerializeField] private List<SwingDirPos> _boxPos = new List<SwingDirPos>();



    [Header("自身の左側にワイヤーを飛ばす位置")]
    [SerializeField] private List<SwingDirPos> _swingPosLeft = new List<SwingDirPos>();

    [Header("自身の右側にワイヤーを飛ばす位置")]
    [SerializeField] private List<SwingDirPos> _swingPosRight = new List<SwingDirPos>();

    [Header("ワイヤーの最長の長さ")]
    [SerializeField] private float _maxWireLong = 20;

    [Header("ワイヤーの最短の長さ")]
    [SerializeField] private float _minWireLong = 15;

    [SerializeField] private Transform _Cpos;

    [SerializeField] private LayerMask _layer;

    /// <summary>Swingが出来るかどうか</summary>
    private bool _isCanHit;
    /// <summary>Swingのワイヤーを刺す位置</summary>
    private Vector3 _swingPosition;
    private Vector3 _realSwingPoint;
    public Vector3 RealSwingPoint { get => _realSwingPoint; set => _realSwingPoint = value; }


    public Vector3 SwingPos => _swingPosition;
    public bool IsCanHit => _isCanHit;

    /// <summary>Swingの出来る場所を探す</summary>
    public bool Search()
    {
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        //右入力があった場合、右側を優先して検索
        if (_playerControl.InputManager.HorizontalInput > 0)
        {
            //右側を先に確認
            bool firstCheck = WallSearch(1, _playerControl.PlayerT, horizontalRotation);

            //右側に当たり場所がなかった場合
            if (firstCheck == false)
            {
                //2回目、左側を確認する
                bool secondCheck = WallSearch(-1, _playerControl.PlayerT, horizontalRotation);

                if (secondCheck)
                {
                    return true;
                }　//左側が当たっていたら返す
                else
                {
                    //最終確認。Boxでの確認。右側から
                    bool lastCheckRight = BoxCheck(1, horizontalRotation);
                    if (lastCheckRight)
                    {
                        return true;
                    }   
                    else
                    {
                        return BoxCheck(-1, horizontalRotation);
                    }
                }
            }
            else
            {
                return firstCheck;
            }   //１回目で右側に当たっていたら右側を返す
        }   
        //左側の入力があった場合。確認方法は同じ
        else if (_playerControl.InputManager.HorizontalInput < 0)
        {
            bool firstCheck = WallSearch(-1, _playerControl.PlayerT, horizontalRotation);

            if (firstCheck == false)
            {
                bool secondCheck = WallSearch(1, _playerControl.PlayerT, horizontalRotation);

                if (secondCheck)
                {
                    return true;
                }
                else
                {
                    bool lastCheckLeft = BoxCheck(-1, horizontalRotation);
                    if (lastCheckLeft)
                    {
                        return true;
                    }
                    else
                    {
                        return BoxCheck(1, horizontalRotation);
                    }
                }
            }
            else
            {
                return firstCheck;
            }
        }
        else
        {
            bool firstCheck = WallSearch(1, _playerControl.PlayerT, horizontalRotation);

            if (firstCheck == false)
            {
                bool secondCheck = WallSearch(-1, _playerControl.PlayerT, horizontalRotation);

                if (secondCheck)
                {
                    return true;
                }
                else
                {
                    bool lastCheckRight = BoxCheck(1, horizontalRotation);
                    if (lastCheckRight)
                    {
                        return true;
                    }
                    else
                    {
                        return BoxCheck(-1, horizontalRotation);
                    }
                }
            }
            else
            {
                return firstCheck;
            }
        }
    }


    public bool WallSearch(float inputH, Transform player, Quaternion hRotation)
    {
        //探す場所
        List<SwingDirPos> searchPoints = new List<SwingDirPos>();

        if (inputH == 1)
        {
            searchPoints = _swingPosRight;
        }   //右入力、右側を探すとき
        else if (inputH == -1)
        {
            searchPoints = _swingPosLeft;
        }   //左入力、左側を探すとき

        //高い順に並び変える
        searchPoints.Sort();

        //Swingポイントの探索
        foreach (var searchPos in searchPoints)
        {
            RaycastHit hit;

            //カメラの回転軸を考慮して、例の方向を変換
            Vector3 pos = hRotation * new Vector3(searchPos.Dir.x, searchPos.Dir.y, searchPos.Dir.z);

            Vector3 dir = default;

            if (inputH > 0)
            {
                dir = hRotation * Vector3.right;
            }
            else
            {
                dir = hRotation * Vector3.left;
            }


            //Rayを飛ばす
            bool isHit = Physics.Raycast(_playerControl.PlayerT.position + new Vector3(pos.x, pos.y, pos.z), dir, out hit, _maxWireLong, _layer);

            Debug.DrawRay(_playerControl.PlayerT.position, pos * 30, Color.green);

            //RayがHitしていたら
            if (isHit)
            {
                float distance = Vector3.Distance(hit.point, _playerControl.PlayerT.position);

                //Hit地点までの距離が、ワイヤーの最短距離より長かったら有効
                if (distance >= _minWireLong)
                {
                    _isCanHit = true;
                    _swingPosition = hit.point;



                    Vector3 d = new Vector3(_playerControl.PlayerT.position.x, _swingPosition.y, _playerControl.PlayerT.position.z);

                    Vector3 f = Camera.main.transform.forward;
                    f.y = 0;

                    Vector3 dirPlayer = d + f * 20;

                    _realSwingPoint = dirPlayer;

                    if (_swingPosition == Vector3.zero)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        _isCanHit = false;
        return false;
    }

    public bool BoxCheck(float h, Quaternion hRotation)
    {

        foreach (var boxPos in _boxPos)
        {
            ////Boxでの参照
            Vector3 addCenter = hRotation * new Vector3(boxPos.Dir.x, boxPos.Dir.y, boxPos.Dir.z);
            Vector3 boxCenter = _playerControl.PlayerT.position + addCenter;

            RaycastHit boxHit;

            Vector3 dir = default;

            if (h == 1)
            {
                dir = hRotation * Vector3.right;
            }
            else
            {
                dir = hRotation * Vector3.left;
            }


            //Castを飛ばす
            bool isHitBox = Physics.BoxCast(boxCenter, _boxSize, dir, out boxHit, Quaternion.identity, _maxWireLong, _layer);

            if (isHitBox)
            {
                float distance = Vector3.Distance(boxHit.point, _playerControl.PlayerT.position);

                //Hit地点までの距離が、ワイヤーの最短距離より長かったら有効
                if (distance >= _minWireLong)
                {
                    _isCanHit = true;
                    _swingPosition = boxHit.point;

                    Vector3 d = new Vector3(_playerControl.PlayerT.position.x, _swingPosition.y, _playerControl.PlayerT.position.z);

                    Vector3 cameraFoward = Camera.main.transform.forward;
                    cameraFoward.y = 0;

                    Vector3 dirPlayer = d + cameraFoward * 20;

                    _realSwingPoint = dirPlayer;

                    if (_swingPosition == Vector3.zero)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }



    public void OnDrawGizmos(Transform player)
    {
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        foreach (var a in _boxPos)
        {
            Gizmos.color = Color.blue;

            Quaternion cameraR = default;

            var q = Camera.main.transform.rotation.eulerAngles;
            q.x = 0f;
            cameraR = Quaternion.Euler(q);


            Gizmos.matrix = Matrix4x4.TRS(player.position, cameraR, player.localScale);

            Gizmos.DrawCube(a.Dir, _boxSize);

            Gizmos.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);
        }

        foreach (var a in _swingPosLeft)
        {
            Gizmos.color = Color.red;

            Vector3 newA = horizontalRotation * new Vector3(a.Dir.x, a.Dir.y, a.Dir.z);
            Vector3 dir = horizontalRotation * Vector3.right;


            Gizmos.DrawRay(player.position + new Vector3(newA.x, newA.y, newA.z), dir);
        }

        foreach (var a in _swingPosRight)
        {

            Vector3 newA = horizontalRotation * new Vector3(a.Dir.x, a.Dir.y, a.Dir.z);
            Vector3 dir = horizontalRotation * Vector3.left;

            Gizmos.DrawRay(player.position + new Vector3(newA.x, newA.y, newA.z), dir);
        }
    }




}

[System.Serializable]
public class SwingDirPos : IComparable<SwingDirPos>
{
    public Vector3 Dir;


    public int CompareTo(SwingDirPos other)
    {
        if (this.Dir.y < other.Dir.y)
        {
            return -1;  // 自分の ID が小さい時は「自分の方が前」とする
        }
        else if (this.Dir.y > other.Dir.y)
        {
            return 1;  // 自分の ID が大きい時は「自分の方が後ろ」とする
        }
        else if (this.Dir.y == other.Dir.y) // ID が同じなら「同じ」とする
        {
            if (this.Dir.z < other.Dir.z)
            {
                return -1;  // 自分の ID が小さい時は「自分の方が前」とする
            }
            else if (this.Dir.z > other.Dir.z)
            {
                return 1;  // 自分の ID が大きい時は「自分の方が後ろ」とする
            }
            else if (this.Dir.z == other.Dir.z)
            {
                if (this.Dir.x < other.Dir.x)
                {
                    return -1;  // 自分の ID が小さい時は「自分の方が前」とする
                }
                else if (this.Dir.x > other.Dir.x)
                {
                    return 1;  // 自分の ID が大きい時は「自分の方が後ろ」とする
                }
                else
                {
                    return 0;
                }
            }
            return 0;
        }
        else
        {
            return 0;
        }
    }
}
