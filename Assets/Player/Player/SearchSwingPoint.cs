using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class SearchSwingPoint : IPlayerAction
{
    [Header("箱のサイズ")]
    [SerializeField] private Vector3 _boxSize;

    [Header("右側の")]
    [SerializeField] private Vector3 _rightP;

    [Header("左側の")]
    [SerializeField] private Vector3 _leftP;

    [Header("自身の左側にワイヤーを飛ばす位置")]
    [SerializeField] private List<SwingDirPos> _swingPosLeft = new List<SwingDirPos>();

    [Header("自身の右側にワイヤーを飛ばす位置")]
    [SerializeField] private List<SwingDirPos> _swingPosRight = new List<SwingDirPos>();

    [Header("ワイヤーの最長の長さ")]
    [SerializeField] private float _maxWireLong = 20;

    [Header("ワイヤーの最短の長さ")]
    [SerializeField] private float _minWireLong = 15;


    [SerializeField] private LayerMask _layer;

    /// <summary>Swingが出来るかどうか</summary>
    private bool _isCanHit;
    /// <summary>Swingのワイヤーを刺す位置</summary>
    private Vector3 _swingPosition;

    public Vector3 SwingPos => _swingPosition;
    public bool IsCanHit => _isCanHit;

    /// <summary>Swingの出来る場所を探す</summary>
    public void Search(Transform player)
    {
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

        if (_playerControl.InputManager.HorizontalInput > 0)
        {
            Vector3 p = WallSearch(1, player, horizontalRotation);

            if (p == Vector3.zero)
            {
                WallSearch(-1, player, horizontalRotation);
            }
        }
        else if (_playerControl.InputManager.HorizontalInput < 0)
        {
            Vector3 p = WallSearch(-1, player, horizontalRotation);

            if (p == Vector3.zero)
            {
                WallSearch(1, player, horizontalRotation);
            }
        }
        else
        {
            Vector3 p = WallSearch(1, player, horizontalRotation);

            if (p == Vector3.zero)
            {
                WallSearch(-1, player, horizontalRotation);
            }
        }
    }


    public Vector3 WallSearch(float inputH, Transform player, Quaternion hRotation)
    {
        //探す場所
        List<SwingDirPos> searchPoints = new List<SwingDirPos>();

        Vector3 searchBoxPos = default;

        if (inputH == 1)
        {
            searchPoints = _swingPosRight;
            searchBoxPos = _rightP;
        }   //右入力、右側を探すとき
        else if (inputH == -1)
        {
            searchPoints = _swingPosLeft;
            searchBoxPos = _leftP;
        }   //左入力、左側を探すとき
        //else
        //{
        //    List<SwingDirPos> tmpSearchPoint = new List<SwingDirPos>();

        //    foreach (var leftPos in _swingPosLeft)
        //    {
        //        tmpSearchPoint.Add(leftPos);
        //    }
        //    foreach (var rightPos in _swingPosRight)
        //    {
        //        tmpSearchPoint.Add(rightPos);
        //    }
        //    searchPoints = tmpSearchPoint;
        //}   //入力無し、高い所から探す

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
                    float groundY = _playerControl.GroundCheck.IsSwingPlayerToGroundOfLong();

                    //プレイヤーと地面の距離>ワイヤーの長さ
                    //の時有効
                    //  if (hit.point.y - distance >= groundY)
                    // {
                    _isCanHit = true;
                    _swingPosition = hit.point;
                    return _swingPosition;
                    //  }
                }
            }
        }

        ////Boxでの参照
        //Vector3 addCenter = hRotation * new Vector3(searchBoxPos.x, searchBoxPos.y, searchBoxPos.z);
        //Vector3 boxDir = hRotation * new Vector3(0, 0, 1);
        //Vector3 boxCenter = _playerControl.PlayerT.position + addCenter;

        //RaycastHit boxHit;

        ////Castを飛ばす
        //bool isHitBox = Physics.BoxCast(boxCenter, _boxSize, boxDir, out boxHit, Quaternion.identity, _maxWireLong, _layer);

        //if (isHitBox)
        //{
        //    float distance = Vector3.Distance(boxHit.point, _playerControl.PlayerT.position);

        //    //Hit地点までの距離が、ワイヤーの最短距離より長かったら有効
        //    if (distance >= _minWireLong)
        //    {
        //        float groundY = _playerControl.GroundCheck.IsSwingPlayerToGroundOfLong();

        //        //プレイヤーと地面の距離>ワイヤーの長さ
        //        //の時有効
        //        //    if (boxHit.point.y - distance >= groundY)
        //        //    {
        //        _isCanHit = true;
        //        _swingPosition = boxHit.point;
        //        // Debug.Log($"{inputH}:BoxHit");
        //        return _swingPosition;
        //        //  }
        //    }
        //}

        _isCanHit = false;
        return Vector3.zero;
    }


    public void OnDrawGizmos(Transform player)
    {
        var horizontalRotation = Quaternion.AngleAxis(Camera.main.transform.eulerAngles.y, Vector3.up);

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
